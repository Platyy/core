using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
	
	//public GameObject target;
	private Vector3 m_OriginalPos;


	public float m_Amplitude = 0.1f;

	private Vector3 initialPos;

	public bool isShaking = false;
    
	void Start ()
    {
        m_OriginalPos = transform.position;
	}
	
	void Update ()
    {
		if (isShaking) {
			transform.localPosition = m_OriginalPos + Random.insideUnitSphere * m_Amplitude;
		}
	}

	public void Shake (float _amplitude, float _duration)
    {
        m_Amplitude = _amplitude;
		isShaking = true;
		Invoke ("StopShaking", _duration);
	}

	public void StopShaking ()
    {
		isShaking = false;
	}
}
