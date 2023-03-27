using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Shrooms.cs
  * COSC 360 Game Assignment
  * Team Goold
  *
  * Applies a force to the player whenever they collide with this object.
  */

public class Shrooms : MonoBehaviour {
    public float bounceForce;
    public Vector2 bounceDirection;
    public static bool canBounce;
    public AudioClip sound;
    Animator mushroom;

  void Start(){
    canBounce = true;
        mushroom = GetComponent<Animator>();
  }
    /** Triggers a force on the player whenever a collision occurs. 
      */
	void OnTriggerEnter2D (Collider2D col) {
		if ((col.gameObject.name.Equals ("Armadillo")|| col.gameObject.name.Equals("Snake") || col.gameObject.name.Equals("Monkey")) && canBounce) {
      AudioSource.PlayClipAtPoint(sound, transform.position);
      canBounce = false;
            mushroom.SetTrigger("MushroomLaunch");
			Rigidbody2D rigidBody = GameObject.Find("Player").GetComponent<PlayerManager>().rigidBody;
      rigidBody.velocity = bounceDirection * bounceForce;
      StartCoroutine(Timer());
		}
	}

  //Used to prevent mushroom force being applied multiple times
  IEnumerator Timer(){
    yield return new WaitForSeconds(0.6f);
    canBounce = true;
  }

}
