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
    private GameObject m_DynamicBG, m_ShieldBG, m_MoveBG, m_FireBG, m_LengthBG;

    public List<Sprite> m_LevelImages;
    public List<string> m_LevelScenes;

    private GameObject m_EventSystem;

    private bool m_Mutators = false;

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
        GAMELENGTH
    }

    private enum LevelSelected
    {
        LEVEL1,
        LEVEL2,
    }

    private SelectedButton m_CurrentButton;

    private MutatorButtons m_MutatorButtons;

    private LevelSelected m_LevelSelected;

    void Start()
    {
        m_EventSystem = GameObject.Find("EventSystem");
        m_EventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(GameObject.Find("PlayButton"));
        m_CurrentButton = SelectedButton.PLAYBUTTON;
    }

    void Update()
    {
        if (!m_Mutators)
        {
            UpdateMain();
        }
        else UpdateMutators();

    }

    public void ClickPlay()
    {
        
        m_MenuCanvas.SetActive(false);
        m_MutatorCanvas.SetActive(true);
        m_MutatorButtons = MutatorButtons.LEVELSELECT;
        m_LevelSelected = LevelSelected.LEVEL1;
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

                break;
            case MutatorButtons.GAMELENGTH:

                #region GAMEUPDOWN
                if (m_LengthSlider.transform.GetChild(0).GetComponent<Outline>().enabled == false)
                {
                    m_LengthSlider.transform.GetChild(0).GetComponent<Outline>().enabled = true;
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

                break;
            default:
                break;
        }

    }

    public void ClickOptions()
    {


    }

    public void ClickQuit()
    {
        Application.Quit();
    }



}
