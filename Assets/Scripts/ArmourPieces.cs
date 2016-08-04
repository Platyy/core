using UnityEngine;
using System.Collections;

public class ArmourPieces : MonoBehaviour {

	private int health;

	// Use this for initialization
	void Start () {

		health = 1;
	
	}
	
	// Update is called once per frame
	void Update () {

		if (GetComponent<FixedJoint> ().connectedBody == null || GetComponent<FixedJoint>() == null) {
			GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.None;
		}

		if (health <= 0) {
			Destroy (gameObject);
		}
	
	}

	void OnCollisionEnter (Collision other) {
//		if (other.gameObject.tag == "Bullet") {
//			Destroy (gameObject);
//		}

		if (other.gameObject.tag == "Bullet") {
			health -= 1;
		}
	}
}
