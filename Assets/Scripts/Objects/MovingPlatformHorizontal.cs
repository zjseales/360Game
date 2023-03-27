using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformHorizontal : MonoBehaviour
{
    public bool facingRight = true;
    // Rate of the 'bob' movement
    public float bobRate;

    public Vector2 startVector;

    // Scale of the 'bob' movement
    public float bobScale;

    void Awake(){
        facingRight = true;
        startVector = new Vector2 (37.5f, -3.14f);

    }
    // Update is called once per frame
    void Update()
    {
        // Change in vertical distance 
        float dx = bobScale * Mathf.Sin(bobRate * Time.time);

        // Move the game object on the vertical axis
        transform.Translate(new Vector3(dx, 0, 0));

        if(dx >= 0 && !facingRight){
            flipY();
        } else if (dx < 0 && facingRight){
            flipY();
        }


    }

    /** Flips the bird (lol) about the Y - axis.
      * Direction depends on current rigidbody velocity vector.
      */
    void flipY(){
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        facingRight = !facingRight;
    }
}

