using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** FinalBoss2.cs
  * COSC 360 Game Assignment
  * Team Goold
  *
  * Defines the attack and movement mechanics of the FinalBoss, phase1.
  */

public class FinalBoss2 : MonoBehaviour {

	public AudioClip sound1;

	//the current phase position (null is initial phase 1)
    public GameObject currentPosition;

    // movement speed
    public float speed;
	//true if boss is moving
	public bool moving;

	//the boss rigid body component
	private Rigidbody2D rigidBody;

	float fireRate;
	float nextFire;
	bool first;

	// Use this for initialization
	void Start () {
		this.transform.GetChild(0).gameObject.SetActive(true);
		rigidBody = GetComponent<Rigidbody2D>();
	}

    //Calls the boss hit method if the armadillo hits the monkey at a certain speed
    void OnTriggerEnter2D(Collider2D col){
        if (col.tag == "Armadillo" || col.tag == "Monkey" || col.tag == "Snake"){
			this.transform.GetChild(0).gameObject.SetActive(false);
            AudioSource.PlayClipAtPoint(sound1, transform.position);
            Boss4.bossHit();
            StartCoroutine(bossDeath());
        }
    }

	IEnumerator bossDeath(){
		yield return new WaitForSeconds(0.2f);
		this.gameObject.SetActive(false);
	}

}