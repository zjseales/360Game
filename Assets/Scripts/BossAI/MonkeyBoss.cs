using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** MonkeyBoss.cs
* COSC 360 Game Assignment
* Team Goold
*
* Defines the attack and movement mechanics of the monkey boss.
*/

public class MonkeyBoss : MonoBehaviour {
	//The projectile array of prefabs for the waves of bananas
	[SerializeField] GameObject[] bullet1;
	//the projectile of individual bananas
	[SerializeField] GameObject bullet2;

	public AudioClip sound1;
	public AudioClip sound2;
	public AudioClip sound3;

	private bool canPlay = true;

	//the postions the boss will move to
	public GameObject phase2PositionA;
	public GameObject phase2PositionB;
	public GameObject phase3Position;

	public GameObject target;

	//the current phase position (null is initial phase 1)
	public GameObject currentPosition = null;

	// movement speed of the monkey boss
	public float speed = 10;
	//true if boss is moving
	public bool moving;

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
	bool second;
	bool patrolling = false;
	bool check;
	Animator MonkeyBossAnim;
	// Use this for initialization

	public GameObject Coin;

	void Start () {
		this.transform.GetChild(0).gameObject.SetActive(false);
		rigidBody = GetComponent<Rigidbody2D>();
		//initializes movement variables
		pathfinder = transform.GetComponent<AStarPathfinder>();
		moving = false;
		//projectile variables
		fireRate = 2f;
		nextFire = Time.time;
		BossLevel1.phase1 = true;
		BossLevel1.phase2 = false;
		BossLevel1.phase3 = false;
		MonkeyBossAnim = gameObject.GetComponent<Animator>();
		first = true;
		second = true;
		check = true;
	}

	// Update is called once per frame
	void Update () {
		if(!GameMaster.canMove){
			return;
		} else if (check){
			check = false;
			StartCoroutine(startBuffer());
		}
		if (BossLevel1.phase1 && canFire()){
			CheckIfTimeToFire1();
		} else if (BossLevel1.phase2 && canFire()){
			CheckIfTimeToFire2();
		}
		if (currentPosition == null){
			return;
		} else if (BossLevel1.phase2) {
			if (!patrolling)
			{
				speed = 6;

			}
			else
			{
				speed = 2;
			}
			MonkeyBossAnim.SetBool("MonkeyBossIdle", false);
			MonkeyBossAnim.SetBool("MonkeyBossClimbing", true);
			pathfinder.GoTowards(currentPosition, speed );
		} else if (BossLevel1.phase3){
			speed = 4;
			MonkeyBossAnim.SetBool("MonkeyBossIdle", true);
			MonkeyBossAnim.SetBool("MonkeyBossClimbing", false); 
			pathfinder.GoTowards(currentPosition, speed );
			//* Time.deltaTime
		}
	}
	//Waits 3 seconds before first wave of bananas in phase1
	IEnumerator startBuffer(){
		yield return new WaitForSeconds(2.5f);
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
		if (col.tag == "FallingTree" && BossLevel1.phase3 ){
			AudioSource.PlayClipAtPoint(sound3, transform.position);
			BossLevel1.bossHit();
			Coin.GetComponent<CircleCollider2D>().enabled = true;
			Coin.GetComponent<SpriteRenderer>().enabled = true;
			Coin.GetComponent<Rigidbody2D>().gravityScale = 1;
			CheckPointMaster.firstPosition = new Vector2(-7.48f,-5.37f);
			CheckPointMaster.lastCheckPointPos = CheckPointMaster.firstPosition;
			StartCoroutine(monkeyDeath());
			canPlay = false;
		}

		if (col.tag == "Armadillo" && !BossLevel1.phase3){
			BossLevel1.bossHit();
		if (BossLevel1.bossHealth == 2 && canPlay){
			AudioSource.PlayClipAtPoint(sound1, transform.position);
			canPlay = false;
		} else if (BossLevel1.bossHealth == 1 && canPlay){
			AudioSource.PlayClipAtPoint(sound2, transform.position);
			canPlay = false;
		}
		changePhase();
		}
		if (col.tag == "position1" ){
			patrolling = true;
			canPlay = true;
			BossLevel1.invincible = false;
			moving = false;
			currentPosition = phase2PositionB;
			BossLevel1.phase1 = false;
			BossLevel1.phase2 = true;
		}
		if (col.tag == "position2")
		{
			currentPosition = phase2PositionA;
		}
		if (col.tag == "position3")
		{
			BossLevel1.invincible = false;
			moving = false;
			BossLevel1.phase2 = false;
			BossLevel1.phase3 = true;
		}
	}

	//changes the position of the boss for the next phase of the fight
	void changePhase(){
		if (BossLevel1.bossHealth == 2){
			currentPosition = phase2PositionA;
			BossLevel1.phase1 = false;
			BossLevel1.phase2 = true;
		} else if (BossLevel1.bossHealth == 1){
			currentPosition = phase3Position;
			BossLevel1.phase2 = false;
			BossLevel1.phase3 = true;
			//starts the falling fruit
			this.transform.GetChild(0).gameObject.SetActive(true);
		} else if (BossLevel1.bossHealth <= 0){
			Debug.Log("endlevel");
			Coin.GetComponent<CircleCollider2D>().enabled = true;
			Coin.GetComponent<SpriteRenderer>().enabled = true;
			Coin.GetComponent<Rigidbody2D>().gravityScale = 1;
			StartCoroutine(monkeyDeath());
		}
		moving = true;
		return;
	}

	IEnumerator monkeyDeath(){
		yield return new WaitForSeconds(0.2f);
		this.gameObject.SetActive(false);
	}

	//fires the wave of bananas
	void CheckIfTimeToFire1(){
		if (Time.time > nextFire && target.activeInHierarchy){
			AudioSource.PlayClipAtPoint(attack[Random.Range(0, 3)], transform.position);
			for(int i = 0; i < 3; i++){
				Instantiate (bullet1[i], transform.position, Quaternion.identity);
			}
			nextFire = Time.time + fireRate * 3;
		}
	}
	// fires the individual bananas
	void CheckIfTimeToFire2() {
		if(second){
			StartCoroutine(secondBuffer());
		} else if (Time.time > nextFire && target.activeInHierarchy) {
			AudioSource.PlayClipAtPoint(attack[Random.Range(0, 3)], transform.position);
			Instantiate (bullet2, transform.position, Quaternion.identity);
			nextFire = Time.time + fireRate * 2;
		}
	}
	
	//does not fire immediately when phase 2 begins
	IEnumerator secondBuffer(){
		yield return new WaitForSeconds(2);
		second = false;
	}

}

