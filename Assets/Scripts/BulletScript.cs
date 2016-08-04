using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

	public ParticleSystem bulletPS;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter (Collision other) {
		Instantiate (bulletPS, transform.position, transform.rotation);
		Destroy (gameObject);
	}
}
