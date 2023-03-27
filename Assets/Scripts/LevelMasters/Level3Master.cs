using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/** Level3Master.cs
  * COSC360 Game Assignment
  * Team Goold
  *
  * Manages the mechanics of the third level.
  */

public class Level3Master : MonoBehaviour
{
  public GameObject inst;

    // Start is called before the first frame update
    void Start()
    {
      GameMaster.numAnimals = 3;
      GameMaster.playerHealth = 3;
      GameMaster.canMove = false;
    }

    void Update(){
      if (inst.activeInHierarchy && (Input.GetKey(KeyCode.Space) || CheckPointMaster.lastCheckPointPos != CheckPointMaster.firstPosition)){
        GameMaster.canMove = true;
        GameMaster.canJump = false;
        inst.SetActive(false);
        StartCoroutine(jumpTimer());
      }
    }

    IEnumerator jumpTimer(){
        yield return new WaitForSeconds(0.5f);
        GameMaster.canJump = true;
    }
}