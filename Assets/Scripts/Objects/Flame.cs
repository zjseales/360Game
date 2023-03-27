using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col){
        if (col.tag == "Player"){
            Boss3.playerKilled();
        }
    }
}
