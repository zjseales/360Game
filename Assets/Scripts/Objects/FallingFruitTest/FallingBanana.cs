using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** FallingBanana.cs
  * COSC360 Game Assignment
  * Team Goold
  *
  * Defines the movement and collision triggers of the falling banana.
  */

public class FallingBanana : MonoBehaviour {

	[SerializeField] GameObject projectile;

	float moveSpeed = 7f;

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
        if (col.tag == "Armadillo") {
			Debug.Log ("Hit!");
			Destroy (gameObject);
			BossLevel1.PlayerHit();
		} else if (col.tag == "DestroyBanana"){
            Destroy(gameObject);
            FallingFruit.current--;
        }
	}

}
