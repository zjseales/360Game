using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    public CheckPointMaster cpm;

    private bool playing;

    public AudioClip bell;

    // Start is called before the first frame update
    void Start()
    {
        cpm = GameObject.FindGameObjectWithTag("CheckPointMaster").GetComponent<CheckPointMaster>();
        playing = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            CheckPointMaster.lastCheckPointPos = transform.position;

            if(!playing){
                playing = true;
                AudioSource.PlayClipAtPoint(bell, FindObjectOfType<PlayerManager>().transform.position);
                StartCoroutine(bellTimer());
            }
        }
    }

    IEnumerator bellTimer(){
        yield return new WaitForSeconds(3);
        playing = false;
    }

}
