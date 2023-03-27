using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArmadilloCoin : MonoBehaviour
{

    public AudioClip coinSound;

    private bool playing = false;


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Goo")
        {
            GooMovement.goToCoin = false;
            GooMovement.hitCoin = true;
            if (!playing)
            {
                playing = true;
                AudioSource.PlayClipAtPoint(coinSound, transform.position);

            }
            GameMaster.numAnimals = 1;
            StartCoroutine("CoinAnimation");

        }


    }

    IEnumerator CoinAnimation()
    {
        gameObject.GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(0.6f);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;

        StartCoroutine("EndCutscene");
    }
    IEnumerator EndCutscene()
    {


        yield return new WaitForSeconds(4f);
        GameMaster.numAnimals = 1;
        //SceneManager.LoadScene("Level1_ArmadilloOnly");
        SceneManager.LoadScene("TutorialLevel");

    }

}
