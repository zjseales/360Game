using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** FallingFruit.cs
  * COSC360 Game Assignment
  * Team Goold
  *
  * Controls the falling fruit mechanic in the monkey fight.
  */

public class FallingFruit : MonoBehaviour {

    //the fruit object
    [SerializeField] GameObject projectile;

    public float fallSpeed;
    private float fallRate;

    public Vector2 spawnLine;
    public Vector2 startPoint;

    public int max = 4;
    public static int current;

    Rigidbody2D rigidBody;

    // Start is called before the first frame update
    void Start() {
        rigidBody = projectile.GetComponent<Rigidbody2D>();

        spawnLine = new Vector2(26, 0);
        startPoint = new Vector2(-13f, 13);
        current = 0;
        fallRate = fallSpeed;

    }

    // Update is called once per frame
    void Update() {
        rigidBody = projectile.GetComponent<Rigidbody2D>();
        if (current != max && fallRate <= 0){
            //spawn a projectile at a random point on the line
            Instantiate (projectile, startPoint + new Vector2(Random.Range(0, spawnLine.magnitude), 0), Quaternion.identity);
            current++;
            fallRate = fallSpeed;
        }
        fallRate--;
    }

}
