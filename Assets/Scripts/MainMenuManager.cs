using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;
using InControl;

public class MainMenuManager : MonoBehaviour {

    public GameObject m_MenuCanvas, m_MutatorCanvas;

    public GameObject m_LevelSelect, m_DynamicToggle, m_ShieldSlider, m_MoveSlider, m_FireSlider, m_LengthSlider;

    public int m_SelectedShieldHealth, m_SelectedMoveSpeed, m_SelectedFireRate, m_SelectedGameLength;
    public int m_PlayersReady = 0;
    public InputDevice[] m_MenuDevices;

    public GameObject m_PlayerReadyIcons;


    public List<Sprite> m_LevelImages;
    public List<string> m_LevelScenes;

    public Button m_StartButton;

    private GameObject m_EventSystem;


    private bool m_Mutators = false;

    private bool m_MenuScene = true;

    private enum SelectedButton
    {
        PLAYBUTTON,
        OPTIONSBUTTON,
        EXITBUTTON
    }

    private enum MutatorButtons
    {
        LEVELSELECT,
        DYNAMICLEVEL,
        SHIELDHEALTH,
        MOVESPEED,
        FIRERATE,
        GAMELENGTH,
        STARTGAME
    }

    private enum LevelSelected
    {
        Level1,
        Level2,
    }

    private SelectedButton m_CurrentButton;

    private MutatorButtons m_MutatorButtons;

    private LevelSelected m_LevelSelected;

    void Start()
    {
        DontDestroyOnLoad(transform);
        m_MenuDevices = new InputDevice[4];
        m_EventSystem = GameObject.Find("EventSystem");
        m_EventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(GameObject.Find("PlayButton"));
        m_CurrentButton = SelectedButton.PLAYBUTTON;
        m_StartButton.interactable = false;
    }

    void Update()
    {
        if (m_MenuScene)
        {
            if (!m_Mutators)
            {
                UpdateMain();
            }
            else UpdateMutators();

            UpdateControllers();
        }
    }

    public void ClickPlay()
    {
        
        m_MenuCanvas.SetActive(false);
        m_MutatorCanvas.SetActive(true);
        m_MutatorButtons = MutatorButtons.LEVELSELECT;
        m_LevelSelected = LevelSelected.Level1;
        m_EventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(m_LevelSelect);
        m_Mutators = true;
        //SceneManager.LoadScene("AlphaLevel", LoadSceneMode.Single);
    }

    void UpdateMain()
    {
        switch (m_CurrentButton)
        {
            case SelectedButton.PLAYBUTTON:

                if (InputManager.ActiveDevice.DPadDown.WasPressed)
                {
                    m_EventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(GameObject.Find("OptionsButton"));
                    m_CurrentButton = SelectedButton.OPTIONSBUTTON;
                }
                else if (InputManager.ActiveDevice.DPadUp.WasPressed)
                {
                    break;
                }
                break;

            case SelectedButton.OPTIONSBUTTON:
                if (InputManager.ActiveDevice.DPadUp.WasPressed)
                {
                    m_EventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(GameObject.Find("PlayButton"));
                    m_CurrentButton = SelectedButton.PLAYBUTTON;
                }
                else if (InputManager.ActiveDevice.DPadDown.WasPressed)
                {
                    m_EventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(GameObject.Find("ExitButton"));
                    m_CurrentButton = SelectedButton.EXITBUTTON;
                }
                break;

            case SelectedButton.EXITBUTTON:
                if (InputManager.ActiveDevice.DPadDown.WasPressed)
                {
                    break;
                }
                else if (InputManager.ActiveDevice.DPadUp.WasPressed)
                {
                    m_EventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(GameObject.Find("OptionsButton"));
                    m_CurrentButton = SelectedButton.OPTIONSBUTTON;
                }
                break;
        }
    }

    void UpdateMutators()
    {
        switch (m_MutatorButtons)
        {
            case MutatorButtons.LEVELSELECT:

                #region LEVELUPDOWNLEFTRIGHT
                if (m_LevelSelect.GetComponent<Outline>().enabled == false)
                {
                    m_LevelSelect.GetComponent<Outline>().enabled = true;
                }

                if(InputManager.ActiveDevice.LeftStickRight.WasPressed || InputManager.ActiveDevice.DPadRight.WasPressed)
                {
                    m_LevelSelected++;
                    if((int)m_LevelSelected > (int)Enum.GetValues(typeof(LevelSelected)).Cast<LevelSelected>().Max())
                    {
                        m_LevelSelected = Enum.GetValues(typeof(LevelSelected)).Cast<LevelSelected>().Min();
                    }
                }
                else if(InputManager.ActiveDevice.LeftStickLeft.WasPressed || InputManager.ActiveDevice.DPadLeft.WasPressed)
                {
                    m_LevelSelected--;
                    if ((int)m_LevelSelected < (int)Enum.GetValues(typeof(LevelSelected)).Cast<LevelSelected>().Min())
                    {
                        m_LevelSelected = Enum.GetValues(typeof(LevelSelected)).Cast<LevelSelected>().Max();
                    }
                }
                
                m_LevelSelect.GetComponent<Image>().sprite = m_LevelImages[(int)m_LevelSelected];

                if(InputManager.ActiveDevice.LeftStickDown.WasPressed || InputManager.ActiveDevice.DPadDown.WasPressed)
                {
                    m_LevelSelect.GetComponent<Outline>().enabled = false;
                    m_MutatorButtons++;
                }
                #endregion
                
                break;
            case MutatorButtons.DYNAMICLEVEL:

                #region DYNAMICUPDOWN
                if (m_DynamicToggle.transform.GetChild(0).GetComponent<Outline>().enabled == false)
                {
                    m_DynamicToggle.transform.GetChild(0).GetComponent<Outline>().enabled = true;
                }


                if (InputManager.ActiveDevice.LeftStickDown.WasPressed || InputManager.ActiveDevice.DPadDown.WasPressed)
                {
                    m_DynamicToggle.transform.GetChild(0).GetComponent<Outline>().enabled = false;
                    m_MutatorButtons++;
                }
                if (InputManager.ActiveDevice.LeftStickUp.WasPressed || InputManager.ActiveDevice.DPadUp.WasPressed)
                {
                    m_DynamicToggle.transform.GetChild(0).GetComponent<Outline>().enabled = false;
                    m_MutatorButtons--;
                }
                #endregion

                #region DYNAMICTOGGLE
                if(InputManager.ActiveDevice.Action1.WasPressed)
                {
                    if (m_DynamicToggle.transform.GetChild(1).GetComponent<Toggle>().isOn)
                    {
                        m_DynamicToggle.transform.GetChild(1).GetComponent<Toggle>().isOn = false;
                    }
                    else m_DynamicToggle.transform.GetChild(1).GetComponent<Toggle>().isOn = true;
                }
                #endregion

                break;
            case MutatorButtons.SHIELDHEALTH:

                #region SHIELDUPDOWN
                if (m_ShieldSlider.transform.GetChild(0).GetComponent<Outline>().enabled == false)
                {
                    m_ShieldSlider.transform.GetChild(0).GetComponent<Outline>().enabled = true;
                }


                if (InputManager.ActiveDevice.LeftStickDown.WasPressed || InputManager.ActiveDevice.DPadDown.WasPressed)
                {
                    m_ShieldSlider.transform.GetChild(0).GetComponent<Outline>().enabled = false;
                    m_MutatorButtons++;
                }
                if (InputManager.ActiveDevice.LeftStickUp.WasPressed || InputManager.ActiveDevice.DPadUp.WasPressed)
                {
                    m_ShieldSlider.transform.GetChild(0).GetComponent<Outline>().enabled = false;
                    m_MutatorButtons--;
                }
                #endregion
                
                #region SHIELDLEFTRIGHT
                if (InputManager.ActiveDevice.LeftStickLeft.WasPressed || InputManager.ActiveDevice.DPadLeft.WasPressed)
                {
                    if(m_ShieldSlider.transform.GetChild(1).GetComponent<Slider>().value > 1)
                        m_ShieldSlider.transform.GetChild(1).GetComponent<Slider>().value--;
                    
                }
                else if(InputManager.ActiveDevice.LeftStickRight.WasPressed || InputManager.ActiveDevice.DPadRight.WasPressed)
                {
                    if (m_ShieldSlider.transform.GetChild(1).GetComponent<Slider>().value < 5)
                        m_ShieldSlider.transform.GetChild(1).GetComponent<Slider>().value++;
                }
                #endregion

                m_SelectedShieldHealth = (int)m_ShieldSlider.transform.GetChild(1).GetComponent<Slider>().value;

                break;
            case MutatorButtons.MOVESPEED:

                #region MOVEUPDOWN
                if (m_MoveSlider.transform.GetChild(0).GetComponent<Outline>().enabled == false)
                {
                    m_MoveSlider.transform.GetChild(0).GetComponent<Outline>().enabled = true;
                }


                if (InputManager.ActiveDevice.LeftStickDown.WasPressed || InputManager.ActiveDevice.DPadDown.WasPressed)
                {
                    m_MoveSlider.transform.GetChild(0).GetComponent<Outline>().enabled = false;
                    m_MutatorButtons++;
                }
                if (InputManager.ActiveDevice.LeftStickUp.WasPressed || InputManager.ActiveDevice.DPadUp.WasPressed)
                {
                    m_MoveSlider.transform.GetChild(0).GetComponent<Outline>().enabled = false;
                    m_MutatorButtons--;
                }
                #endregion

                #region MOVELEFTRIGHT
                if (InputManager.ActiveDevice.LeftStickLeft.WasPressed || InputManager.ActiveDevice.DPadLeft.WasPressed)
                {
                    if (m_MoveSlider.transform.GetChild(1).GetComponent<Slider>().value > 1)
                        m_MoveSlider.transform.GetChild(1).GetComponent<Slider>().value--;

                }
                else if (InputManager.ActiveDevice.LeftStickRight.WasPressed || InputManager.ActiveDevice.DPadRight.WasPressed)
                {
                    if (m_MoveSlider.transform.GetChild(1).GetComponent<Slider>().value < 4)
                        m_MoveSlider.transform.GetChild(1).GetComponent<Slider>().value++;
                }
                #endregion

                m_SelectedMoveSpeed = (int)m_MoveSlider.transform.GetChild(1).GetComponent<Slider>().value;

                break;
            case MutatorButtons.FIRERATE:

                #region FIREUPDOWN
                if (m_FireSlider.transform.GetChild(0).GetComponent<Outline>().enabled == false)
                {
                    m_FireSlider.transform.GetChild(0).GetComponent<Outline>().enabled = true;
                }


                if (InputManager.ActiveDevice.LeftStickDown.WasPressed || InputManager.ActiveDevice.DPadDown.WasPressed)
                {
                    m_FireSlider.transform.GetChild(0).GetComponent<Outline>().enabled = false;
                    m_MutatorButtons++;
                }
                if (InputManager.ActiveDevice.LeftStickUp.WasPressed || InputManager.ActiveDevice.DPadUp.WasPressed)
                {
                    m_FireSlider.transform.GetChild(0).GetComponent<Outline>().enabled = false;
                    m_MutatorButtons--;
                }
                #endregion

                #region FIRELEFTRIGHT
                if (InputManager.ActiveDevice.LeftStickLeft.WasPressed || InputManager.ActiveDevice.DPadLeft.WasPressed)
                {
                    if (m_FireSlider.transform.GetChild(1).GetComponent<Slider>().value > 1)
                        m_FireSlider.transform.GetChild(1).GetComponent<Slider>().value--;

                }
                else if (InputManager.ActiveDevice.LeftStickRight.WasPressed || InputManager.ActiveDevice.DPadRight.WasPressed)
                {
                    if (m_FireSlider.transform.GetChild(1).GetComponent<Slider>().value < 10)
                        m_FireSlider.transform.GetChild(1).GetComponent<Slider>().value++;
                }

                #endregion

                m_SelectedFireRate = (int)m_FireSlider.transform.GetChild(1).GetComponent<Slider>().value;

                break;
            case MutatorButtons.GAMELENGTH:

                #region GAMEUPDOWN
                if (m_LengthSlider.transform.GetChild(0).GetComponent<Outline>().enabled == false)
                {
                    m_LengthSlider.transform.GetChild(0).GetComponent<Outline>().enabled = true;
                }

                if (InputManager.ActiveDevice.LeftStickDown.WasPressed && m_PlayersReady > 0 || InputManager.ActiveDevice.DPadDown.WasPressed && m_PlayersReady > 0)
                {
                    m_LengthSlider.transform.GetChild(0).GetComponent<Outline>().enabled = false;
                    m_MutatorButtons++;
                }
                if (InputManager.ActiveDevice.LeftStickUp.WasPressed || InputManager.ActiveDevice.DPadUp.WasPressed)
                {
                    m_LengthSlider.transform.GetChild(0).GetComponent<Outline>().enabled = false;
                    m_MutatorButtons--;
                }
                #endregion

                #region GAMELEFTRIGHT
                if (InputManager.ActiveDevice.LeftStickLeft.WasPressed || InputManager.ActiveDevice.DPadLeft.WasPressed)
                {
                    if (m_LengthSlider.transform.GetChild(1).GetComponent<Slider>().value > 1)
                        m_LengthSlider.transform.GetChild(1).GetComponent<Slider>().value--;

                }
                else if (InputManager.ActiveDevice.LeftStickRight.WasPressed || InputManager.ActiveDevice.DPadRight.WasPressed)
                {
                    if (m_LengthSlider.transform.GetChild(1).GetComponent<Slider>().value < 10)
                        m_LengthSlider.transform.GetChild(1).GetComponent<Slider>().value++;
                }

                #endregion

                m_SelectedGameLength = (int)m_LengthSlider.transform.GetChild(1).GetComponent<Slider>().value;

                break;
            case MutatorButtons.STARTGAME:
                #region STARTUPDOWN
                if (m_StartButton.interactable == false)
                {
                    m_StartButton.interactable = true;
                    
                }
                m_StartButton.Select();
                if (InputManager.ActiveDevice.LeftStickUp.WasPressed || InputManager.ActiveDevice.DPadUp.WasPressed)
                {
                    m_LengthSlider.transform.GetChild(0).GetComponent<Outline>().enabled = false;
                    m_EventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
                    m_MutatorButtons--;
                }

                #endregion
                break;

            default:
                break;
        }

    }

    void UpdateControllers()
    {
        if(InputManager.ActiveDevice.CommandWasPressed && !m_MenuDevices.Contains(InputManager.ActiveDevice) && m_PlayersReady < 5)
        {
            UpdatePlayerCount(InputManager.ActiveDevice);
        }
    }

    void UpdatePlayerCount(InputDevice _activeDevice)
    {
        m_PlayersReady++;
        m_MenuDevices[m_PlayersReady -1] = _activeDevice;
        var _playerIcon = m_PlayerReadyIcons.transform.GetChild(m_PlayersReady - 1);
        _playerIcon.transform.GetChild(0).gameObject.SetActive(false);
        _playerIcon.transform.GetChild(1).gameObject.SetActive(true);
    }

    public void ClickOptions()
    {

    }

    public void ClickQuit()
    {
        Application.Quit();
    }

    public void LoadLevel()
    {
        if (m_DynamicToggle.transform.GetChild(1).GetComponent<Toggle>().isOn)
        {
            SceneManager.LoadScene(m_LevelSelected.ToString() + "Dynamic", LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadScene(m_LevelSelected.ToString(), LoadSceneMode.Single);
        }

        m_MenuScene = false;
    }
}
