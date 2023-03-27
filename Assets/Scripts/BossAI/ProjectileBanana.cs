using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBanana : MonoBehaviour {

	[SerializeField] GameObject bullet;

	float moveSpeed = 7f;

	Rigidbody2D rb;

	PlayerManager target;
	Vector2 moveDirection;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		target = GameObject.FindObjectOfType<PlayerManager>();

		Instantiate (bullet, transform.position, Quaternion.identity);
		moveDirection = (target.transform.position - transform.position).normalized * moveSpeed;
		rb.velocity = new Vector2 (moveDirection.x, moveDirection.y);
		Destroy (gameObject, 3f);
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.gameObject.name.Equals ("Armadillo")|| col.gameObject.name.Equals("Snake") || col.gameObject.name.Equals("Monkey")) {
			Debug.Log ("Hit!");
			Destroy (gameObject);
			GameObject.Find("snakke").GetComponent<BossMovement>().SwapPlace();
		}
	}

}
