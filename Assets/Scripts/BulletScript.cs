using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

	public ParticleSystem m_BulletParticle;

	void OnCollisionEnter (Collision other)
    {
<<<<<<< HEAD
		Instantiate (m_BulletParticle, transform.position, transform.rotation);
=======
>>>>>>> + InControl
		Destroy (gameObject);
		Instantiate (bulletPS, transform.position, transform.rotation);
	}
}
