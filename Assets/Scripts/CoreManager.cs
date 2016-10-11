using UnityEngine;
using System.Collections;

public class CoreManager : MonoBehaviour {

    public GameObject m_Player;
    private LMS m_LMS;
    public int m_ID = -1;

    void Start()
    {
        m_LMS = FindObjectOfType<LMS>();
    }

    void OnCollisionEnter(Collision _col)
    {
        if(_col.gameObject.tag == "Bullet")
        {
            FindObjectOfType<LMS>().PlayDeath(gameObject, m_LMS.m_PlayerColors[m_ID]);
            Destroy(m_Player);
        }
    }
}
