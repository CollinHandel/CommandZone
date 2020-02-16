using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

	public GameObject GameOver;
	public GameObject WonText;
	public GameObject LostText;
	public bool Won = false;

	public void StartGame() {
		SceneManager.LoadScene (1);
	}

	public void GameOverAppear() {
		if (Won) {
			WonText.SetActive (true);
			LostText.SetActive (false);
		} else {
			WonText.SetActive (false);
			LostText.SetActive (true);
		}

		GameOver.SetActive (true);
	}

	public void Menu() {
		SceneManager.LoadScene (0);
	}

	public void Restart() {
		SceneManager.LoadScene (1);
	}

}
