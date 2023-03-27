using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Coin : MonoBehaviour
{
    public Rigidbody2D rb;

    public AudioClip coinSound;

    private bool playing;


        private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<CircleCollider2D>().enabled = false;

        playing = false;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "CoinLanding")
        {
            //Turn gravity off so coin stops moving.
            rb.gravityScale = 0;
            rb.velocity = Vector3.zero;
        }
        if (other.tag == "Armadillo")
        {
            if(!playing){
                playing = true;
                AudioSource.PlayClipAtPoint(coinSound, FindObjectOfType<PlayerManager>().transform.position);

            }
            GameMaster.numAnimals = 2;
            StartCoroutine("EndBossLevel");
        } 
    }

        IEnumerator EndBossLevel()
        {
        //Animation for coin disappearing?
            gameObject.GetComponent<SpriteRenderer>().enabled = false;

        yield return new WaitForSeconds(2f);
        GameMaster.numAnimals = 2;
        SceneManager.LoadScene("Level2_ArmadilloMonkey");
        
        
        }
    }

