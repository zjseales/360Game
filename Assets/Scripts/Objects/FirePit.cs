using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePit : MonoBehaviour
{
    void OnCollisionStay2D(Collision2D col){
        if(col.collider.tag == "Player"){
            Boss4.PlayerHit();
        }
    }
}
