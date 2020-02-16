using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public bool FriendlyBullet = false;

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Enemy" && FriendlyBullet == true) {
			other.GetComponent<Enemy> ().Health -= 10f;
			Destroy (this.gameObject);
		}

		if (other.tag == "Friendly" && FriendlyBullet == false) {
			other.GetComponent<Friendly> ().Health -= 10f;
			Destroy (this.gameObject);
		}
	}

}
