using UnityEngine;
using InControl;
using System.Collections;
using System.Collections.Generic;


public class PlayerController : MonoBehaviour {

    public int m_PlayerID = 0;

    // Player Prefab Objects
    public GameObject m_Drone, m_Core, m_Shields, m_Bullet;

    public Transform m_BulletSpawn;

    [ColorUsage(true, true, 2.0f, 2.0f, .5f, 3.0f)] public Color m_PlayerColor;

    //Player Variables
    public float m_BulletSpeed = 10f;
    public int m_ShootingAngles = 16;
    public float m_ShieldRotationSpeed = 5f;
    public bool m_CanShoot = true;
    private bool m_Angled = false;
    public bool m_HasControls = false;
    private float m_Major, m_Minor; // Shooting Angles

    //Systems
    public CameraScript m_CameraScript;
    private LMS m_LMS; // Last Man Standing Gamemode
    private PlayerControllerManager m_ControllerManager;

    public Renderer[] m_Renderer;
    
    // Input Variables
    public float m_Acceleration; // Acceleration speed
    public float m_MaxSpeed; // Max movement speed
    public float m_AimSpeed = 0.04f;

    // Private variable
    private float m_Deadzone = 0.25f; // Analog stick deadzone

    private Rigidbody m_RB; // Player's rigidbody component
    private Vector3 m_MovementInput; // Input for movement
    private Vector3 m_RotationInput; // Input for rotation
    private Vector3 m_TurretRotation; // Current rotation

    private float m_Timer;
    public float m_FireDelay;

    // Input
    public InputDevice m_Device;
    private InputManager m_InputManager;
    
    // Line Renderer (Laser)
    public Material m_LineMaterial;
    private LineRenderer m_LineRenderer;
    
    // Audio
    public GameObject m_ShotSound;
    
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
        m_RB = GetComponent<Rigidbody>(); 

        // Creating player aim laser.
        m_LineRenderer = gameObject.AddComponent<LineRenderer>();
        m_LineRenderer.material = m_LineMaterial;
        m_LineRenderer.SetWidth(0.3f, 0.1f);
        m_LineRenderer.SetVertexCount(2);
        m_LineRenderer.useWorldSpace = true;

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

        StopVibrations(m_Device);
        m_Major = 360f / m_ShootingAngles;
        m_Minor = m_Major / 2;
        GetComponentInChildren<CoreManager>().m_ID = m_PlayerID;
    }

    void Update()
    {
        m_Timer += Time.deltaTime;
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

    public void StopVibrations(InputDevice _device)
    {
        _device.StopVibration();
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
        m_MovementInput = new Vector3(m_Device.GetControl(InputControlType.LeftStickX).RawValue, 0, m_Device.GetControl(InputControlType.LeftStickY).RawValue);
        m_RotationInput = new Vector3(m_Device.GetControl(InputControlType.RightStickX), 0, m_Device.GetControl(InputControlType.RightStickY));

        if (m_MovementInput.magnitude > 1)
        {
            m_MovementInput.Normalize();
        }
        if (m_RotationInput.magnitude < m_Deadzone)
        {
            m_RotationInput = Vector3.zero;
        }
        // Rigidbody Movement
        if (m_MovementInput.magnitude < m_Deadzone)
        {
            m_MovementInput = Vector3.zero;
        }
        if (m_RB.velocity.magnitude < m_MaxSpeed)
        {
            m_RB.AddForce(m_MovementInput * m_Acceleration);
        }

        // Rotate shields based on mutator speed.
        if (m_Device.GetControl(InputControlType.LeftBumper).IsPressed)
        {
            m_Shields.transform.Rotate(0, -m_ShieldRotationSpeed, 0);
        }
        if (m_Device.GetControl(InputControlType.RightBumper).IsPressed)
        {
            m_Shields.transform.Rotate(0, m_ShieldRotationSpeed, 0);
        }
        Rotation();
    }

    void Shooting()
    {
        if (m_Device.GetControl(InputControlType.RightTrigger).IsPressed && m_Timer > m_FireDelay && m_CanShoot)
        {
            ParticleSystem _s = (ParticleSystem)Instantiate(m_ShotParticle, (new Vector3(m_BulletSpawn.position.x, m_BulletSpawn.position.y, m_BulletSpawn.position.z)), m_Drone.transform.rotation);
            _s.GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(m_PlayerColor.r * 2, m_PlayerColor.g * 2, m_PlayerColor.b * 2));
            Destroy(_s.gameObject, 2f);

            // Random shot pitch.
            Instantiate(m_ShotSound, transform.position, Quaternion.identity);
            m_ShotSound.GetComponent<AudioSource>().pitch = Random.Range(1f, 1.3f);

            m_Timer = 0f;

            GameObject _bullet = (GameObject)Instantiate(m_Bullet, (new Vector3(m_BulletSpawn.position.x, m_BulletSpawn.position.y, m_BulletSpawn.position.z )), m_Drone.transform.rotation);
            _bullet.GetComponent<BulletScript>().m_ID = m_PlayerID;
            _bullet.GetComponent<Rigidbody>().AddForce(m_BulletSpawn.forward * m_BulletSpeed, ForceMode.Impulse);
            m_CameraScript.Shake(0.1f, 0.1f);
            StartCoroutine(BulletFade(_bullet));
            Destroy(_bullet, 3);
        }
    }

    IEnumerator BulletFade(GameObject _bullet)
    {
        yield return new WaitForSeconds(2.95f);
        if(_bullet != null)
        {
            m_LMS.PlayHitParticle(_bullet.transform.position, m_PlayerColor);
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
        if (m_RotationInput.magnitude > m_Deadzone)
        {
            float _angle = m_Device.RightStick.Angle;
            for (int i = 0; i < m_ShootingAngles; i++)
            {
                if(_angle > (m_Major * i) - m_Minor && _angle < (m_Major * i) + m_Minor)
                {
                    m_TurretRotation.y = (m_Major * i);
                }
            }
            m_Drone.transform.rotation = Quaternion.Lerp(Quaternion.Euler(m_TurretRotation), m_Drone.transform.rotation, m_AimSpeed);
        }
    }

    void HandleLaser()
    {
        Ray _ray = new Ray(m_BulletSpawn.position, m_BulletSpawn.forward);
        RaycastHit _hit;
        m_LineRenderer.SetPosition(0, _ray.origin);

        if (Physics.Raycast(_ray, out _hit, 1000f))
            m_LineRenderer.SetPosition(1, _hit.point);
        else
            m_LineRenderer.SetPosition(1, _ray.direction * 10f);

        Debug.DrawLine(_ray.origin, _hit.point, Color.red);
    }

    public void DestroyShields()
    {
        Destroy(m_Shields);
    }
}
