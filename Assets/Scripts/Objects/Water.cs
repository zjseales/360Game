using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Water.cs
  * COSC360 Game Assignment
  * Team Goold
  *
  * Defines the mechanics of the water prefab.
  */

public class Water : MonoBehaviour {

    //blue mesh, Used while testing
    private MeshRenderer mr;

    // Start is called before the first frame update
    void Start() {
        mr = GetComponent<MeshRenderer>();
        mr.material.color = new Color(0.6784f, 0.8471f, 0.902f, 180);
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
