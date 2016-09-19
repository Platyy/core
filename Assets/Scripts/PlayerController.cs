using UnityEngine;
using InControl;
using System.Collections;
using System.Collections.Generic;


public class PlayerController : MonoBehaviour {

    public GameObject m_Drone, m_Core, m_Shields, m_Bullet;
    public Color m_PlayerColor;
    public Transform m_BulletSpawn;
    public float m_BulletSpeed = 10f;
    public bool m_CanShoot = true;
    public int m_PlayerID = 0;

    private bool m_Angled = false;

    public int m_ShootingAngles = 16;

    public Renderer[] m_Renderer;
    
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
    private Quaternion targetRotation;

    private float leftTrigger;
    private float rightTrigger;
    
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
    public float projectileSpeed;
    
    public float exploForce = 100f;
    public float exploRadius = 5f;
    public float upwardsMod = 0.2f;

    public CameraScript m_CameraScript;

    private LMS m_LMS;

    private PlayerControllerManager m_ControllerManager;
    
    
    public InputDevice m_Device;
    private InputManager m_InputManager;
    
    public bool m_HasControls = false;

    private LineRenderer m_LineRenderer;

    public Material m_LineMaterial;

    public AudioSource m_AS;

    private float m_Major, m_Minor;

    public ParticleSystem m_ShotParticle;

    void Awake()
    {
        m_ControllerManager = FindObjectOfType<PlayerControllerManager>();
        m_Device = m_ControllerManager.GiveInput(gameObject);
    }

    void Start()
    {
        m_CameraScript = Camera.main.GetComponent<CameraScript>();
        m_LMS = FindObjectOfType<LMS>();
        rb = GetComponent<Rigidbody>(); // Setting rb as player's rigidbody component

        m_LineRenderer = gameObject.AddComponent<LineRenderer>();
        m_LineRenderer.material = m_LineMaterial;
        
        m_LineRenderer.SetWidth(0.3f, 0.1f);
        m_LineRenderer.SetVertexCount(2);
        m_LineRenderer.useWorldSpace = true;

        movementXName = "LSX" + m_PlayerID;
        movementYName = "LSY" + m_PlayerID;
        rotationXName = "RSX" + m_PlayerID;
        rotationYName = "RSY" + m_PlayerID;
        rightTriggerName = "RT" + m_PlayerID;
        leftTriggerName = "LT" + m_PlayerID;
        rightBumperName = "RB" + m_PlayerID;
        leftBumperName = "LB" + m_PlayerID;

        m_ShotParticle.startColor = m_PlayerColor;

        if (m_Device != null)
        {
            m_HasControls = true;
        }
        else
        {
            m_Device = m_ControllerManager.GiveInput(gameObject);
            m_HasControls = true;
        }
        m_LineRenderer.SetColors(m_PlayerColor, m_PlayerColor);

        m_Major = 360f / m_ShootingAngles;
        m_Minor = m_Major / 2;
    }

    void Update()
    {
        timer += Time.deltaTime;
        HandleLaser();
        //HandleAngledShooting();
    }

    void FixedUpdate()
    {
        if(m_LMS.m_RoundStarted)
        {
            Movement();
            Shooting();
        }
    }

    void HandleAngledShooting()
    {
        if(InputManager.ActiveDevice.Action4.WasPressed)
        {
            if (m_Angled)
                m_Angled = false;
            else
                m_Angled = true;
            Debug.Log(m_Angled);
        }

        if (m_Angled)
            m_ShootingAngles = 16;
        else
            m_ShootingAngles = 360;
    }

    void Movement()
    {
        movementInput = new Vector3(m_Device.GetControl(InputControlType.LeftStickX).RawValue, 0, m_Device.GetControl(InputControlType.LeftStickY).RawValue);
        rotationInput = new Vector3(m_Device.GetControl(InputControlType.RightStickX), 0, m_Device.GetControl(InputControlType.RightStickY));

        if (movementInput.magnitude > 1)
        {
            movementInput.Normalize();
        }
        if (rotationInput.magnitude < deadzone)
        {
            rotationInput = Vector3.zero;
        }
        // Rigidbody Movement
        if (movementInput.magnitude < deadzone)
        {
            movementInput = Vector3.zero;
        }
        if (rb.velocity.magnitude < maxSpeed)
        {
            rb.AddForce(movementInput * moveSpeed);
        }

        if (m_Device.GetControl(InputControlType.LeftBumper).IsPressed)
        {
            m_Shields.transform.Rotate(0, -5, 0);
        }

        if (m_Device.GetControl(InputControlType.RightBumper).IsPressed)
        {
            m_Shields.transform.Rotate(0, 5, 0);
        }
        Rotation();
    }

    void Shooting()
    {
        if (m_Device.GetControl(InputControlType.RightTrigger).IsPressed && timer > fireDelay && m_CanShoot)
        {
            m_ShotParticle.Play();
            m_AS.pitch = Random.Range(0.5f, 1.3f);
            m_AS.Play();
            timer = 0f;
            GameObject _bullet = (GameObject)Instantiate(m_Bullet, (new Vector3(m_BulletSpawn.position.x, m_BulletSpawn.position.y, m_BulletSpawn.position.z )), m_Drone.transform.rotation);
            _bullet.GetComponent<BulletScript>().m_ID = m_PlayerID;
            _bullet.GetComponent<Rigidbody>().AddForce(m_BulletSpawn.forward * m_BulletSpeed, ForceMode.Impulse);
            m_CameraScript.Shake(0.25f, 0.1f);
            Destroy(_bullet, 3);
        }
    }

    public void HitVibration()
    {
        StartCoroutine(VibrateFromHit());
    }

    IEnumerator VibrateFromHit()
    {
        m_Device.Vibrate(0.6f);
        yield return new WaitForSeconds(0.2f);
        m_Device.StopVibration();
    }

    void Rotation()
    {
        if (rotationInput.magnitude > deadzone)
        {
            float _angle = m_Device.RightStick.Angle;
            for (int i = 0; i < m_ShootingAngles; i++)
            {
                if(_angle > (m_Major * i) - m_Minor && _angle < (m_Major * i) + m_Minor)
                {
                    turretRotation.y = (m_Major * i);
                }
            }
            m_Drone.transform.rotation = Quaternion.Lerp(Quaternion.Euler(turretRotation), m_Drone.transform.rotation, droneSpeed);
        }
    }

    void HandleLaser()
    {
        Ray _ray = new Ray(m_BulletSpawn.position, m_BulletSpawn.forward);
        RaycastHit _hit;
        m_LineRenderer.SetPosition(0, _ray.origin);
        if (Physics.Raycast(_ray, out _hit, 1000f))
        {
            m_LineRenderer.SetPosition(1, _hit.point);
        }
        else
        {
            m_LineRenderer.SetPosition(1, _ray.direction * 10f);    
        }
        Debug.DrawLine(_ray.origin, _hit.point, Color.red);
    }
}
