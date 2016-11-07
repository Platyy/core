using UnityEngine;
using System.Collections;

public class ShieldDestroy : MonoBehaviour {

    public ParticleSystem m_ShieldDestroy;

    [ColorUsage(true, true, 2.0f, 2.0f, .5f, 3.0f)] public Color m_PlayerColor;

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
        _go.GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(m_PlayerColor.r * 2, m_PlayerColor.g * 2, m_PlayerColor.b * 2));
        _go.Play();
        m_Play = false;
        Destroy(_go, 5f);
    }

}
