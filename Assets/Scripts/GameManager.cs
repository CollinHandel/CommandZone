using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public List<GameObject> Friendly;
	public List<GameObject> Enemy;
	public GameObject PlayableArea;


	void Awake() {
		foreach (GameObject friendly in GameObject.FindGameObjectsWithTag("Friendly")) {
			Friendly.Add (friendly);
		}

		foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")) {
			Enemy.Add (enemy);
		}
	}

	void Update() {
		int friendlyDead = CheckFriendlyDead (Friendly);
		int enemyDead = CheckEnemyDead (Enemy);

		if (friendlyDead >= Friendly.Count) {
			GetComponent<MenuManager> ().Won = false;
			GetComponent<MenuManager> ().GameOverAppear ();
		} else if (enemyDead >= Enemy.Count) {
			GetComponent<MenuManager> ().Won = true;
			GetComponent<MenuManager> ().GameOverAppear ();
		}
	}

	int CheckFriendlyDead(List<GameObject> Players) {
		int count = 0;
		foreach (GameObject player in Players) {
			if (player.GetComponent<Friendly> ().Dead == true) {
				count++;
			}
		}
		return count;
	}

	int CheckEnemyDead(List<GameObject> Players) {
		int count = 0;
		foreach (GameObject player in Players) {
			if (player.GetComponent<Enemy> ().Dead == true) {
				count++;
			}
		}
		return count;
	}

}
