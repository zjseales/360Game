using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/** Boss1Master.cs
  * COSC360 Game Assignment
  * Team Goold
  *
  * Manages the mechanics of the first boss.
  */

public class Level1Master : MonoBehaviour {

  // Start is called before the first frame update
  void Start() {
    GameMaster.numAnimals = 1;
    GameMaster.playerHealth = 3;

  }
}
