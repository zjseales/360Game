using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Fall.cs
  * COSC360 Game Assignment
  * Team Goold
  *
  * Defines the movement and collision triggers of the falling projectiles for the final boss
  */

public class Fall : MonoBehaviour {

	[SerializeField] GameObject projectile;

	float moveSpeed = 3f;

	Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
        rb.AddForce(Vector2.down * moveSpeed, ForceMode2D.Impulse);
        //Adds rotation to the banana
		rb.AddTorque(6f, ForceMode2D.Impulse);
	}

    //called when the projectile collides
	void OnTriggerEnter2D (Collider2D col) {
        if (col.tag == "Armadillo" || col.tag == "Monkey" || col.tag == "Snake") {
			Debug.Log ("Hit!");
			Destroy (gameObject);
			Boss4.PlayerHit();
		} else if (col.tag == "DestroyBanana"){
            Destroy(gameObject);
        }
	}

}
