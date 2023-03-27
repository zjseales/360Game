using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreenControl : MonoBehaviour
{
    void Update(){
        if (Input.GetKey(KeyCode.S)){
            GameMaster.loadLevel("MainMenu");
        }
    }

}
