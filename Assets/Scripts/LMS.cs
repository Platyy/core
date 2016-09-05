using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using InControl;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public struct InputDevices
{
    public InputDevice m_Player1;
    public InputDevice m_Player2;
    public InputDevice m_Player3;
    public InputDevice m_Player4;
}

public class LMS : MonoBehaviour {

    public int m_Rounds = 3;
    private int m_RoundsRemaining = 3;
    public float m_RoundTime = 60f;
    private float m_RemainingTime = 60f;

    float m_Mins;
    float m_Secs;

    public Color[] m_PlayerColors = new Color[4] { Color.blue, Color.red, Color.magenta, Color.green };


    public int m_ScorePerKill = 3;
    public int m_ScorePerRoundWin = 1;

    public bool m_RoundStarted = false;

    public Text m_TimeDisplay;
    public GameObject m_Scores;

    public int[] m_PlayerScores = new int[] { 0, 0, 0, 0 };

    public int[] m_PlayersAlive;
    public int[] m_PlayerKillsThisRound = new int[] { 0, 0, 0, 0 };
    public GameObject m_Player;
    public Transform[] m_Spawns = new Transform[4];

    private GameObject[] m_PlayerList;


    public InputDevice[] m_InputDevices = new InputDevice[4];
    public List<GameObject> m_InstantiatedPlayers;

    private bool m_P1Ready = false, m_P2Ready = false, m_P3Ready = false, m_P4Ready = false;

    private PlayerControllerManager m_ControllerManager;
    private int m_DevicesAssigned = -1;

    private int m_CurrentDead = 0;

    public void Start()
    {
        m_ControllerManager = FindObjectOfType<PlayerControllerManager>();
        m_DevicesAssigned = m_ControllerManager.m_DevicesAssigned + 1;

        m_PlayersAlive = new int[m_DevicesAssigned];
        m_PlayerList = new GameObject[m_DevicesAssigned];

        m_Scores.SetActive(false);
        m_RemainingTime = m_RoundTime;
        for (int i = 0; i < m_PlayersAlive.Length; i++)
        {
            m_PlayersAlive[i] = 1;
        }

        ResetPlayers();
    }

    public void Update()
    {
        if (Input.GetButtonDown("StartButton"))
        {
            m_RoundStarted = true;
        }

        ManageTime();
    }

    public void ManagePlayers()
    {
        for (int i = 0; i < m_DevicesAssigned; i++)
        {
            if(m_PlayersAlive[i] == 0)
            {
                m_CurrentDead++;
            }
        }
        if (m_CurrentDead >= m_DevicesAssigned - 1)
        {
            //EndRound();
            EndGame();
        }
        else m_CurrentDead = 0;
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
            EndGame();
            Debug.Log(m_RoundsRemaining);
        }
        else
        {
            EndGame();
        }
    }

    void EndGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
            m_InstantiatedPlayers.Add(_go);
            m_PlayerList[i] = _go;
            var _pc = _go.GetComponent<PlayerController>();
            _pc.m_PlayerID = i;

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

    void SpawnPlayers()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject _go = (GameObject)Instantiate(m_Player, m_Spawns[i].position, m_Spawns[i].rotation);
            m_InstantiatedPlayers.Add(_go);
            m_PlayerList[i] = _go;
            var _pc = _go.GetComponent<PlayerController>();
            _pc.m_PlayerID = i;

            _pc.m_Renderer = _go.GetComponentsInChildren<Renderer>();
            for (int j = 0; j < _pc.m_Renderer.Length; j++)
            {
                _pc.m_Renderer[j].material.shader = Shader.Find("Standard");
                _pc.m_Renderer[j].material.SetColor("_EmissionColor", m_PlayerColors[i]);
            }
            m_PlayersAlive[i] = 1;

        }
    }
    
}
