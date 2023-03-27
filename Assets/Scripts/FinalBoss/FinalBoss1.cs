using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** FinalBoss1.cs
  * COSC 360 Game Assignment
  * Team Goold
  *
  * Defines the attack and movement mechanics of the FinalBoss, phase1.
  */

public class FinalBoss1 : MonoBehaviour {
	//The projectile array of prefabs for the waves of bananas
	[SerializeField] GameObject[] bullet1;
	//the projectile of individual bananas
	[SerializeField] GameObject bullet2;

	public AudioClip sound1;
	public AudioClip sound2;
	public AudioClip sound3;

	private bool canPlay = true;

	//the 2 postions the boss will move to
	public GameObject phase2Position;
    public GameObject phase3Position;

    public GameObject phase1move1;
    public GameObject phase1move2;

	public GameObject target;

	//the current phase position (null is initial phase 1)
    public GameObject currentPosition;

    // movement speed
    public float speed;
	//true if boss is moving
	public bool moving;

	private bool one;

    public float numAttacks = 5;

	public AudioClip[] attack = new AudioClip[3];

    // Chasing game object must have a AStarPathfinder component - 
    // this is a reference to that component, which will get initialised
    // in the Start() method
    private AStarPathfinder pathfinder = null;

	//the boss rigid body component
	private Rigidbody2D rigidBody;

	float fireRate;
	float nextFire;
	bool first;
	private int playTrack;

	// Use this for initialization
	void Start () {
		one = true;
		this.transform.GetChild(0).gameObject.SetActive(false);
		rigidBody = GetComponent<Rigidbody2D>();
		//initializes movement variables
		pathfinder = transform.GetComponent<AStarPathfinder>();
		speed = 200;
		moving = false;
		//projectile variables
		fireRate = 2f;
		nextFire = Time.time;
		
		Boss3.phase1 = true;
		Boss3.phase2 = false;
		Boss3.phase3 = false;
        currentPosition = phase1move1;

		first = true;

		playTrack = 0;
		GameMaster.canMove = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!GameMaster.canMove){
			return;
		} else if (first){
			StartCoroutine(startBuffer());
		}
		if (Boss3.phase1 || Boss3.phase3){
            pathfinder.GoTowards(currentPosition, speed * Time.deltaTime);
            if(canFire()){
                CheckIfTimeToFire2();
            }
			if(currentPosition == phase2Position){
				currentPosition = phase1move1;
			}
		} else if (Boss3.phase2 && canFire()){
			CheckIfTimeToFire1();
		}
		if (currentPosition == null){
			return;
		} else if (currentPosition == phase2Position) {
			pathfinder.GoTowards(currentPosition, speed);
		}
	}

	//Waits 3 seconds before first wave of bananas in phase1
	IEnumerator startBuffer(){
		yield return new WaitForSeconds(3);
		first = false;
	}

	//Does not fire at player if they are within a certain distance
	public bool canFire(){
		float x;
		float y;
		Vector2 distance;
		x = (target.transform.position - transform.position).x;
		y = (target.transform.position - transform.position).y;
		distance = new Vector2(x, y);

		if (distance.magnitude <= 7){
			return false;
		} else if (!first) {
			return true;
		}
		return false;
	}

    //Calls the boss hit method if the armadillo hits the monkey at a certain speed
    void OnTriggerEnter2D(Collider2D col){
        if (col.tag == "Armadillo" || col.tag == "Monkey" || col.tag == "Snake"){
            Boss3.bossHit();
			if (Boss3.bossHealth == 2 && canPlay){
				AudioSource.PlayClipAtPoint(sound1, transform.position);
				canPlay = false;
			} else if (Boss3.bossHealth == 1 && canPlay){
				AudioSource.PlayClipAtPoint(sound2, transform.position);
				canPlay = false;
			}
			changePhase();
        }
        //phase 1 movement
        if (Boss3.phase1 && col.tag == "position2" && currentPosition == phase1move1){
            currentPosition = phase1move2;
        } else if (Boss3.phase1 && col.tag == "position3" && currentPosition == phase1move2){
            currentPosition = phase1move1;
        }
		if (col.tag == "position1"){
			canPlay = true;
			Boss3.invincible = false;
			moving = false;
		}
    }

	//changes the position of the boss for the next phase of the fight
	void changePhase(){
		if (Boss3.bossHealth == 2){
			currentPosition = phase2Position;
			//StartCoroutine(flameWave());
		} else if (Boss3.bossHealth == 1){
			currentPosition = phase3Position;
			Boss3.phase2 = false;
			Boss3.phase3 = true;
		} else if (Boss3.bossHealth <= 0){
			StartCoroutine(bossDeath());
		}
		moving = true;
		return;
	}


	IEnumerator bossDeath(){
		yield return new WaitForSeconds(0.2f);
		this.gameObject.SetActive(false);
	}

	//fires the wave of bananas
	void CheckIfTimeToFire1(){
		if(one){
			StartCoroutine(oneDelay());
		}
		if (Time.time > nextFire && target.activeInHierarchy){
			if (playTrack == 0){
				AudioSource.PlayClipAtPoint(attack[Random.Range(0, 3)], transform.position);
				playTrack = 1;
			} else {
				playTrack = 0;
			}
			
			for(int i = 0; i < 5; i++){
				Instantiate (bullet1[i], transform.position, Quaternion.identity);
			}
			nextFire = Time.time + fireRate * 0.6f;
		}
	}

	IEnumerator oneDelay(){
		yield return new WaitForSeconds(1);
		one = false;
	}

	// fires the individual bananas
	void CheckIfTimeToFire2() {
		if (Time.time > nextFire && target.activeInHierarchy) {
			if (playTrack == 0){
				AudioSource.PlayClipAtPoint(attack[Random.Range(0, 3)], transform.position);
				playTrack = 1;
			} else {
				playTrack = 0;
			}
			Instantiate (bullet2, transform.position, Quaternion.identity);
			nextFire = Time.time + fireRate;
		}
	}

}