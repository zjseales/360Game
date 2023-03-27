using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame2 : MonoBehaviour
{
    public AudioClip fireHurts;

    public bool playing;

    void Start(){
        playing = false;
    }

    void OnTriggerEnter2D(Collider2D col){
        if (col.tag == "Monkey" || col.tag == "Snake" || col.tag == "Armadillo"){
            Boss4.PlayerHit();
        }
    }

    void OnTriggerStay2D(Collider2D col){
        if (col.tag == "Monkey" || col.tag == "Snake" || col.tag == "Armadillo"){
            if(playing){
                return;
            }
            playing = true;
            StartCoroutine(waitTimer());
            AudioSource.PlayClipAtPoint(fireHurts, col.gameObject.transform.position);
            Boss4.PlayerHit();
        }
    }

    IEnumerator waitTimer(){
        yield return new WaitForSeconds(1f);
        playing = false;
    }
}
