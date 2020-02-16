using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseBehavior : MonoBehaviour {

	GameManager gameManager;
	public GameObject checkPointPrefab;

	RaycastHit hit;
	public int MovingPlayer {
		get { return _movingPlayer; }

		set { _movingPlayer = value; }
	}
	[SerializeField]
	int _movingPlayer = 0;

	void Start() {
		gameManager =  this.GetComponent<GameManager>();
		MovingPlayer = gameManager.Friendly.Count + 1;
	}

	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown (0)) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

			if (Physics.Raycast (ray, out hit)) {
				GameObject ground = gameManager.PlayableArea;
				float x = (ground.transform.localScale.x / 2) - 2f;
				float z = (ground.transform.localScale.z / 2) - 2f;

				Vector3 hitPoint;
				hitPoint.x = Mathf.Clamp (hit.point.x, -(x - 1f), x - 1f);
				hitPoint.z = Mathf.Clamp (hit.point.z, -(z - 1f), z - 1f);
				hitPoint.y = 0;
					
				if (GameObject.FindGameObjectsWithTag ("Move Point").Length > 0) {
					GameObject[] allMovePoints = GameObject.FindGameObjectsWithTag ("Move Point");
					foreach (GameObject mP in allMovePoints) {
						Destroy (mP.gameObject);
					}
				}

				if (MovingPlayer >= gameManager.Friendly.Count) {
					List<Vector3> movePointList = new List<Vector3> ();

					foreach (GameObject friendly in gameManager.Friendly) {
						if (friendly.GetComponent<Friendly> ().Dead == false) {
							bool invalid = false;
							Vector3 movePoint;
							int counter = 0;
							do {

								if (invalid)
									invalid = false;


								movePoint = GetMovePoints (friendly, hitPoint);

								if (movePointList.Count > 0) {
									foreach (Vector3 point in movePointList) {
										if (Vector3.Distance (point, movePoint) < 2f) {
											invalid = true;
										}
									}

									if (!invalid) {
										movePointList.Add (movePoint);
									}
								} else {
									movePointList.Add (movePoint);
								}

								counter++;

								if (counter > 10) {
									movePoint = new Vector3 (0, friendly.transform.position.y, 0);
									movePointList.Add (movePoint);
									invalid = false;
								}

								GameObject checkPoint = (GameObject)Instantiate (checkPointPrefab, movePoint, friendly.transform.rotation);

								if (invalid) {
									Destroy (checkPoint.gameObject);
								}
							} while (invalid);

							friendly.GetComponent<Friendly> ().MovePosition = movePoint;
						}
					}
				} else {
					Friendly movingPlayer = gameManager.Friendly [MovingPlayer].GetComponent<Friendly>();

					if (movingPlayer.Dead == false)
						movingPlayer.MovePosition = hitPoint;
				}
			}
		}
	}

	public Vector3 GetMovePoints(GameObject player, Vector3 hitSpot) {

		GameObject ground = gameManager.PlayableArea;
		float groundX = (ground.transform.localScale.x / 2);
		float groundZ = (ground.transform.localScale.z / 2);

		Vector3 pos;
		pos.x = Mathf.Clamp(Random.Range(hitSpot.x - 2f, hitSpot.x + 2f), -groundX, groundX);
		pos.z = Mathf.Clamp(Random.Range(hitSpot.z - 2f, hitSpot.z + 2f), -groundZ, groundZ);
		pos.y = player.transform.position.y;
		return pos;
	}
}
