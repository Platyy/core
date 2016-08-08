using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

	public ParticleSystem m_BulletParticle;

	void OnCollisionEnter (Collision other)
    {
		Instantiate (m_BulletParticle, transform.position, transform.rotation);
		Destroy (gameObject);
	}
}
