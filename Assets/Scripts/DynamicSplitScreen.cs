using UnityEngine;
using System.Collections;

public class DynamicSplitScreen : MonoBehaviour {

    public Camera m_Camera;

    public GameObject m_Player;

    void Start()
    {

    }

    void Update()
    {
        CheckPos();
    }

    void CheckPos()
    {
        Plane[] _planes = GeometryUtility.CalculateFrustumPlanes(m_Camera);

        if (!GeometryUtility.TestPlanesAABB(_planes, m_Player.GetComponentInChildren<Collider>().bounds))
        {
            FillRight();
        }
        else
            transform.position = new Vector3(0, 0, 0);
    }

    void FillRight()
    {

        float _position = m_Camera.nearClipPlane + 0.01f;
        transform.position = (m_Camera.transform.position + m_Camera.transform.forward * _position);

        float _height = Mathf.Tan(m_Camera.fieldOfView * Mathf.Deg2Rad * 0.5f) * _position * 2.0f;
        transform.position = new Vector3(_height * m_Camera.aspect / 4, transform.position.y, transform.position.z);

        transform.localScale = new Vector3(_height * m_Camera.aspect / 2, _height, 0);

    }

}
