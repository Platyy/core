using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
	
	// Public variables
	//public GameObject target;
	private Vector3 originalPos;

	// Camera shake variables
	public static CameraScript instance;
	public float _amplitude = 0.1f;
	private Vector3 initialPos;
	public bool isShaking = false;

	// Use this for initialization
	void Start () {

		originalPos = transform.position;
		instance = this;
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (originalPos.x, originalPos.y, originalPos.z);

		initialPos = new Vector3 (originalPos.x, originalPos.y, originalPos.z);

		if (isShaking) {
			transform.localPosition = initialPos + Random.insideUnitSphere * _amplitude;
		}
	}

	public void Shake (float amplitude, float duration) {
		_amplitude = amplitude;
		isShaking = true;
		Invoke ("StopShaking", duration);
	}

	public void StopShaking () {
		isShaking = false;
	}
}
