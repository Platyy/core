using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

    Hashtable m_FirstHash;
    Hashtable m_SecondHash;

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
        m_FirstHash = new Hashtable();
        m_SecondHash = new Hashtable();
        
        m_FirstHash.Add("x", 6.0f);
        m_FirstHash.Add("y", 2.0f);
        m_FirstHash.Add("z", 4f);
        m_FirstHash.Add("time", 0.05f);
        m_FirstHash.Add("looptype", iTween.LoopType.none);
        m_FirstHash.Add("easetype", iTween.EaseType.easeInQuint);

        m_SecondHash.Add("x", 1f);
        m_SecondHash.Add("y", 1f);
        m_SecondHash.Add("z", 6f);
        m_SecondHash.Add("delay", 0.05f);
        m_SecondHash.Add("time", 0.25f);
        m_SecondHash.Add("looptype", iTween.LoopType.none);
        m_SecondHash.Add("easetype", iTween.EaseType.easeOutQuint);


        m_LMS = FindObjectOfType<LMS>();
        m_PlayerControllerManager = FindObjectOfType<PlayerControllerManager>();

    }

    void Start()
    {
        m_Light = GetComponentInChildren<Light>();
        m_Light.color = m_LMS.m_PlayerColors[m_ID];

        m_Materials = gameObject.GetComponentInChildren<MeshRenderer>().materials;
        for (int i = 0; i < m_Materials.Length; i++)
        {
            m_Materials[i].SetColor("_Color", m_LMS.m_PlayerColors[m_ID]);
        }

        gameObject.GetComponentInChildren<MeshRenderer>().materials = m_Materials;

        m_Materials = gameObject.GetComponentInChildren<TrailRenderer>().materials;
        for (int i = 0; i < m_Materials.Length; i++)
        {
            m_Materials[i].SetColor("_Color", m_LMS.m_PlayerColors[m_ID]);
        }
        gameObject.GetComponentInChildren<TrailRenderer>().materials = m_Materials;

        iTween.ScaleTo(transform.GetChild(0).gameObject, m_FirstHash);
        iTween.ScaleTo(transform.GetChild(0).gameObject, m_SecondHash);
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
