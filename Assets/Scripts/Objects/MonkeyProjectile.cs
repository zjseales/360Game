using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** MonkeyProjectile.cs
  * COSC360 Game Assignment
  * Team Goold
  *
  * Controls the movement of a projectile thrown by the monkey boss.
  */
public class MonkeyProjectile : MonoBehaviour {

	public float moveSpeed = 10f;
	public float spinSpeed = 5f;

	Rigidbody2D rb;

	PlayerManager target;
	Vector2 moveDirection;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
        //the projectile's target (the player)
		target = GameObject.FindObjectOfType<PlayerManager>();
        //calculates vector direction to player
		moveDirection = (target.transform.position - transform.position).normalized * moveSpeed;
        //applies force in that direction, 
		rb.AddForce(new Vector2 (moveDirection.x, moveDirection.y), ForceMode2D.Impulse);
		//Adds rotation to the banana
		rb.AddTorque(spinSpeed, ForceMode2D.Impulse);
		Destroy (gameObject, 3f);
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Armadillo")
        {
            Debug.Log("Hit!");
            Destroy(gameObject);
            BossLevel1.PlayerHit();

        }
    }

}