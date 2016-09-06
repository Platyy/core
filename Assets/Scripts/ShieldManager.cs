using UnityEngine;
using System.Collections;
using InControl;


public class ShieldManager : MonoBehaviour {

    private PlayerController m_PC;

    void Start()
    {
        m_PC = transform.GetComponentInParent<PlayerController>();
    }


    void OnCollisionEnter(Collision _col)
    {
        if(_col.gameObject.tag == "Bullet")
        {
            m_PC.HitVibration();
            m_PC.m_CameraScript.Shake(0.3f, 0.3f);
            Destroy(gameObject);
        }
    }
    
}
