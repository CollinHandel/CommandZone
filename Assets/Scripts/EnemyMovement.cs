using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour {

	public GameObject checkPointPrefab;

	GameManager gameManager;
	GameObject ground;
	bool moving = false;
	float timer = 4f;

	void Awake() {
		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		ground = gameManager.PlayableArea;
	}

	void Update() {

		if (moving == true) {
			int enemyCount = 0;
			int enemiesNotMoving = 0;
			foreach (GameObject enemy in gameManager.Enemy) {
				if (enemy.GetComponent<Enemy> ().Dead)
					continue;
				else
					enemyCount++;

				if (enemy.GetComponent<NavMeshAgent> ().remainingDistance <= .4f && enemy.GetComponent<Enemy>().attacking != true) {
					enemiesNotMoving++;
				}
			}

			if (enemyCount == enemiesNotMoving) {
				moving = false;
			}
		} else {

			if (timer <= 0) {
				if (GameObject.FindGameObjectsWithTag ("Move Point").Length > 0) {
					GameObject[] allMovePoints = GameObject.FindGameObjectsWithTag ("Move Point");
					foreach (GameObject mP in allMovePoints) {
						Destroy (mP.gameObject);
					}
				}

				moving = true;

				float x = (ground.transform.localScale.x / 2) - 2f;
				float z = (ground.transform.localScale.z / 2) - 2f;

				Vector3 pos;
				pos.x = Mathf.Clamp (Random.Range(-(x - 2.3f), x - 2.3f), -(x - 2.3f), x - 2.3f);
				pos.z = Mathf.Clamp (Random.Range(-(z - 2.3f), z - 2.3f), -(z - 2.3f), z - 2.3f);
				pos.y = 0;

				List<Vector3> movePointList = new List<Vector3>();

				foreach (GameObject enemy in gameManager.Enemy) {
					if (enemy.GetComponent<Enemy> ().Dead == false) {
						bool invalid = false;
						Vector3 movePoint;

						do {

							if (invalid)
								invalid = false;
						
							movePoint = GetComponent<MouseBehavior> ().GetMovePoints (enemy, pos);
							GameObject checkPoint = (GameObject)Instantiate (checkPointPrefab, movePoint, enemy.transform.rotation);

							if (movePointList.Count > 0) {
								foreach (Vector3 point in movePointList) {
									if (Vector3.Distance (point, movePoint) < 1f) {
										invalid = true;
									}
								}

								if (!invalid) {
									movePointList.Add (movePoint);
								}
							} else {
								movePointList.Add (movePoint);
							}
						
							if (invalid) {
								Destroy (checkPoint.gameObject);
							}
						} while (invalid);

						enemy.GetComponent<Enemy> ().MovePosition = movePoint;
					}
				}

				timer = 4f;
			}

			timer -= Time.fixedDeltaTime;
		}
	}

}
