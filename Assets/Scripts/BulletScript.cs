using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

	//public ParticleSystem m_BulletParticle;
    public LMS m_LMS;
    public int m_ID = -1;
    public int m_HitID = 0;

    void Awake()
    {
        m_LMS = FindObjectOfType<LMS>();
    }

    void Start()
    {
    }

	void OnCollisionEnter (Collision other)
    {
        if(other.gameObject.tag == "Core")
        {
            m_HitID = other.gameObject.GetComponentInParent<PlayerController>().m_PlayerID;

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
