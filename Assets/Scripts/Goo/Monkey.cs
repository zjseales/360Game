using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Monkey.cs
  * COSC360 Game Assignment
  * Team Goold
  *
  * Defines the monkey's animation and collision interactions.
*/

public class Monkey : MonoBehaviour {
    Animator MonkeyAnim;

    public AudioSource audio;

    public float maxClimbSpeed;
    public float climbSpeed;
    //flag to indicate if the monkey is at a vine object
    public bool atVine = false;
    //Parent Rigidbody component - (needs to move all animals child forms)
    Rigidbody2D parentRigidbody;

    void Awake(){
        parentRigidbody = GetComponentInParent<Rigidbody2D>();
        MonkeyAnim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }
    //Start, initializes the parent rigidbody component
    void Start(){
    }

    //Collision with a collider trigger
    void OnTriggerEnter2D(Collider2D other){
        parentRigidbody = GetComponentInParent<Rigidbody2D>();
        //Sets gravity of the parent to 0 when at a vine as a monkey.
        if(other.tag == "Vine"){
            audio.Play();
            atVine = true;
            parentRigidbody.gravityScale = 0;
            MonkeyAnim.SetBool("MonkeyClimbing", true);
            MonkeyAnim.SetBool("MonkeyWalking", false);
        }

    }
    //Exit collision trigger
    void OnTriggerExit2D(Collider2D other){
        //Turns gravity back on once vine is no longer in collision
        if(other.tag == "Vine"){
            audio.Stop();
            atVine = false;
            parentRigidbody.gravityScale = 1;
            MonkeyAnim.SetBool("MonkeyClimbing", false);
            MonkeyAnim.SetBool("MonkeyWalking", true);
        }
    }

    // Update is called once per frame
    void Update() {
        
        //if at vine, stops falling
        if(parentRigidbody.velocity.y < 0 && atVine){
            parentRigidbody.velocity = new Vector2(parentRigidbody.velocity.x, 0);
        }
        
        //transforms the parent rigidbody, so as to move all animal sprites
        if (atVine && parentRigidbody.velocity.magnitude < maxClimbSpeed){
            parentRigidbody.AddForce(Time.deltaTime * Vector2.up * climbSpeed);
        }
        
    }
}
