using UnityEngine;
using System.Collections;

public class CameraOrbit : MonoBehaviour {

    public Transform m_Center;

    public float m_RotationSpeed = 50f;

    void Update()
    {
        transform.RotateAround(m_Center.position, Vector3.up, m_RotationSpeed * Time.deltaTime);
    }

}
