using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/** BossLevel1.cs
  * COSC360 Game Assignment
  * Team Goold
  *
  * Manages the first boss level, controls movement of the enemy,
  * attacks, projectiles, and player health.
  */

public class BossLevel1 : MonoBehaviour {

    static public BossLevel1 instance;

    // Boss Hit points
    public static int bossHealth = 3;
    // stops boss losing multiple hp with 1 hit
    public static bool invincible;

    public static bool phase3;
    public static bool phase2;
    public static bool phase1;

	public GameObject blood;

    public static AudioClip[] playerHit = new AudioClip[2];
    public static GameObject player;

    public static bool playerInvincible;

    void Start() {
        GameMaster.playerHealth = 3;
        bossHealth = 3;
        instance = this;
        phase1 = true;
        phase2 = false;
        phase3 = false;
        invincible = false;
        playerInvincible = false;
        GameMaster.canMove = false;

        player = FindObjectOfType<PlayerManager>().gameObject;
        playerHit[0] = FindObjectOfType<PlayerSound>().sound1;
        playerHit[1] = FindObjectOfType<PlayerSound>().sound2;

    }
    
    // Method to call when enemy is hit
    public static void bossHit() {
        if (invincible){
            return;
        }
        sprayBlood();
        // boss loses a hit point
        bossHealth--;

        invincible = true;
        Debug.Log("Boss Health " + bossHealth);
        
    }

   

    public static void sprayBlood(){
		ParticleSystem particleCurrent = Instantiate(instance.blood.GetComponent<ParticleSystem>(), FindObjectOfType<MonkeyBoss>().transform.position, Quaternion.identity);
        particleCurrent.Play();
	}


    // Method to call when player is hit
    public static void PlayerHit() {
        if (playerInvincible){
            return;
        }
        playerInvincible = true;
        instance.StartCoroutine(instance.playerTime());
        GameMaster.playerHealth--;
        // Reduce player's lives
        if(GameMaster.playerHealth > 0) {
            AudioSource.PlayClipAtPoint(playerHit[Random.Range(0, 2)], player.transform.position);
        } else {
           bossHealth = 3;
           phase3 = false;
           instance.StartCoroutine(instance.wait());
           
        }
   }//end PlayerHit method

   IEnumerator playerTime(){
       yield return new WaitForSeconds(1);
       playerInvincible = false;
   }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(0.0f);
        GameMaster.reloadScene();
        GameMaster.playerHealth = 3;
    }

}//end BossLevel1 class