using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack2 : MonoBehaviour {

	public float spinSpeed;

    [SerializeField] GameObject bullet;

	float moveSpeed = 7f;

	Rigidbody2D rb;

	GameObject target;
	Vector2 moveDirection;

    //initialize the array of projectiles
    void Awake(){
    }

	// Use this for initialization
	void Start () {
		target = GameObject.FindObjectOfType<PlayerManager>().gameObject;
		rb = GetComponent<Rigidbody2D> ();
        moveDirection = (target.transform.position - transform.position).normalized * moveSpeed;
       	//applies force in that direction,
		rb.AddForce(new Vector2 (moveDirection.x, moveDirection.y), ForceMode2D.Impulse);
		//Adds rotation to the banana
		rb.AddTorque(spinSpeed, ForceMode2D.Impulse);
		Destroy(gameObject, 3.1f);
	}

	void OnTriggerEnter2D (Collider2D col) {
		if (col.tag == "Armadillo" || col.tag == "Monkey" || col.tag == "Snake") {
			Debug.Log ("Hit!");
			Destroy (gameObject);
            Boss3.PlayerHit();
		} else if (col.tag == "DestroyBanana") {
			Destroy(gameObject);
		}
	}

}
