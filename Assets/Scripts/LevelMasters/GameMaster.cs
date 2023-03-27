using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/** GameMaster.cs
  * COSC 360 Assignment
  * Team Goold
  *
  * Stores the players current health and resets the scene when player hp becomes 0.
  * Also defines the player's available animal forms and controls music.
  */

public class GameMaster : MonoBehaviour {
    static public GameMaster instance;
    // Player HP
    public static int playerHealth;
    //Number of available animal forms
    public static float numAnimals;
    //the audio played on death
    public static AudioClip deathClip;
    //the music currently being played
    public static AudioSource currentMusic;

    public static bool canMove;

    public static bool canJump;

    public static bool bossPause;

    public AudioClip defeatBoss;

    public bool bossLevel;

    public bool first;

    public GameObject bossGuy;

    void Awake(){
        canMove = true;
    }

    void Start(){
        bossPause = true;
        currentMusic = GetComponent<AudioSource>();
        deathClip = transform.GetChild(0).gameObject.GetComponent<AudioSource>().clip;
        instance = this;
        canJump = true;
    }

    void Update(){
        if (!bossLevel){
            return;
        }
        if ((!canMove || PauseManager.showing) && Input.GetKey(KeyCode.Space)){
            first = false;
            bossPause = true;
            canMove = true;
            canJump = false;
            GameObject.FindGameObjectWithTag("instructions").SetActive(false);
            StartCoroutine(jumpTimer());
        } else {
            bossPause = false;
        }
        if (currentMusic.isPlaying && !bossGuy.activeInHierarchy && bossLevel){
            bossLevel = false;
            currentMusic.Stop();
            AudioSource.PlayClipAtPoint(defeatBoss, FindObjectOfType<PlayerManager>().transform.position);
            StartCoroutine(musicTimer());
        }
    }

    IEnumerator jumpTimer(){
        yield return new WaitForSeconds(0.5f);
        canJump = true;
    }

    IEnumerator musicTimer(){
        yield return new WaitForSeconds(2.2f);
        currentMusic.Play();
    }

    /** reloads scene if player is on zero health */
    public static void playerHit(){
        GameMaster.playerHealth--;

        if (GameMaster.playerHealth <= 0) {
            //reset scene
            playerHealth = 3;
            reloadScene();
        }
    }

    /** Method to reload scene after a death */
    public static void reloadScene(){
        //stops method being called mulitple times
        if (!currentMusic.isPlaying){
            return;
        }
        currentMusic.Stop();
        AudioSource.PlayClipAtPoint(deathClip, FindObjectOfType<PlayerManager>().transform.position);
        instance.StartCoroutine(instance.die());
        FindObjectOfType<PlayerManager>().gameObject.SetActive(false);
    }

    IEnumerator die(){
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    /**  Reloads scene if player falls off map. */
    public static void fallOffMap(){
        reloadScene();
    }

    /** Reloads scene if player drowns */
    public static void playerDrown(){
        reloadScene();
    }

    /** load the scene specified by the String argument.
      *@param levelName - the name of the scene being loaded */
    public static void loadLevel(string levelName){
        SceneManager.LoadScene(levelName);
    }

}
