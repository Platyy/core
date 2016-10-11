using UnityEngine;
using System.Collections;

public class CoreManager : MonoBehaviour {

    public GameObject m_Player;

    void OnCollisionEnter(Collision _col)
    {
        if(_col.gameObject.tag == "Bullet")
        {
            FindObjectOfType<LMS>().PlayDeath(gameObject);
            Destroy(m_Player);
        }
    }
}
