using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

    Hashtable m_FirstHash;
    Hashtable m_SecondHash;

    private Ray m_Left, m_Center, m_Right;

    private Vector3 m_LeftVec, m_RightVec;

	//public ParticleSystem m_BulletParticle;
    public LMS m_LMS;
    private PlayerControllerManager m_PlayerControllerManager;
    public int m_ID = -1;
    public int m_HitID = 0;
    private MeshRenderer m_Renderer;
    private Material[] m_Materials;
    private TrailRenderer m_Trail;
    private Light m_Light;

    private Rigidbody m_RB;

    void Awake()
    {
        m_FirstHash = new Hashtable();
        m_SecondHash = new Hashtable();
        
        m_FirstHash.Add("x", 3.5f);
        m_FirstHash.Add("z", 2f);
        m_FirstHash.Add("time", 0.05f);
        m_FirstHash.Add("looptype", iTween.LoopType.none);
        m_FirstHash.Add("easetype", iTween.EaseType.easeInQuint);

        m_SecondHash.Add("x", .75f);
        m_SecondHash.Add("z", 4f);
        m_SecondHash.Add("delay", 0.05f);
        m_SecondHash.Add("time", 0.25f);
        m_SecondHash.Add("looptype", iTween.LoopType.none);
        m_SecondHash.Add("easetype", iTween.EaseType.easeOutQuint);


        m_LMS = FindObjectOfType<LMS>();
        m_PlayerControllerManager = FindObjectOfType<PlayerControllerManager>();


        m_RB = GetComponent<Rigidbody>();
    }

    void Start()
    {
        var _col = GetComponent<SphereCollider>();
        m_LeftVec = new Vector3(-_col.bounds.extents.x, transform.position.y, transform.position.z);
        m_RightVec = new Vector3(_col.bounds.extents.x, transform.position.y, transform.position.z);

        m_Light = GetComponentInChildren<Light>();
        m_Light.color = m_LMS.m_PlayerColors[m_ID];

        m_Materials = gameObject.GetComponentInChildren<MeshRenderer>().materials;
        for (int i = 0; i < m_Materials.Length; i++)
        {
            m_Materials[i].SetColor("_Color", m_LMS.m_PlayerColors[m_ID]);
        }

        gameObject.GetComponentInChildren<MeshRenderer>().materials = m_Materials;

        m_Materials = gameObject.GetComponentInChildren<TrailRenderer>().materials;
        for (int i = 0; i < m_Materials.Length; i++)
        {
            m_Materials[i].SetColor("_Color", m_LMS.m_PlayerColors[m_ID]);
        }
        gameObject.GetComponentInChildren<TrailRenderer>().materials = m_Materials;

        iTween.ScaleTo(transform.GetChild(0).gameObject, m_FirstHash);
        iTween.ScaleTo(transform.GetChild(0).gameObject, m_SecondHash);
    }

    void FixedUpdate()
    {
        HandleRays();
    }

    void HandleRays()
    {
        m_Left = new Ray(m_LeftVec, transform.forward);
        m_Center = new Ray(transform.position, transform.forward);
        m_Right = new Ray(m_RightVec, transform.forward);

        Debug.DrawRay(m_Left.origin, m_Left.direction, Color.red);
        Debug.DrawRay(m_Center.origin, m_Center.direction, Color.red);
        Debug.DrawRay(m_Right.origin, m_Right.direction, Color.red);
    }

	void OnCollisionEnter (Collision other)
    {
        if(other.gameObject.CompareTag("Core"))
        {
            var _pc = other.gameObject.GetComponentInParent<PlayerController>();
            m_HitID = _pc.m_PlayerID;
            _pc.m_CameraScript.Shake(0.9f, 0.75f);

            for (int i = 0; i < 4; i++)
            {
                if (m_HitID == i)
                {
                    m_LMS.m_PlayersAlive[i] = 0;
                    m_LMS.ManagePlayers();
                    break;
                }
            }
            m_LMS.m_PlayerKillsThisRound[m_ID]++;
            m_LMS.m_PlayerScores[m_ID] += m_LMS.m_ScorePerKill;
            Destroy(gameObject);
        }
        else if(other.gameObject.CompareTag("BounceWall"))
        {
            Quaternion _rot = Quaternion.LookRotation(m_RB.velocity, Vector3.forward);
            transform.rotation = _rot;
        }
        else
            Destroy(gameObject);
    }
}
