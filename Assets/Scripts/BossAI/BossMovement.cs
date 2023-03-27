using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{

    public GameObject target = null;
    public GameObject target2 = null;

    public GameObject currentTarget = null;

    // Chaser's speed
    // (initialise via the Inspector Panel)
    public float speed;

    // Chasing game object must have a AStarPathfinder component - 
    // this is a reference to that component, which will get initialised
    // in the Start() method
    private AStarPathfinder pathfinder = null;


    // Start is called before the first frame update
    void Start()
    {
        pathfinder = transform.GetComponent<AStarPathfinder>();
        currentTarget = target;
    }

    // Update is called once per frame
    void Update()
    {
        pathfinder.GoTowards(currentTarget, speed);
    }

    public void SwapPlace()
    {
        
        if(currentTarget == target)
        {
            
            currentTarget = target2;
        }
        else
        {
            
            currentTarget = target;
        }

    }
}
