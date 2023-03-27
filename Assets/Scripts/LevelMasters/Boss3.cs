using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/** Boss3.cs
  * COSC360 Game Assignment
  * Team Goold
  *
  * Manages the first boss level, controls movement of the enemy,
  * attacks, projectiles, and player health.
  */

public class Boss3 : MonoBehaviour {

    static public Boss3 instance;

    // Boss Hit points
    public static int bossHealth = 3;
    // stops boss losing multiple hp with 1 hit
    public static bool invincible;

    public static bool phase3;
    public static bool phase2;
    public static bool phase1;

    public GameObject vine1;
    public GameObject vine2;

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
        bossHealth = 3;
        instance = this;
        phase1 = true;
        phase2 = false;
        phase3 = false;
        invincible = false;
        playerInvincible = false;

        player = FindObjectOfType<PlayerManager>().gameObject;
        playerHit[0] = FindObjectOfType<PlayerSound>().sound1;
        playerHit[1] = FindObjectOfType<PlayerSound>().sound2;
    }

    void Update(){
        if(player.GetComponent<PlayerManager>().attached && !(vine1.activeInHierarchy || vine2.activeInHierarchy)){
            player.GetComponent<SwingVinePlayer>().Detach();
        }
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
        //check if boss is dead
        if(bossHealth == 2){
            phase1 = false;
            phase2 = true;
            disappear = GameObject.FindGameObjectWithTag("destroyStuff");
            instance.particleCurr = Instantiate(instance.particles.GetComponent<ParticleSystem>(), instance.transform.position, Quaternion.identity);
            instance.particleCurr.Play();
            disappear.SetActive(false);

            instance.StartCoroutine("destroy");
        } else if (bossHealth == 1){
            playerInvincible = true;
            instance.StartCoroutine("EndPt1");
            return;
        }

    }

    IEnumerator destroy(){
        yield return new WaitForSeconds(13f);
        disappear.SetActive(true);
        phase1 = true;
        phase2 = false;
        phase3 = true;
    }

    //end of phase 1 boss
    IEnumerator EndPt1()
    {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene("FinalBossPt2");
    }
    
        
   



    public static void sprayBlood(){
		ParticleSystem particleCurrent = Instantiate(instance.blood.GetComponent<ParticleSystem>(), FindObjectOfType<Boss3>().transform.position, Quaternion.identity);
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
        
        AudioSource.PlayClipAtPoint(playerHit[Random.Range(0, 2)], player.transform.position);
        bossHealth = 3;
        phase3 = false;
        instance.StartCoroutine(instance.wait());
        
   }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(0.0f);
        GameMaster.reloadScene();
        GameMaster.playerHealth = 3;
    }


    IEnumerator playerTime(){
       yield return new WaitForSeconds(1);
       playerInvincible = false;
   }

}//end Boss3 class