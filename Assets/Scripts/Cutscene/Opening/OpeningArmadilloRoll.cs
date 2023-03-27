using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningArmadilloRoll : MonoBehaviour
{
    public GameObject Coin;
    public AudioClip sound1;
    bool hasPlayed = false;

    private void Start()
    {
        hasPlayed = false;
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.tag == "detect")
        {
            if (!hasPlayed)
            {
                //effect of armadillo hitting tree?
                AudioSource.PlayClipAtPoint(sound1, transform.position);
                hasPlayed = true;
            }

            StartCoroutine(wait());

        }



    }
    IEnumerator wait()
    {

        yield return new WaitForSeconds(.9f);
        
        Coin.SetActive(true);
        GooMovement.goToCoin = true;

        Destroy(gameObject);
    }
}
