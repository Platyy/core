using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using InControl;

public class MainMenuManager : MonoBehaviour {

    private GameObject m_EventSystem;

    private enum SelectedButton
    {
        PLAYBUTTON,
        OPTIONSBUTTON,
        EXITBUTTON
    }

    private SelectedButton m_CurrentButton;

    void Start()
    {
        m_EventSystem = GameObject.Find("EventSystem");
        m_EventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(GameObject.Find("PlayButton"));
        m_CurrentButton = SelectedButton.PLAYBUTTON;
    }

    void Update()
    {
            switch (m_CurrentButton)
            {
                case SelectedButton.PLAYBUTTON:

                    if(InputManager.ActiveDevice.DPadDown.WasPressed)
                    {
                        m_EventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(GameObject.Find("OptionsButton"));
                        m_CurrentButton = SelectedButton.OPTIONSBUTTON;
                    }
                    else if(InputManager.ActiveDevice.DPadUp.WasPressed)
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

    public void ClickPlay()
    {
        SceneManager.LoadScene("SymmetricalLayout1", LoadSceneMode.Single);
    }

    public void ClickOptions()
    {


    }

    public void ClickQuit()
    {
        Application.Quit();
    }



}
