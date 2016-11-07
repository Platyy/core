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

    public GameObject m_LevelSelect, m_ShieldSlider, m_MoveSlider, m_FireSlider, 
        m_LengthSlider, m_LevelHighlight, m_BulletSlider, m_ShieldRotateSlider, m_RoundSlider, m_PresetSlider;

    public int m_SelectedShieldHealth, m_SelectedMoveSpeed, m_SelectedFireRate, m_SelectedGameLength, m_SelectedBulletSpeed, m_SelectedShieldRotateSpeed, m_SelectedRounds, m_SuddenDeath;
    public int m_PlayersReady = 0;
    public InputDevice[] m_MenuDevices;

    public GameObject m_PlayerReadyIcons;

    public GameObject m_BGSphere;

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
        PRESET,
        SHIELDHEALTH,
        MOVESPEED,
        FIRERATE,
        BULLETSPEED,
        SHIELDSPEED,
        GAMELENGTH,
        ROUNDS,
        STARTGAME
    }

    Dictionary<Presets, int> m_PresetDefault = new Dictionary<Presets, int>(), m_PresetSniper = new Dictionary<Presets, int>(), m_PresetTactical = new Dictionary<Presets, int>(),
        m_PresetBulletHell = new Dictionary<Presets, int>(), m_PresetSuddenDeath = new Dictionary<Presets, int>();

    private enum Presets
    {
        SHIELDHEALTH,
        MOVESPEED,
        FIREDELAY,
        BULLETSPEED,
        SHIELDROTSPEED,
        NOSHIELDS
    }

    private enum SelectedPreset
    {
        DEFAULT,
        SNIPER,
        TACTICAL,
        BULLET_HELL,
        SUDDEN_DEATH
    }


    private enum LevelSelected
    {
        Level1,
        Level2,
        Level3,
        Level4,
        Level5,
        Level6,
    }

    private SelectedButton m_CurrentButton;

    private MutatorButtons m_MutatorButtons;

    private LevelSelected m_LevelSelected;

    private Presets m_Presets;

    private SelectedPreset m_SelectedPreset;

    void Start()
    {
        DontDestroyOnLoad(transform);
        m_MenuDevices = new InputDevice[4];
        m_EventSystem = GameObject.Find("EventSystem");
        m_EventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(GameObject.Find("PlayButton"));
        m_CurrentButton = SelectedButton.PLAYBUTTON;
        m_StartButton.interactable = false;
        SetupPresetDictionary();
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
        if (m_BGSphere.activeInHierarchy)
        {
            m_BGSphere.SetActive(false);
        }
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
        if(!m_BGSphere.activeInHierarchy)
        {
            m_BGSphere.SetActive(true);
        }
        switch (m_MutatorButtons)
        {
            case MutatorButtons.LEVELSELECT:

                #region LEVELUPDOWNLEFTRIGHT
                if (m_LevelHighlight.GetComponent<Outline>().enabled == false)
                {
                    m_LevelHighlight.GetComponent<Outline>().enabled = true;
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
                    m_LevelHighlight.GetComponent<Outline>().enabled = false;
                    m_MutatorButtons++;
                }
                #endregion
                
                break;
            case MutatorButtons.PRESET:

                #region PRESETUPDOWN
                if (m_PresetSlider.transform.GetChild(0).GetComponent<Outline>().enabled == false)
                {
                    m_PresetSlider.transform.GetChild(0).GetComponent<Outline>().enabled = true;
                }


                if (InputManager.ActiveDevice.LeftStickDown.WasPressed || InputManager.ActiveDevice.DPadDown.WasPressed)
                {
                    m_PresetSlider.transform.GetChild(0).GetComponent<Outline>().enabled = false;
                    m_MutatorButtons++;
                }
                if (InputManager.ActiveDevice.LeftStickUp.WasPressed || InputManager.ActiveDevice.DPadUp.WasPressed)
                {
                    m_PresetSlider.transform.GetChild(0).GetComponent<Outline>().enabled = false;
                    m_MutatorButtons--;
                }
                #endregion

                #region PRESETLEFTRIGHT
                if (InputManager.ActiveDevice.LeftStickLeft.WasPressed || InputManager.ActiveDevice.DPadLeft.WasPressed)
                {
                    if (m_PresetSlider.transform.GetChild(1).GetComponent<Slider>().value > 1)
                    {
                        m_PresetSlider.transform.GetChild(1).GetComponent<Slider>().value--;
                        m_PresetSuddenDeath[Presets.NOSHIELDS] = 0;
                    }

                }
                else if (InputManager.ActiveDevice.LeftStickRight.WasPressed || InputManager.ActiveDevice.DPadRight.WasPressed)
                {
                    if (m_PresetSlider.transform.GetChild(1).GetComponent<Slider>().value < 5)
                    {
                        m_PresetSuddenDeath[Presets.NOSHIELDS] = 0;
                        m_PresetSlider.transform.GetChild(1).GetComponent<Slider>().value++;
                        if(m_PresetSlider.transform.GetChild(1).GetComponent<Slider>().value == 5) // On Sudden Death
                        {
                            int _val;
                            if (m_PresetSuddenDeath.TryGetValue(Presets.NOSHIELDS, out _val))
                            {
                                m_PresetSuddenDeath[Presets.NOSHIELDS] = 1;
                            }
                        }
                        else
                        {
                            m_PresetSuddenDeath[Presets.NOSHIELDS] = 0;
                        }
                    }
                }
                #endregion

                #region PRESETS
                m_SelectedPreset = (SelectedPreset)m_PresetSlider.transform.GetChild(1).GetComponent<Slider>().value - 1;
                switch (m_SelectedPreset)
                {
                    case SelectedPreset.DEFAULT:
                        m_ShieldSlider.transform.GetChild(1).GetComponent<Slider>().value =       m_PresetDefault[Presets.SHIELDHEALTH];
                        m_MoveSlider.transform.GetChild(1).GetComponent<Slider>().value =         m_PresetDefault[Presets.MOVESPEED];
                        m_FireSlider.transform.GetChild(1).GetComponent<Slider>().value =         m_PresetDefault[Presets.FIREDELAY];
                        m_BulletSlider.transform.GetChild(1).GetComponent<Slider>().value =       m_PresetDefault[Presets.BULLETSPEED];
                        m_ShieldRotateSlider.transform.GetChild(1).GetComponent<Slider>().value = m_PresetDefault[Presets.SHIELDROTSPEED];
                        break;
                    case SelectedPreset.SNIPER:
                        m_ShieldSlider.transform.GetChild(1).GetComponent<Slider>().value =       m_PresetSniper[Presets.SHIELDHEALTH];
                        m_MoveSlider.transform.GetChild(1).GetComponent<Slider>().value = m_PresetSniper[Presets.MOVESPEED];
                        m_FireSlider.transform.GetChild(1).GetComponent<Slider>().value = m_PresetSniper[Presets.FIREDELAY];
                        m_BulletSlider.transform.GetChild(1).GetComponent<Slider>().value = m_PresetSniper[Presets.BULLETSPEED];
                        m_ShieldRotateSlider.transform.GetChild(1).GetComponent<Slider>().value = m_PresetSniper[Presets.SHIELDROTSPEED];
                        break;

                    case SelectedPreset.TACTICAL:
                        m_ShieldSlider.transform.GetChild(1).GetComponent<Slider>().value = m_PresetTactical[Presets.SHIELDHEALTH];
                        m_MoveSlider.transform.GetChild(1).GetComponent<Slider>().value = m_PresetTactical[Presets.MOVESPEED];
                        m_FireSlider.transform.GetChild(1).GetComponent<Slider>().value = m_PresetTactical[Presets.FIREDELAY];
                        m_BulletSlider.transform.GetChild(1).GetComponent<Slider>().value = m_PresetTactical[Presets.BULLETSPEED];
                        m_ShieldRotateSlider.transform.GetChild(1).GetComponent<Slider>().value = m_PresetTactical[Presets.SHIELDROTSPEED];
                        break;

                    case SelectedPreset.BULLET_HELL:
                        m_ShieldSlider.transform.GetChild(1).GetComponent<Slider>().value = m_PresetBulletHell[Presets.SHIELDHEALTH];
                        m_MoveSlider.transform.GetChild(1).GetComponent<Slider>().value = m_PresetBulletHell[Presets.MOVESPEED];
                        m_FireSlider.transform.GetChild(1).GetComponent<Slider>().value = m_PresetBulletHell[Presets.FIREDELAY];
                        m_BulletSlider.transform.GetChild(1).GetComponent<Slider>().value = m_PresetBulletHell[Presets.BULLETSPEED];
                        m_ShieldRotateSlider.transform.GetChild(1).GetComponent<Slider>().value = m_PresetBulletHell[Presets.SHIELDROTSPEED];
                        break;
                    case SelectedPreset.SUDDEN_DEATH:
                        m_ShieldSlider.transform.GetChild(1).GetComponent<Slider>().value = m_PresetSuddenDeath[Presets.SHIELDHEALTH];
                        m_MoveSlider.transform.GetChild(1).GetComponent<Slider>().value = m_PresetSuddenDeath[Presets.MOVESPEED];
                        m_FireSlider.transform.GetChild(1).GetComponent<Slider>().value = m_PresetSuddenDeath[Presets.FIREDELAY];
                        m_BulletSlider.transform.GetChild(1).GetComponent<Slider>().value = m_PresetSuddenDeath[Presets.BULLETSPEED];
                        m_ShieldRotateSlider.transform.GetChild(1).GetComponent<Slider>().value = m_PresetSuddenDeath[Presets.SHIELDROTSPEED];
                        break;

                    default:
                        break;
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

                if (InputManager.ActiveDevice.LeftStickDown.WasPressed || InputManager.ActiveDevice.DPadDown.WasPressed)
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

            case MutatorButtons.BULLETSPEED:

                #region BULLETUPDOWN
                if (m_BulletSlider.transform.GetChild(0).GetComponent<Outline>().enabled == false)
                {
                    m_BulletSlider.transform.GetChild(0).GetComponent<Outline>().enabled = true;
                }


                if (InputManager.ActiveDevice.LeftStickDown.WasPressed || InputManager.ActiveDevice.DPadDown.WasPressed)
                {
                    m_BulletSlider.transform.GetChild(0).GetComponent<Outline>().enabled = false;
                    m_MutatorButtons++;
                }
                if (InputManager.ActiveDevice.LeftStickUp.WasPressed || InputManager.ActiveDevice.DPadUp.WasPressed)
                {
                    m_BulletSlider.transform.GetChild(0).GetComponent<Outline>().enabled = false;
                    m_MutatorButtons--;
                }
                #endregion

                #region BULLETLEFTRIGHT
                if (InputManager.ActiveDevice.LeftStickLeft.WasPressed || InputManager.ActiveDevice.DPadLeft.WasPressed)
                {
                    if (m_BulletSlider.transform.GetChild(1).GetComponent<Slider>().value > 1)
                        m_BulletSlider.transform.GetChild(1).GetComponent<Slider>().value--;

                }
                else if (InputManager.ActiveDevice.LeftStickRight.WasPressed || InputManager.ActiveDevice.DPadRight.WasPressed)
                {
                    if (m_BulletSlider.transform.GetChild(1).GetComponent<Slider>().value < 5)
                        m_BulletSlider.transform.GetChild(1).GetComponent<Slider>().value++;
                }

                #endregion

                m_SelectedBulletSpeed = (int)m_BulletSlider.transform.GetChild(1).GetComponent<Slider>().value;
                break;

            case MutatorButtons.SHIELDSPEED:

                #region SHIELDSPEEDUPDOWN
                if (m_ShieldRotateSlider.transform.GetChild(0).GetComponent<Outline>().enabled == false)
                {
                    m_ShieldRotateSlider.transform.GetChild(0).GetComponent<Outline>().enabled = true;
                }


                if (InputManager.ActiveDevice.LeftStickDown.WasPressed || InputManager.ActiveDevice.DPadDown.WasPressed)
                {
                    m_ShieldRotateSlider.transform.GetChild(0).GetComponent<Outline>().enabled = false;
                    m_MutatorButtons++;
                }
                if (InputManager.ActiveDevice.LeftStickUp.WasPressed || InputManager.ActiveDevice.DPadUp.WasPressed)
                {
                    m_ShieldRotateSlider.transform.GetChild(0).GetComponent<Outline>().enabled = false;
                    m_MutatorButtons--;
                }
                #endregion

                #region SHIELDSPEEDLEFTRIGHT
                if (InputManager.ActiveDevice.LeftStickLeft.WasPressed || InputManager.ActiveDevice.DPadLeft.WasPressed)
                {
                    if (m_ShieldRotateSlider.transform.GetChild(1).GetComponent<Slider>().value > 1)
                        m_ShieldRotateSlider.transform.GetChild(1).GetComponent<Slider>().value--;

                }
                else if (InputManager.ActiveDevice.LeftStickRight.WasPressed || InputManager.ActiveDevice.DPadRight.WasPressed)
                {
                    if (m_ShieldRotateSlider.transform.GetChild(1).GetComponent<Slider>().value < 5)
                        m_ShieldRotateSlider.transform.GetChild(1).GetComponent<Slider>().value++;
                }

                #endregion

                m_SelectedShieldRotateSpeed = (int)m_ShieldRotateSlider.transform.GetChild(1).GetComponent<Slider>().value;
                break;

            case MutatorButtons.ROUNDS:

                #region ROUNDSUPDOWN
                if (m_RoundSlider.transform.GetChild(0).GetComponent<Outline>().enabled == false)
                {
                    m_RoundSlider.transform.GetChild(0).GetComponent<Outline>().enabled = true;
                }

                if (InputManager.ActiveDevice.LeftStickDown.WasPressed && m_PlayersReady > 0 || InputManager.ActiveDevice.DPadDown.WasPressed && m_PlayersReady > 0)
                {
                    m_RoundSlider.transform.GetChild(0).GetComponent<Outline>().enabled = false;
                    m_MutatorButtons++;
                }
                if (InputManager.ActiveDevice.LeftStickUp.WasPressed || InputManager.ActiveDevice.DPadUp.WasPressed)
                {
                    m_RoundSlider.transform.GetChild(0).GetComponent<Outline>().enabled = false;
                    m_MutatorButtons--;
                }
                #endregion

                #region ROUNDSLEFTRIGHT
                if (InputManager.ActiveDevice.LeftStickLeft.WasPressed || InputManager.ActiveDevice.DPadLeft.WasPressed)
                {
                    if (m_RoundSlider.transform.GetChild(1).GetComponent<Slider>().value > 1)
                        m_RoundSlider.transform.GetChild(1).GetComponent<Slider>().value--;

                }
                else if (InputManager.ActiveDevice.LeftStickRight.WasPressed || InputManager.ActiveDevice.DPadRight.WasPressed)
                {
                    if (m_RoundSlider.transform.GetChild(1).GetComponent<Slider>().value < 10)
                        m_RoundSlider.transform.GetChild(1).GetComponent<Slider>().value++;
                }

                #endregion

                m_SelectedRounds = (int)m_RoundSlider.transform.GetChild(1).GetComponent<Slider>().value;

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
        int _val;
        if (m_PresetSuddenDeath.TryGetValue(Presets.NOSHIELDS, out _val))
        {
            m_SuddenDeath = _val;
        }
        SceneManager.LoadScene(m_LevelSelected.ToString(), LoadSceneMode.Single);
        m_MenuScene = false;
    }

    private void SetupPresetDictionary()
    {
        #region DEFAULT
        m_PresetDefault.Add(Presets.SHIELDHEALTH, 3);
        m_PresetDefault.Add(Presets.MOVESPEED, 2);
        m_PresetDefault.Add(Presets.FIREDELAY, 4);
        m_PresetDefault.Add(Presets.BULLETSPEED, 3);
        m_PresetDefault.Add(Presets.SHIELDROTSPEED, 2);
        m_PresetDefault.Add(Presets.NOSHIELDS, 0);

        #endregion

        #region SNIPER
        m_PresetSniper.Add(Presets.SHIELDHEALTH, 1);
        m_PresetSniper.Add(Presets.MOVESPEED, 1);
        m_PresetSniper.Add(Presets.FIREDELAY, 10);
        m_PresetSniper.Add(Presets.BULLETSPEED, 5);
        m_PresetSniper.Add(Presets.SHIELDROTSPEED, 1);
        m_PresetSniper.Add(Presets.NOSHIELDS, 0);
        #endregion

        #region TACTICAL
        m_PresetTactical.Add(Presets.SHIELDHEALTH, 1);
        m_PresetTactical.Add(Presets.MOVESPEED, 2);
        m_PresetTactical.Add(Presets.FIREDELAY, 10);
        m_PresetTactical.Add(Presets.BULLETSPEED, 3);
        m_PresetTactical.Add(Presets.SHIELDROTSPEED, 2);
        m_PresetTactical.Add(Presets.NOSHIELDS, 0);
        #endregion

        #region BULLET_HELL
        m_PresetBulletHell.Add(Presets.SHIELDHEALTH, 5);
        m_PresetBulletHell.Add(Presets.MOVESPEED, 2);
        m_PresetBulletHell.Add(Presets.FIREDELAY, 1);
        m_PresetBulletHell.Add(Presets.BULLETSPEED, 2);
        m_PresetBulletHell.Add(Presets.SHIELDROTSPEED, 2);
        m_PresetBulletHell.Add(Presets.NOSHIELDS, 0);
        #endregion

        #region SUDDEN_DEATH
        m_PresetSuddenDeath.Add(Presets.SHIELDHEALTH, 1);
        m_PresetSuddenDeath.Add(Presets.MOVESPEED, 2);
        m_PresetSuddenDeath.Add(Presets.FIREDELAY, 5);
        m_PresetSuddenDeath.Add(Presets.BULLETSPEED, 2);
        m_PresetSuddenDeath.Add(Presets.SHIELDROTSPEED, 2);
        m_PresetSuddenDeath.Add(Presets.NOSHIELDS, 0);

        #endregion
    }
}
