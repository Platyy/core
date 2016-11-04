using UnityEngine;
using System.Collections;
using InControl;


public class ShieldManager : MonoBehaviour {

    private PlayerController m_PC;
    public ParticleSystem m_HitParticle;

    private ShieldDestroy m_ShieldDestroy;

    public int m_HitsToTake;

    void Start()
    {
        m_PC = transform.GetComponentInParent<PlayerController>();
        m_ShieldDestroy = transform.GetComponentInParent<ShieldDestroy>();
        
    }


    void OnCollisionEnter(Collision _col)
    {
        if(_col.gameObject.tag == "Bullet")
        {
            ParticleSystem _s = (ParticleSystem)Instantiate(m_HitParticle, transform.position, transform.rotation);
            _s.startColor = m_PC.m_PlayerColor;
            m_PC.HitVibration();
            m_PC.m_CameraScript.Shake(0.35f, 0.3f);
            m_HitsToTake--;
            if(_col.gameObject != null)
            {
                Destroy(_col.gameObject);
            }
            if(m_HitsToTake == 0)
            {
                m_ShieldDestroy.m_Forward = transform.rotation;
                m_ShieldDestroy.m_PlayerColor = m_PC.m_PlayerColor;
                m_ShieldDestroy.m_Play = true;
                Destroy(gameObject);
            }
        }
    }
    
}
