using UnityEngine;
using System.Collections;

public class ArmourPieces : MonoBehaviour {

	private int m_Health = 1;
    
	void Update ()
    {
		if (m_Health <= 0) {
			Destroy (gameObject);
		}
	
	}

	void OnCollisionEnter (Collision other)
    {
		if (other.gameObject.tag == "Bullet") {
            m_Health -= 1;
		}
	}
}
