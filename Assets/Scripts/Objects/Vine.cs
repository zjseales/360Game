using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Vine.cs
  * COSC360 Game Assignment
  * Team Goold
  *
  * Defines the vine game objects and assigns a mesh 
  * with size equal to the collider component.
  */

[RequireComponent(typeof(MeshRenderer))]

public class Vine : MonoBehaviour {

    //all components instantiated
    private MeshRenderer mr;

    // Start is called before the first frame update
    void Start() {
        //Retrieve MeshRenderer and make green,
        mr = GetComponent<MeshRenderer>();
        mr.material.color = new Color(0f, 1f, 0f);       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
