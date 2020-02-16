using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControls : MonoBehaviour {

	public GameObject PlayerSelector;
	GameManager gameManager;
	[SerializeField]
	public List<GameObject> selectors = new List<GameObject>();

	void Start() {
		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> ();

		foreach (GameObject player in gameManager.Friendly) {

			GameObject selector = (GameObject)Instantiate (PlayerSelector, transform.position, Quaternion.identity);
			selector.name = player.name;
			selector.transform.SetParent (this.transform, false);
			selector.transform.GetChild (1).GetComponent<Text> ().text = player.name;
			selector.transform.GetChild (0).GetComponent<Image> ().color = player.GetComponent<Renderer> ().material.color;
			selectors.Add (selector.gameObject);
		}

		GameObject unitSelector = (GameObject)Instantiate (PlayerSelector, transform.position, Quaternion.identity);
		unitSelector.name = "Unit";
		unitSelector.transform.SetParent (this.transform, false);
		unitSelector.transform.GetChild (1).GetComponent<Text> ().text = "Unit";
		unitSelector.transform.GetChild (0).GetComponent<Image> ().color = Color.green;
		selectors.Add (unitSelector.gameObject);

	}

	public void Selector(GameObject player) {
		int count = 0;

		List<GameObject> foundSelectors = GameObject.Find ("PlayerSelect").GetComponent<PlayerControls> ().selectors;

		foreach (GameObject selector in foundSelectors) {
			if (player.name == selector.name) {
				break;
			}
			count++;
		}

		if (count == foundSelectors.Count - 1) {
			foreach (GameObject selector in foundSelectors) {
				selector.transform.GetChild (2).GetComponent<Text> ().text = "Moving";
			}
		} else {
			for (int i = 0; i < foundSelectors.Count; i++) {
				if (i == count) {
					foundSelectors [i].transform.GetChild (2).GetComponent<Text> ().text = "Moving";
				} else {
					foundSelectors [i].transform.GetChild (2).GetComponent<Text> ().text = "Standby";
				}
			}
		}

		GameObject.Find ("GameManager").GetComponent<MouseBehavior> ().MovingPlayer = count;
	}

}








