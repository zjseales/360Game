using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armadillo : MonoBehaviour
{
    AudioSource audio;
    bool currentlyPlaying;

    // Start is called before the first frame update
    void Start() {
     audio = GetComponent<AudioSource>();
     currentlyPlaying = false;
    }

    // Update is called once per frame
    void Update() {
        if(!canPlay()){
            audio.Stop();
            return;
        } else if (!currentlyPlaying) {
            currentlyPlaying = true;
            audio.Play();
        }
        
    }

    bool canPlay(){
        if(!GetComponentInParent<PlayerManager>().grounded()
        || Mathf.Abs(GetComponentInParent<Rigidbody2D>().velocity.x) < 0.5f){
            currentlyPlaying = false;
            return false;
        }
        if ((GetComponentInParent<PlayerManager>().grounded() || GetComponentInParent<Rigidbody2D>().velocity.y <= 0.01f)
        && Mathf.Abs(GetComponentInParent<Rigidbody2D>().velocity.x) >= 0.5f){
            return true;
        }
        return true;
    }
}
