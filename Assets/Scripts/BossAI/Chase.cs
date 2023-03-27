/*
	Created by: Lech Szymanski
				lechszym@cs.otago.ac.nz
				Dec 29, 2015			
*/

using UnityEngine;
using System.Collections;

/* This is an example script using A* pathfinding to chase a
 * target game object*/

public class Chase : MonoBehaviour {

	// Target of the chase
	// (initialise via the Inspector Panel)
	public GameObject target = null;
	public GameObject target2 = null;
	public float swappingTime = 10f;
	bool swap = true;
	
	// Chaser's speed
	// (initialise via the Inspector Panel)
	public float speed;

	// Chasing game object must have a AStarPathfinder component - 
	// this is a reference to that component, which will get initialised
	// in the Start() method
	private AStarPathfinder pathfinder = null; 

	// Use this for initialization
	void Start () {
		//Get the reference to object's AStarPathfinder component
		pathfinder = transform.GetComponent<AStarPathfinder> ();
		
	}

	// Update is called once per frame
	void Update() {
		if (pathfinder != null) {
			if(swap){ 
			//Travel towards the target object at certain speed.
			pathfinder.GoTowards(target, speed);
				if (IsInDistance(gameObject.transform ,target.transform, 1)){
					swap = false;
				}
		} else
		{
			//Travel towards the target object at certain speed.
			pathfinder.GoTowards(target2, speed);
				swap =true;

		}
	}
	}
	bool IsInDistance(Transform a, Transform b, float maxDistance)
	{
		return Vector3.Distance(a.position, b.position) <= maxDistance;
	}

}
