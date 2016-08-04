using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneMan : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			SceneManager.LoadScene ("Volcano - Prototype");
		}
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			SceneManager.LoadScene ("Snow - Prototype");
		}
		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			SceneManager.LoadScene ("Desert - Prototype");
		}
	}
}
