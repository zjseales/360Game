using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SnakeCoin : MonoBehaviour
{
    public Rigidbody2D rb;

    private bool playing;

    public AudioClip coinSound;


    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<CircleCollider2D>().enabled = false;

        playing = false;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.tag == "Armadillo")
        {
            if(!playing){
                playing = true;
                AudioSource.PlayClipAtPoint(coinSound, FindObjectOfType<PlayerManager>().transform.position);
            }
            GameMaster.numAnimals = 3;
            StartCoroutine("CoinAnimation");
            
        }
    }

    IEnumerator CoinAnimation()
    {
        gameObject.GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(0.6f);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        StartCoroutine("EndBossLevel");
    }
    IEnumerator EndBossLevel()
    {
        
        yield return new WaitForSeconds(2f);
        
        GameMaster.numAnimals = 2;
        SceneManager.LoadScene("Level3_AllAnimals");


    }
}

