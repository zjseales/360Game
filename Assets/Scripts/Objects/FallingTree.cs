using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** FallingTree.cs
  * COSC360 Game Assignment
  * Team Goold
  *
  * Defines the mechanics of the falling tree objects.
  * (A tipping box that starts when it collides with the armadillo ball).
  * The tree will rotate clockwise about it's parent object pivot before stopping when it hits
  * a "TreeLanding" collider tag.
  */

public class FallingTree : MonoBehaviour {

    //The speed at which the tree falls
    public float fallSpeed = -0.7f;
    //falling variable, true if tree is in motion
    private bool falling;
    //true if tree has already fallen
    private bool fallen;

    public AudioClip treeFall = null;

    SpriteRenderer spriteRend;

    // Initializes the tree and it's variables
    void Start() {
        spriteRend = GetComponent<SpriteRenderer>();
        falling = false;
        fallen = false;
        spriteRend.color = new Color(1f,1f,1f);
    }

    //Tree tips over if collided with the Armadillo tag
    void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Armadillo" && BossLevel1.phase3 == true){
            GetComponent<BoxCollider2D>().isTrigger = false;
            BossLevel1.playerInvincible = true;
            AudioSource.PlayClipAtPoint(treeFall, transform.position);
            falling = true;
            
        }
        //Turns off Collider trigger once tree has fallen
        if(other.tag == "TreeLanding"){
            falling = false;
            fallen = true;
            GetComponent<BoxCollider2D>().isTrigger = false;
        } else if (other.tag == "Water"){
            falling = false;
            fallen = true;
            GetComponent<BoxCollider2D>().isTrigger = false;
            GetComponent<Rigidbody2D>().isKinematic = false;
        } else if (other.tag == "Monkey") { 
            BossLevel1.bossHit();
            falling = false;
            fallen = true;
            GetComponent<BoxCollider2D>().isTrigger = false;
        }

    }

    // Update is called once per frame
    void Update() {
        //moves the tree
        if (falling == true && fallen == false){
            //rotate tree clockwise about the parent pivot point
            transform.parent.Rotate(0f, 0f, -fallSpeed, Space.World);
        }
        if (BossLevel1.phase3){
            spriteRend.color = new Color(0.53f,0.53f,0.53f);
        }
    }
}
