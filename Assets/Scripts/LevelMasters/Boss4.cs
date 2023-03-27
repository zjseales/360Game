using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/** Boss4.cs
  * COSC360 Game Assignment
  * Team Goold
  *
  * Manages the final boss level, controls movement of the enemy,
  * attacks, projectiles, and player health.
  */

public class Boss4 : MonoBehaviour {

    static public Boss4 instance;

    // Boss Hit points
    public static int bossHealth = 3;
    // stops boss losing multiple hp with 1 hit
    public static bool invincible;

    public static GameObject disappear;
	public GameObject blood;
    public GameObject particles;
    public ParticleSystem particleCurr;

    public static AudioClip[] playerHit = new AudioClip[2];
    public static GameObject player;

    public static bool playerInvincible;

    void Start() {
        GameMaster.numAnimals = 3;
        GameMaster.playerHealth = 3;
        bossHealth = 1;
        instance = this;
        invincible = false;
        playerInvincible = false;

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
        if (bossHealth <= 0){
            playerInvincible = true;
            GameMaster.loadLevel("Endscene");
            return;
        }
    }


    public static void sprayBlood(){
		ParticleSystem particleCurrent = Instantiate(instance.blood.GetComponent<ParticleSystem>(), FindObjectOfType<Boss4>().transform.position, Quaternion.identity);
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
            playerKilled();
        }
   }//end PlayerHit method

   public static void playerKilled(){
        instance.StartCoroutine(instance.wait());
        
   }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(0.0f);
        AudioSource.PlayClipAtPoint(playerHit[Random.Range(0, 2)], player.transform.position);
        GameMaster.reloadScene();
    }

    IEnumerator playerTime(){
       yield return new WaitForSeconds(1);
       playerInvincible = false;
   }

}//end Boss3 class