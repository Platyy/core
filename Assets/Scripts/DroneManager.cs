using UnityEngine;
using System.Collections;

public class DroneManager : MonoBehaviour {
    
    public PlayerController m_PlayerController;

    private Vector3 m_Forward, m_Left, m_Right;
    private RaycastHit m_HitInfo;

    void Start()
    {
        if(m_PlayerController == null)
        {
            Debug.Log("No player controller attatched to drone manager");
        }
    }

    void FixedUpdate()
    {
        GetDirections();

        CheckRays();
    }

    void GetDirections()
    {
        m_Forward = transform.forward;
        m_Left = -transform.right;
        m_Right = transform.right;
        Debug.DrawRay(transform.position, m_Forward, Color.blue);
        Debug.DrawRay(transform.position, m_Left, Color.green);
        Debug.DrawRay(transform.position, m_Right, Color.red);
    }

    void CheckRays()
    {
        Ray _forwardRay = new Ray(transform.position, m_Forward);
        Ray _leftRay    = new Ray(transform.position, m_Left);
        Ray _rightRay   = new Ray(transform.position, m_Right);

        if (Physics.Raycast(_forwardRay, out m_HitInfo, 4f))
        {
            if(m_HitInfo.collider.tag == "Environment" || m_HitInfo.collider.tag == "Player")
            {
                //Debug.Log("Colliding with: " + m_HitInfo.collider.tag);
                m_PlayerController.m_CanShoot = false;
            }
        }
        else if (Physics.Raycast(_leftRay, out m_HitInfo, 4f))
        {
            if (m_HitInfo.collider.tag == "Environment" || m_HitInfo.collider.tag == "Player")
            {
                //Debug.Log("Colliding with: " + m_HitInfo.collider.tag);
                m_PlayerController.m_CanShoot = false;
            }
        }
        else if (Physics.Raycast(_rightRay, out m_HitInfo, 4f))
        {
            if (m_HitInfo.collider.tag == "Environment" || m_HitInfo.collider.tag == "Player")
            {
                //Debug.Log("Colliding with: " + m_HitInfo.collider.tag);
                m_PlayerController.m_CanShoot = false;
            }
        }
        else if(m_PlayerController.m_CanShoot == false)
        {
            m_PlayerController.m_CanShoot = true;
        }
    }
}
