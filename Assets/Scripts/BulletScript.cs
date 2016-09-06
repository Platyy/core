using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

	//public ParticleSystem m_BulletParticle;
    public LMS m_LMS;
    private PlayerControllerManager m_PlayerControllerManager;
    public int m_ID = -1;
    public int m_HitID = 0;
    private MeshRenderer m_Renderer;
    private Material[] m_Materials;
    private TrailRenderer m_Trail;
    private Light m_Light;

    void Awake()
    {
        m_LMS = FindObjectOfType<LMS>();
        m_PlayerControllerManager = FindObjectOfType<PlayerControllerManager>();

    }

    void Start()
    {
        m_Light = GetComponent<Light>();
        m_Light.color = m_LMS.m_PlayerColors[m_ID];

        m_Materials = gameObject.GetComponent<MeshRenderer>().materials;
        for (int i = 0; i < m_Materials.Length; i++)
        {
            m_Materials[i].SetColor("_Color", m_LMS.m_PlayerColors[m_ID]);
        }

        gameObject.GetComponent<MeshRenderer>().materials = m_Materials;

        m_Materials = gameObject.GetComponent<TrailRenderer>().materials;
        for (int i = 0; i < m_Materials.Length; i++)
        {
            m_Materials[i].SetColor("_Color", m_LMS.m_PlayerColors[m_ID]);
        }
        gameObject.GetComponent<TrailRenderer>().materials = m_Materials;
    }

	void OnCollisionEnter (Collision other)
    {
        if(other.gameObject.tag == "Core")
        {
            var _pc = other.gameObject.GetComponentInParent<PlayerController>();
            m_HitID = _pc.m_PlayerID;
            _pc.m_CameraScript.Shake(0.9f, 0.75f);

            for (int i = 0; i < 4; i++)
            {
                if (m_HitID == i)
                {
                    m_LMS.m_PlayersAlive[i] = 0;
                    m_LMS.ManagePlayers();
                    break;
                }
            }
            m_LMS.m_PlayerKillsThisRound[m_ID]++;
            m_LMS.m_PlayerScores[m_ID] += m_LMS.m_ScorePerKill;
        }
		Destroy (gameObject);
	}
}
