using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour {

	[SerializeField]
	float moveSpeed = 5f;

	float dirX , dirY;
	Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		dirX = Input.GetAxis ("Horizontal") * moveSpeed;
		dirY = Input.GetAxis ("Vertical") * moveSpeed;
	}

	void FixedUpdate()
	{
		rb.velocity = new Vector2 (dirX, dirY);
	}

}
