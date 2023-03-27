using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    //Follows the player
    private GameObject player;
    //Player position
    public float xMin;
    public float xMax;
    public float yMin;
    public float yMax;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float x;
        float y;
        x = Mathf.Clamp(player.transform.position.x, xMin, xMax);
        y = Mathf.Clamp(player.transform.position.y, yMin, yMax);

        gameObject.transform.position = new Vector3(x, y, gameObject.transform.position.z);

    }
}
