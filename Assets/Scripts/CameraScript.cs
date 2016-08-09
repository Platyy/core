using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraScript : MonoBehaviour {
	
	//public GameObject target;
	private Vector3 m_OriginalPos;

    private List<GameObject> m_Players;

    public float m_Amplitude = 0.1f;

	private Vector3 initialPos;

	public bool isShaking = false;
    
    public static void DrawHalfScreenQuad(Material _mat)
    {
        GL.PushMatrix();
        GL.LoadOrtho();

        if(_mat == null)
        {
            Debug.LogError("You must assign a material in the inspector.");
        }
        _mat.SetPass(0);
        GL.Begin(GL.QUADS);
        GL.Color(Color.magenta);
        GL.TexCoord2(0, 0);
        GL.Vertex3(0, 0, 0.1f);

        GL.TexCoord2(1, 0);
        GL.Vertex3(1, 0, 0.1f);

        GL.TexCoord2(1, 1);
        GL.Vertex3(1, 1, 0.1f);

        GL.TexCoord2(0, 1);
        GL.Vertex3(0, 1, 0.1f);
        GL.End();
        GL.PopMatrix();

    }

	void Start ()
    {
        m_OriginalPos = transform.position;
	}
	
	void Update ()
    {
		if (isShaking) {
			transform.localPosition = m_OriginalPos + Random.insideUnitSphere * m_Amplitude;
		}
    }

	public void Shake (float _amplitude, float _duration)
    {
        m_Amplitude = _amplitude;
		isShaking = true;
		Invoke ("StopShaking", _duration);
	}

	public void StopShaking ()
    {
		isShaking = false;
	}
}
