using UnityEngine;
using System.Collections;

public class RotatingObject : MonoBehaviour {

    public float m_RotationSpeed = 5f;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        transform.Rotate(Vector3.up, m_RotationSpeed * Time.deltaTime);
	}
}
