using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/** Level2Master.cs
  * COSC360 Game Assignment
  * Team Goold
  *
  * Manages the mechanics of the second level (armadillo and monkey).
  */

public class Level2Master : MonoBehaviour
{
  public GameObject inst;

    // Start is called before the first frame update
    void Start()
    {
      GameMaster.numAnimals = 2;
      GameMaster.playerHealth = 3;
    }

    void Update(){
      if (CheckPointMaster.lastCheckPointPos != CheckPointMaster.firstPosition && inst.activeInHierarchy){
        inst.SetActive(false);
      }
      if (inst.activeInHierarchy){
        GameMaster.canMove = false;
        GameMaster.canJump = false;
        if (Input.GetKey(KeyCode.Space)){
        GameMaster.canMove = true;
        inst.SetActive(false);
        StartCoroutine(jumpTimer());
      }
      }
    }

    IEnumerator jumpTimer(){
        yield return new WaitForSeconds(0.5f);
        GameMaster.canJump = true;
    }
}