using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	// Public variables
	// Movement
	public float moveSpeed; // Acceleration speed
	public float maxSpeed; // Max movement speed
	public float tankRotSpeed = 0.02f; // Speed of rotation
	public float droneSpeed = 0.04f;
	// Private variable
	private float deadzone = 0.25f; // Analog stick deadzone
	private Rigidbody rb; // Player's rigidbody component
	private Vector3 movementInput; // Input for movement
	private Vector3 rotationInput; // Input for rotation
	private Vector3 rotation;
	private Vector3 tankRotation; // Current rotation
	private Vector3 turretRotation; // Current rotation
	private Vector3 spawn; // Player's starting position
	private Quaternion targetRotation;

	private float leftTrigger;
	private float rightTrigger;
	public GameObject turret;

	public int playerNumber = 1;
	private string movementXName;
	private string movementYName;
	private string rotationXName;
	private string rotationYName;
	private string rightTriggerName;
	private string leftTriggerName;
	private string rightBumperName;
	private string leftBumperName;

	private float timer;
	public float fireDelay;
	public float randomSpreadX;
	public Transform bulletSpawn;
	public float projectileSpeed;
	public Rigidbody projectile;

	public AudioSource shootAudio;
	public AudioSource exploAudio;
	public ParticleSystem exploPS;

	public float exploForce = 100f;
	public float exploRadius = 5f;
	public float upwardsMod = 0.2f;

    private CameraScript m_CameraScript;

	void Start ()
    {
        m_CameraScript = FindObjectOfType<CameraScript>();
        if(m_CameraScript == null)
        {
            Debug.LogError("Camera Script not found!");
        }

        rb = GetComponent<Rigidbody> (); // Setting rb as player's rigidbody component

		movementXName = "LSX" + playerNumber;
		movementYName = "LSY" + playerNumber;
		rotationXName = "RSX" + playerNumber;
		rotationYName = "RSY" + playerNumber;
		rightTriggerName = "RT" + playerNumber;
		leftTriggerName = "LT" + playerNumber;
		rightBumperName = "RB" + playerNumber;
		leftBumperName = "LB" + playerNumber;

	}

	void Update ()
    {

		timer += Time.deltaTime;
        
	}

	void FixedUpdate ()
    {

		Movement ();

		Shooting ();

	}

	void Movement ()
    {

		movementInput = new Vector3 (Input.GetAxis (movementXName), 0, Input.GetAxis (movementYName));
		rotationInput = new Vector3 (Input.GetAxis (rotationXName), 0, Input.GetAxis (rotationYName));

		if (movementInput.magnitude > 1) {
			movementInput.Normalize ();
		}
		if (rotationInput.magnitude < deadzone) {
			rotationInput = Vector3.zero;
		}
		// Rigidbody Movement
		if (movementInput.magnitude < deadzone) {
			movementInput = Vector3.zero;
		}
		if (rb.velocity.magnitude < maxSpeed) {
			rb.AddForce (movementInput * moveSpeed);
		}

		if (Input.GetButton (leftBumperName)) {
			transform.Rotate (0, -5, 0);
		}

		if (Input.GetButton (rightBumperName)) {
			transform.Rotate (0, 5, 0);
		}

		if (rotationInput.x == 0f && rotationInput.z == 0f) { //Checks if player is using right stick
		} else {
			if (rotationInput.magnitude > deadzone) {
				turretRotation = turret.transform.rotation.eulerAngles;
				turret.transform.rotation = Quaternion.LookRotation (rotationInput);
				turret.transform.rotation = Quaternion.Lerp (Quaternion.Euler (turretRotation), turret.transform.rotation, droneSpeed);
			}
		}

	}

	void Shooting ()
    {
		leftTrigger = Input.GetAxis (leftTriggerName);
		rightTrigger = Input.GetAxis (rightTriggerName);

		if (rightTrigger > 0.1 && timer > fireDelay) {
			timer = 0f;
			shootAudio.Play ();
			Rigidbody proj = Instantiate (projectile, bulletSpawn.position, projectile.transform.rotation) as Rigidbody;
			proj.velocity = bulletSpawn.TransformDirection (new Vector3 (0, 0, projectileSpeed));
            Destroy(proj.gameObject, 3);
		}
	}

	void OnCollisionEnter (Collision other)
    {
		if (other.gameObject.tag == "Bullet") {
			Vector3 explosionPos = transform.position;
			Collider[] cols = Physics.OverlapSphere (explosionPos, exploRadius);
			foreach (Collider hit in cols) {
				Rigidbody colRb = hit.GetComponent<Rigidbody> ();
				if (colRb != null) {
					colRb.AddExplosionForce (exploForce, explosionPos, exploRadius, upwardsMod, ForceMode.Impulse);
				}
			}
			m_CameraScript.Shake (1f, 0.2f);
			Instantiate (exploPS, transform.position, transform.rotation);
			Instantiate (exploAudio, transform.position, transform.rotation);
			Destroy (gameObject);
		}
	}
}