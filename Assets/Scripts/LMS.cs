using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using InControl;
using System.Collections;
using System.Collections.Generic;
using System;

public class LMS : MonoBehaviour {

    public int m_Rounds = 3;
    private int m_RoundsRemaining = 3;
    public float m_RoundTime = 60f;
    private float m_RemainingTime = 60f;
    public int[] m_PlayerScores = new int[4];

    public GameObject m_ScoreObject;
    private Text[] m_PScores = new Text[4];
    private ScoreCounter m_ScoreCounter;

    float m_Mins;
    float m_Secs;

    public ParticleSystem m_ExplosionParticle;
    public ParticleSystem m_HitParticle;

    public Color[] m_PlayerColors = new Color[4] { Color.cyan, new Color(1.0f, 0.5f, 0), Color.magenta, Color.green };

    private InputDevice[] m_InputDevicesUsed = new InputDevice[4];

    public int m_ScorePerKill = 3;
    public int m_ScorePerRoundWin = 1;

    public bool m_RoundStarted = false;

    public Text m_TimeDisplay;
    public GameObject m_Scores;

    public int[] m_PlayersAlive;
    public int[] m_PlayerKillsThisRound = new int[] { 0, 0, 0, 0 };
    public GameObject m_Player;

    public GameObject m_QuitCanvas;

    public Transform[] m_Spawns = new Transform[4];

    private GameObject[] m_PlayerList;

    private List<InputDevice> m_UsedDevices;

    public InputDevice[] m_InputDevices = new InputDevice[4];
    public List<GameObject> m_InstantiatedPlayers;

    private PlayerControllerManager m_ControllerManager;
    private GameObject m_EventSystem;
    private int m_DevicesAssigned = -1;

    private int m_CurrentDead = 0;

    private bool m_NewGame = true;

    private MainMenuManager m_MenuManager;

    private enum SelectedButton
    {
        OKBUTTON,
        CANCELBUTTON
    }

    private SelectedButton m_Button;

    void Awake()
    {
        m_MenuManager = FindObjectOfType<MainMenuManager>();
        if(m_MenuManager == null)
        {
            Debug.LogError("Couldn't find a menumanager");
        }
    }

    public void Start()
    {
        if(m_ScoreCounter == null)
        {
            m_ScoreCounter = FindObjectOfType<ScoreCounter>();
        }

        DontDestroyOnLoad(m_ScoreCounter);

        m_ControllerManager = FindObjectOfType<PlayerControllerManager>();
        m_DevicesAssigned = m_MenuManager.m_PlayersReady;
        m_EventSystem = GameObject.Find("EventSystem");

        m_InputDevicesUsed = m_MenuManager.m_MenuDevices;

        m_PlayersAlive = new int[m_MenuManager.m_PlayersReady];
        m_PlayerList = new GameObject[m_MenuManager.m_PlayersReady];

        m_Scores.SetActive(false);
        m_RemainingTime = m_MenuManager.m_SelectedGameLength * 60;
        for (int i = 0; i < m_PlayersAlive.Length; i++)
        {
            m_PlayersAlive[i] = 1;
        }


        m_QuitCanvas.SetActive(false);
        Spawn();

        for (int i = 0; i < 4; i++)
        {
            m_PScores[i] = m_ScoreObject.transform.GetChild(i).gameObject.transform.GetChild(0).GetComponent<Text>();
            m_PScores[i].text = m_ScoreCounter.m_PlayerScores[i].ToString();
        }
    }

    public void Update()
    {
        //Debug.Log(m_PlayersAlive[0] + " " + m_PlayersAlive[1]);

        if (Input.GetButtonDown("StartButton") && m_NewGame)
        {
            m_RoundStarted = true;

            m_NewGame = false;
            GetControllers();
        }

        if (Input.GetButtonDown("StartButton"))
        {
            m_RoundStarted = true;
            GetControllers();
        }
        ManageTime();
        ManageQuitCanvas();
    }

    public void GetControllers()
    {
        PlayerController[] _controller = FindObjectsOfType<PlayerController>();
        for (int i = 0; i < _controller.Length; i++)
        {
            m_InputDevices[i] = _controller[i].m_Device;
        }
    }

    IEnumerator DeathVibration(InputDevice _device)
    {
        _device.Vibrate(1f);
        yield return new WaitForSeconds(0.75f);
        _device.StopVibration();
    }

    public void ManagePlayers()
    {
        for (int i = 0; i < m_DevicesAssigned; i++)
        {
            StartCoroutine(DeathVibration(m_InputDevices[i]));
            if(m_PlayersAlive[i] == 0)
            {
                m_CurrentDead++;
            }
        }
        if (m_CurrentDead >= m_DevicesAssigned - 1)
        {
            StartCoroutine(FinalKill());
            //EndGame();
        }
        else m_CurrentDead = 0;
    }

    IEnumerator FinalKill()
    {
        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(1);
        Time.timeScale = Mathf.Lerp(0.5f, 1.0f, 1.0f);
        yield return new WaitForSeconds(1);
        LastAliveScore();
        EndRound();
    }

    void LastAliveScore()
    {
        int _count = 0;
        int _player = -1;
        for (int i = 0; i < m_PlayersAlive.Length; i++)
        {
            if(m_PlayersAlive[i] == 1)
            {
                _count++;
                _player = i;
                break;
            }
        }
        if(_count == 1)
        {
            m_ScoreCounter.m_PlayerScores[_player] += 1;
        }
    }

    void ManageTime()
    {
        if(m_RoundStarted)
        {
            m_RemainingTime -= Time.deltaTime;
            m_Mins = Mathf.Floor(m_RemainingTime / 60f);
            m_Secs = Mathf.Floor(m_RemainingTime % 60);
            m_TimeDisplay.text = m_Mins.ToString() + ":" + m_Secs.ToString();
            
        }

        if(m_RemainingTime <= 0)
        {
            EndRound();
        }
    }

    void EndRound()
    {
        m_RoundStarted = false;
        m_RoundsRemaining--;
        m_RemainingTime = m_RoundTime;
        if (m_RoundsRemaining > 0)
        {
            //ResetPlayers();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
            //EndGame();
            Debug.Log(m_RoundsRemaining);
        }
        else
        {
            EndGame();
        }
    }

    void EndGame()
    {
        m_Scores.SetActive(true);
        m_RoundsRemaining = 0;
        for (int i = 0; i < m_Scores.transform.childCount; i++)
        {
            if(i <= m_DevicesAssigned)
            {
                m_Scores.transform.GetChild(i).GetComponent<Text>().text = "Player " + i.ToString() + ": "+ CalculateScore(i).ToString();
            }
            else
            {
                m_Scores.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        
        if (Input.GetButtonDown("BackButton"))
        {
            SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
        }
    }

    int CalculateScore(int _playerID)
    {
        int _score;

        _score = m_PlayerScores[_playerID];

        return _score;
    }

    void ResetPlayers()
    {
        for (int i = 0; i < m_DevicesAssigned; i++)
        {
            if (m_PlayerList[i] != null)
            {
                m_PlayerScores[i] += m_ScorePerRoundWin;
                Destroy(m_PlayerList[i]);
            }
        }

        for (int i = 0; i < m_DevicesAssigned; i++)
        {
            GameObject _go = (GameObject)Instantiate(m_Player, m_Spawns[i].position, m_Spawns[i].rotation);
            var _pc = _go.GetComponent<PlayerController>();
            for (int j = 0; j < _go.transform.GetChild(0).transform.childCount; j++)
            {
                _go.transform.GetChild(0).transform.GetChild(j).GetComponent<ShieldManager>().m_HitsToTake = m_MenuManager.m_SelectedShieldHealth;
            }
            _pc.fireDelay = m_MenuManager.m_SelectedFireRate * 0.1f;
            _pc.moveSpeed = _pc.moveSpeed + (m_MenuManager.m_SelectedMoveSpeed * 50);
            _pc.m_BulletSpeed = _pc.m_BulletSpeed * m_MenuManager.m_SelectedBulletSpeed;
            _pc.m_ShieldRotationSpeed = _pc.m_ShieldRotationSpeed * m_MenuManager.m_SelectedShieldRotateSpeed;

            m_InstantiatedPlayers.Add(_go);
            m_PlayerList[i] = _go;
            _pc.m_PlayerID = i;

            _pc.m_Device = m_InputDevicesUsed[i];

            _pc.m_Renderer = _go.GetComponentsInChildren<Renderer>();
            for (int j = 0; j < _pc.m_Renderer.Length; j++)
            {
                _pc.m_Renderer[j].material.shader = Shader.Find("Standard");
                _pc.m_Renderer[j].material.SetColor("_EmissionColor", m_PlayerColors[i]);
                _pc.m_PlayerColor = m_PlayerColors[i];
            }
            m_PlayersAlive[i] = 1;
            _go.SetActive(true);
        }
    }

    public void Spawn()
    {
        for (int i = 0; i < m_DevicesAssigned; i++)
        {
            GameObject _go = (GameObject)Instantiate(m_Player, m_Spawns[i].position, m_Spawns[i].rotation);
            m_InstantiatedPlayers.Add(_go);
            m_PlayerList[i] = _go;
            var _pc = _go.GetComponent<PlayerController>();
            for (int j = 0; j < _go.transform.GetChild(0).transform.childCount; j++)
            {
                _go.transform.GetChild(0).transform.GetChild(j).GetComponent<ShieldManager>().m_HitsToTake = m_MenuManager.m_SelectedShieldHealth;
            }
            _pc.fireDelay = m_MenuManager.m_SelectedFireRate * 0.1f;
            _pc.moveSpeed = _pc.moveSpeed + (m_MenuManager.m_SelectedMoveSpeed * 50);
            _pc.m_BulletSpeed = _pc.m_BulletSpeed * m_MenuManager.m_SelectedBulletSpeed;
            _pc.m_ShieldRotationSpeed = _pc.m_ShieldRotationSpeed * m_MenuManager.m_SelectedShieldRotateSpeed;
            _pc.m_PlayerID = i;

            _pc.m_Renderer = _go.GetComponentsInChildren<Renderer>();
            for (int j = 0; j < _pc.m_Renderer.Length; j++)
            {
                _pc.m_Renderer[j].material.shader = Shader.Find("Standard");
                _pc.m_Renderer[j].material.SetColor("_EmissionColor", m_PlayerColors[i]);
                _pc.m_PlayerColor = m_PlayerColors[i];
            }
            m_PlayersAlive[i] = 1;
        }
    }

    void ManageQuitCanvas()
    {
        if(Input.GetButtonDown("BackButton"))
        {
            m_QuitCanvas.SetActive(true);
            m_Button = SelectedButton.OKBUTTON;
            m_EventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(GameObject.Find("OK"));
        }

        if(m_QuitCanvas.activeSelf && m_QuitCanvas != null)
        {
            switch (m_Button)
            {
                case SelectedButton.OKBUTTON:
                    if(InputManager.ActiveDevice.Action1.WasPressed)
                    {
                        Destroy(m_MenuManager.gameObject);
                        Destroy(m_ScoreCounter.gameObject);
                        SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
                    }
                    break;

                case SelectedButton.CANCELBUTTON:
                    if (InputManager.ActiveDevice.Action1.WasPressed)
                    {
                        m_QuitCanvas.SetActive(false);
                    }
                    break;
            }

            if(m_Button == SelectedButton.OKBUTTON && InputManager.ActiveDevice.DPadRight.WasPressed)
            {
                m_Button = SelectedButton.CANCELBUTTON;
                m_EventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(GameObject.Find("Cancel"));
            }
            else if(m_Button == SelectedButton.CANCELBUTTON && InputManager.ActiveDevice.DPadLeft.WasPressed)
            {
                m_Button = SelectedButton.OKBUTTON;
                m_EventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(GameObject.Find("OK"));
            }

        }
    }

    public void PlayHitParticle(Vector3 _position, Color _color)
    {
        var _go = (ParticleSystem)Instantiate(m_HitParticle, _position, Quaternion.identity);
        _go.GetComponent<Renderer>().material.SetColor("_EmissionColor", _color);
        Destroy(_go, 2f);
    }

    public void PlayDeath(GameObject _player, Color _color)
    {
        var _go = (ParticleSystem)Instantiate(m_ExplosionParticle, _player.transform.position, Quaternion.identity);
        _go.GetComponent<Renderer>().material.SetColor("_EmissionColor", _color);
        _go.Play();
        Destroy(_go, 2f);
    }

}
