using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** FallingStuff.cs
  * COSC360 Game Assignment
  * Team Goold
  *
  * Controls the falling fruit mechanic in the monkey fight.
  */

public class FallingStuff : MonoBehaviour {

    //the fruit object
    [SerializeField] GameObject projectile;

    public float fallSpeed;

    public Vector2[] spawnLines;
    public Vector2[] startPoints;

    public int prevIndex;
    public int currentIndex;

    public bool active;

    private bool check1;
    private bool check2;

    public AudioClip[] growling = new AudioClip[2];

    private int rate;

    Rigidbody2D rigidBody;

    void Awake(){
        spawnLines = new Vector2[4];

        for(int i = 0; i < 4; i++){ 
            spawnLines[i] = new Vector2(6, 0);
        }

    }

    // Start is called before the first frame update
    void Start() {
        rigidBody = projectile.GetComponent<Rigidbody2D>();

        startPoints = new Vector2[4];

        startPoints[0] = new Vector2(-13, 13);
        for(int i = 0; i < 3; i++){
            startPoints[i + 1] = startPoints[i] + spawnLines[i];
        }
        rate = 0;
        active = false;
        check1 = false;
        check2 = false;
        StartCoroutine(buffering());

    }
    //Used to choose the new spawn section and activate
    IEnumerator buffering(){
        yield return new WaitForSeconds(4.2f);
        if(GameMaster.playerHealth != 0){
                
            if((FindObjectOfType<SwingVinePlayer>().attached)){
                while (currentIndex == prevIndex){
                    currentIndex = Random.Range(0, 2);
                }
            } else {
                while (currentIndex == prevIndex){
                    currentIndex = Random.Range(0, 4);
                }
            }
            AudioSource.PlayClipAtPoint(growling[Random.Range(0, 2)], transform.parent.position);
            prevIndex = currentIndex;
            check1 = true;
            check2 = false;
            active = true;
        }
    }
    //used to stop the spawning after a time
    IEnumerator buffer2(){
        yield return new WaitForSeconds(4.2f);
        active = false;
    }

    // Update is called once per frame
    void Update() {
        rate++;
        rigidBody = projectile.GetComponent<Rigidbody2D>();
        if (active){
            //ensures only 1 timer is started
            if (!check2){
                check2 = true;
                StartCoroutine(buffer2());
            }
            //spawn a projectile at a random point on the line
            if(rate >= 4){
                rate = 0;
                Instantiate (projectile, startPoints[currentIndex] + new Vector2(Random.Range(0, spawnLines[currentIndex].magnitude), 0), Quaternion.identity);
            }
        } else if (check1){
            check1 = false;
            active = false;
            StartCoroutine(buffering());
        }
    }

}
