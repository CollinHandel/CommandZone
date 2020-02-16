using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

	public GameObject Target;
	public GameObject bulletPrefab;

	public GameManager gameManager;
	public bool Dead {
		get { return _dead; }
		set { _dead = value; }
	}
	private bool _dead = false;

	public Image healthBar;

	public float startHealth = 100;
	public float Health {
		get { return _health; }
		set { 
			_health = value; 
			healthBar.fillAmount = Health / startHealth;

			if (_health <= 0)
				Die ();
		}
	}
	private float _health = 100f;

	public Vector3 MovePosition {
		get { 
			return _movePosition; 
		}
		set {
			_movePosition = value;
			nav.Resume ();
			nav.SetDestination (MovePosition);

			if (attacking)
			{
				moving = true;
				attacking = false;
			}
		}
	}
	private Vector3 _movePosition;

	NavMeshAgent nav;
	public bool moving = false;
	public bool attacking = false;
	float fireRate = 1f;
	float setRate = 1f;


	void Awake() {
		nav = gameObject.GetComponent<NavMeshAgent> ();
		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		Health = startHealth;
	}

	void Update() {

		Target = null;
		foreach (GameObject friendly in gameManager.Friendly) {
			if (friendly.GetComponent<Friendly> ().Dead == true) {
				attacking = false;
				continue;
			}

			if (Vector3.Distance (transform.position, friendly.transform.position) < 5) {
				gameObject.GetComponent<Enemy> ().Target = friendly;
			}
		}

		if (moving == false) {			
			if (Target && !attacking) {
				attacking = true;
				nav.Stop ();
				nav.SetDestination(transform.position);
			} else {
				attacking = false;
			}

			if (attacking)
				Attack ();
		} else {
			if (nav.pathStatus == NavMeshPathStatus.PathComplete || nav.remainingDistance <= .4f) {
				moving = false;
			}
		}

	}

	void Attack() {
		if (Target != null) {
			if (Vector3.Distance (transform.position, Target.transform.position) > 5) {
				attacking = false;
				return;
			}

			Vector3 targetDir = Target.transform.position - transform.position;
			float step = 4 * Time.deltaTime;
			Vector3 newDir = Vector3.RotateTowards (transform.forward, targetDir, step, 0.0f);
			Debug.DrawRay (transform.position, newDir, Color.red);
			transform.rotation = Quaternion.LookRotation (newDir);

			fireRate -= Time.fixedDeltaTime;

			if (fireRate <= 0) {
				var bullet = (GameObject)Instantiate (bulletPrefab, transform.GetChild (0).transform.position, transform.rotation);
				bullet.GetComponent<Rigidbody> ().velocity = bullet.transform.forward * 6;
				bullet.GetComponent<Bullet> ().FriendlyBullet = false;
				Destroy (bullet, 2.0f);
				fireRate = setRate;
			}
		}
	}


	void Die() {
		Dead = true;
		gameObject.SetActive(false);
	}

}
