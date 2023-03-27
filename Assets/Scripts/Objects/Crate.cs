using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Stops the crate from moving once the player has it in the correct position.
  *
  * Plays a sound when the crate collides.
  */

public class Crate : MonoBehaviour {

    public AudioClip crateSound;
    public bool canPlay = true;

    void OnTriggerEnter2D(Collider2D other){
        if (other.tag == "StaticCrate"){
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Static;;
        }
    }

    IEnumerator waitTime(){
        yield return new WaitForSeconds(0.4f);
        canPlay = true;
    }
    void OnCollisionEnter2D(Collision2D other){
        if(canPlay){
            canPlay = false;
            AudioSource.PlayClipAtPoint(crateSound, transform.position);
            StartCoroutine(waitTime());
        }
    }

}

