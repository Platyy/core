using UnityEngine;
using System.Collections;

public class ShieldDestroy : MonoBehaviour {

    public ParticleSystem m_ShieldDestroy;
    public Color m_PlayerColor;
    public bool m_Play = false;
    public Quaternion m_Forward;

    void Update()
    {
        if(m_Play)
        {
            PlayDeath();
        }
    }

    public void PlayDeath()
    {
        m_Forward.eulerAngles = new Vector3(m_Forward.eulerAngles.x, m_Forward.eulerAngles.y, m_Forward.eulerAngles.z);
        var _go = (ParticleSystem)Instantiate(m_ShieldDestroy, new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z), m_Forward);
        //_ps.startColor = m_PlayerColor;
        _go.Play();
        m_Play = false;
        Destroy(_go, 5f);
    }

}
