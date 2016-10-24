using UnityEngine;
using System.Collections;

public class RotateUI : MonoBehaviour {

    public float m_RotateSpeed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        transform.Rotate(0, 0, m_RotateSpeed);

	}
}
