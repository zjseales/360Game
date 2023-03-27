using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveBanana1 : MonoBehaviour {

	public float spinSpeed = 5f;

    [SerializeField] GameObject bullet;

	float moveSpeed = 6f;

	Rigidbody2D rb;

	PlayerManager target;
	Vector2 moveDirection;

    //initialize the array of projectiles
    void Awake(){
    }

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		target = GameObject.FindObjectOfType<PlayerManager>();
        moveDirection = (target.transform.position - transform.position).normalized * moveSpeed;
       	//applies force in that direction,
		rb.AddForce(new Vector2 (moveDirection.x - 2.5f, moveDirection.y), ForceMode2D.Impulse);
		//Adds rotation to the banana
		rb.AddTorque(spinSpeed, ForceMode2D.Impulse);
		Destroy(gameObject, 3.1f);
	}

	void OnTriggerEnter2D (Collider2D col) {
		if (col.gameObject.name.Equals ("Armadillo")) {
			Debug.Log ("Hit!");
			Destroy (gameObject);
            BossLevel1.PlayerHit();
		} else if (col.tag == "DestroyBanana") {
			Destroy(gameObject);
		}
	}

}
