using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class LastManStanding : MonoBehaviour {

	public GameObject[] playerList;

	public Canvas winCanvas;
	public Canvas pauseCanvas;
	public Text winningPlayer;
	public Text restartText;
	public Button restart;
	public Button pauseToMenu;
	public Button endToMenu;
	public Text winText;
	public int playerOneScore;
	public int playerTwoScore;
	public int playerThreeScore;
	public int playerFourScore;
	public Text p1Score;
	public Text p2Score;
	public Text p3Score;
	public Text p4Score;
	public Text roundText;
	public int roundNumber;
	public int winningScore = 5;

	public static LastManStanding instance;

	public bool isGameOver = false;

	private GameObject p1Prefab;
	private GameObject p2Prefab;
	private GameObject p3Prefab;
	private GameObject p4Prefab;
	public Transform p1Spawn;
	public Transform p2Spawn;
	public Transform p3Spawn;
	public Transform p4Spawn;

	private string currentScene;

	// Use this for initialization
	void Start () {

		instance = this;

		currentScene = SceneManager.GetActiveScene ().name;

		p1Prefab = Resources.Load ("Player1") as GameObject;
		p2Prefab = Resources.Load ("Player2") as GameObject;
		p3Prefab = Resources.Load ("Player3") as GameObject;
		p4Prefab = Resources.Load ("Player4") as GameObject;

		playerOneScore = 0;
		playerTwoScore = 0;
		playerThreeScore = 0;
		playerFourScore = 0;

		roundNumber = 1;

		pauseCanvas.enabled = false;

		playerList = GameObject.FindGameObjectsWithTag ("Player");

		winCanvas.enabled = false;

		if (PlayerPrefs.GetInt("playerOneActive") == 1) {
			Instantiate (p1Prefab, p1Spawn.position, p1Spawn.rotation);
		}

		if (PlayerPrefs.GetInt("playerTwoActive") == 1) {
			Instantiate (p2Prefab, p2Spawn.position, p2Spawn.rotation);
		}

		if (PlayerPrefs.GetInt("playerThreeActive") == 1) {
			Instantiate (p3Prefab, p3Spawn.position, p3Spawn.rotation);
		}

		if (PlayerPrefs.GetInt("playerFourActive") == 1) {
			Instantiate (p4Prefab, p4Spawn.position, p4Spawn.rotation);
		}

	}

	// Update is called once per frame
	void Update () {

		if (isGameOver == true) {
			pauseToMenu.interactable = false;
			winCanvas.enabled = true;
			restart.interactable = true;
			endToMenu.interactable = true;
			Time.timeScale = 0.5f;
		} else if (isGameOver == false && pauseCanvas.enabled == false) {
			winCanvas.enabled = false;
			restart.interactable = false;
			endToMenu.interactable = false;
			Time.timeScale = 1;
		}

		if (Input.GetKeyDown (KeyCode.P) || Input.GetButtonDown ("P1Back") || Input.GetButtonDown ("P2Back") || Input.GetButtonDown ("P3Back") || Input.GetButtonDown ("P3Back")) {
			if (isGameOver == false) {
				if (Time.timeScale == 1) {
					StartCoroutine (PauseGameSelect ());
					pauseCanvas.enabled = true;
					Time.timeScale = 0;
				} else if (Time.timeScale == 0) {
					StopCoroutine (PauseGameSelect ());
					Time.timeScale = 1;
					pauseCanvas.enabled = false;
				} else {

				}
			}
		}

		if (pauseCanvas.enabled == false) {
			pauseToMenu.interactable = false;
		} else if (pauseCanvas.enabled == true) {
			pauseToMenu.interactable = true;
		}

		p1Score.text = "" + playerOneScore;
		p2Score.text = "" + playerTwoScore;
		p3Score.text = "" + playerThreeScore;
		p4Score.text = "" + playerFourScore;
		roundText.text = "Round " + roundNumber;

		playerList = GameObject.FindGameObjectsWithTag ("Player");


		if (playerList.Length == 1) {
			if (playerList [0].transform.parent.name == "Player1") {
				if (isGameOver == false) {
					StartCoroutine (EndGameSelect ());
					isGameOver = true;
					playerOneScore += 1;
					winningPlayer.text = "Player One";
					winningPlayer.color = p1Score.color;
				}
			} else if (playerList [0].transform.parent.name == "Player2") {
				if (isGameOver == false) {
					StartCoroutine (EndGameSelect ());
					isGameOver = true;
					playerTwoScore += 1;
					winningPlayer.text = "Player Two";
					winningPlayer.color = p2Score.color;
				}
			} else if (playerList [0].transform.parent.name == "Player3") {
				if (isGameOver == false) {
					StartCoroutine (EndGameSelect ());
					isGameOver = true;
					playerThreeScore += 1;
					winningPlayer.text = "Player Three";
					winningPlayer.color = p3Score.color;
				}
			} else if (playerList [0].transform.parent.name == "Player4") {
				if (isGameOver == false) {
					StartCoroutine (EndGameSelect ());
					isGameOver = true;
					playerFourScore += 1;
					winningPlayer.text = "Player Four";
					winningPlayer.color = p4Score.color;
				}
			}
		}

		if (playerOneScore == winningScore || playerTwoScore == winningScore || playerThreeScore == winningScore || playerFourScore == winningScore) {
			restartText.text = "Restart";
		}
	}

	public void ResetArena () {

		if (playerOneScore < winningScore && playerTwoScore < winningScore && playerThreeScore < winningScore && playerFourScore < winningScore) {
			roundNumber += 1;
			isGameOver = false;
			if (playerList.Length == 1) {
				Destroy (playerList [0].transform.parent.gameObject);
			}
			if (PlayerPrefs.GetInt("playerOneActive") == 1) {
				Instantiate (p1Prefab, p1Spawn.position, p1Spawn.rotation);
			}
			if (PlayerPrefs.GetInt("playerTwoActive") == 1) {
				Instantiate (p2Prefab, p2Spawn.position, p2Spawn.rotation);
			}
			if (PlayerPrefs.GetInt("playerThreeActive") == 1) {
				Instantiate (p3Prefab, p3Spawn.position, p3Spawn.rotation);
			}
			if (PlayerPrefs.GetInt("playerFourActive") == 1) {
				Instantiate (p4Prefab, p4Spawn.position, p4Spawn.rotation);
			}
		}  else if (playerOneScore == winningScore || playerTwoScore == winningScore || playerThreeScore == winningScore || playerFourScore == winningScore) {
			SceneManager.LoadScene (currentScene, LoadSceneMode.Single);
		}
	}

	public void BackToMenu () {
		SceneManager.LoadScene ("Main Menu"); 
		Time.timeScale = 1;
	}

	public IEnumerator EndGameSelect () {
		yield return new WaitForSeconds (0);
		EventSystem.current.SetSelectedGameObject (restart.gameObject);
	}
	public IEnumerator PauseGameSelect () {
		yield return new WaitForSeconds (0);
		EventSystem.current.SetSelectedGameObject (pauseToMenu.gameObject);
	}

}
