using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public AudioClip squawk;
    public Vector3 startPosition;
    private bool canPlay = true;

    // Start is called before the first frame update
    void Start() {
        if(startPosition == null){
            Debug.Log("Must set a start position for moving platforms");
        }
        this.transform.position = startPosition;
    }

    void OnCollisionEnter2D(Collision2D other){
        if(canPlay){
            AudioSource.PlayClipAtPoint(squawk, this.transform.position);
            canPlay = false;
            StartCoroutine(soundReset());
        }
    }

    IEnumerator soundReset(){
        yield return new WaitForSeconds(1.28f);
        canPlay = true;
    }

}
