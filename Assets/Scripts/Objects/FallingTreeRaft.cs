using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** FallingTreeRaft.cs
  * COSC360 Game Assignment
  * Team Goold
  *
  * Defines the mechanics of the falling tree raft objects.
  * (A tipping box that starts when it collides with the armadillo ball).
  * The tree will rotate anti clockwise about it's parent object pivot before stopping when it hits
  * a "Water" collider tag.
  */

public class FallingTreeRaft : MonoBehaviour {

    //The speed at which the tree falls
    public float fallSpeed = 0.7f;
    //falling variable, true if tree is in motion
    private bool falling;
    //true if tree has already fallen
    private bool fallen;

    public AudioClip splashSound;
    public GameObject splashPrefab;
    private bool playSplash;

    public AudioClip treeFall = null;

    SpriteRenderer spriteRend;

    // Initializes the tree and it's variables
    void Start() {
        playSplash = false;
        fallSpeed = 0.7f;
        spriteRend = GetComponent<SpriteRenderer>();
        falling = false;
        fallen = false;
        spriteRend.color = new Color(0.53f,0.53f,0.53f);
    }

    //Tree tips over if collided with the Armadillo tag
    void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Armadillo" && !falling && !fallen){
            GetComponent<BoxCollider2D>().isTrigger = false;
            AudioSource.PlayClipAtPoint(treeFall, transform.position);
            falling = true;
            
        }
        if (other.tag == "Water"){
            if(!playSplash){
                playSplash = true;
                Instantiate(splashPrefab.GetComponent<ParticleSystem>(), transform.position + new Vector3(0, -2f, 0), Quaternion.identity);
                AudioSource.PlayClipAtPoint(splashSound, transform.position);
            }
            falling = false;
            fallen = true;
            GetComponent<BoxCollider2D>().isTrigger = false;
            GetComponent<Rigidbody2D>().isKinematic = false;


        }

    }

    // Update is called once per frame
    void Update() {
        //moves the tree
        if (falling == true && fallen == false){
            //rotate tree clockwise about the parent pivot point
            transform.parent.Rotate(0f, 0f, -fallSpeed, Space.World);
        }

    }
}
