using UnityEngine;
using System.Collections;
using InControl;


public class ShieldManager : MonoBehaviour {

    private PlayerController m_PC;
    public ParticleSystem m_HitParticle;

    public int m_HitsToTake = 3;

    void Start()
    {
        m_PC = transform.GetComponentInParent<PlayerController>();
        m_HitParticle.startColor = m_PC.m_PlayerColor;
    }


    void OnCollisionEnter(Collision _col)
    {
        if(_col.gameObject.tag == "Bullet")
        {
            m_HitParticle.Play();
            m_PC.HitVibration();
            m_PC.m_CameraScript.Shake(0.35f, 0.3f);
            m_HitsToTake--;
            if(m_HitsToTake == 0)
            {
                Destroy(gameObject);
            }
        }
    }
    
}
