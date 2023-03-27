using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBoss : MonoBehaviour
{
    public GameObject Position1;
    public GameObject Position2;
    public GameObject Position3;

    
    public GameObject Boulder2;
    public GameObject Boulder3;

    

    public AudioClip snakeSound;

    public GameObject target;
    // movement speed of the monkey boss
    public float speed;

    private bool playSound;

    private AStarPathfinder pathfinder = null;


    //the boss rigid body component
    private Rigidbody2D rigidBody;


    Animator SnakeBossAnim;

    public bool first = true;
    public bool second = false;
    public bool third = false;

    bool facingRight = false;

    public GameObject Coin;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        pathfinder = transform.GetComponent<AStarPathfinder>();
        playSound = true;
        Boulder2.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        Boulder3.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        GameMaster.canMove = false;

    }

    //stops sound being played multiple times
    IEnumerator soundTimer(){
        yield return new WaitForSeconds(3);
        playSound = true;
    }

    void OnTriggerEnter2D(Collider2D col)
    {

        if (col.gameObject.CompareTag("Boulder"))
        {
            if(playSound){
                playSound = false;
                AudioSource.PlayClipAtPoint(snakeSound, transform.position);
                StartCoroutine(soundTimer());
            }
            if (first)
            {
                target = Position2;
                first = false;
                second = true;
                flipY();

            } else if (second)
            {
                flipY();

                
                target = Position3;
                second = false;
                third = true;
                
            }
            else if (third)
            {

                
                third = false;

                Coin.GetComponent<CircleCollider2D>().enabled = true;
                Coin.GetComponent<SpriteRenderer>().enabled = true;

                CheckPointMaster.firstPosition = new Vector2(-7.71f,-5.29f);
                CheckPointMaster.lastCheckPointPos = CheckPointMaster.firstPosition;

                this.gameObject.SetActive(false);
                
               

            }
        }
        if (col.gameObject.CompareTag("position2"))
        {
            Boulder2.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

        }
        if (col.gameObject.CompareTag("position3"))
        {
            Boulder3.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (second)
        {
           
            if (transform.position.x > 12)
            {
                if (facingRight)
                {
                    flipY();
                }
                transform.eulerAngles = new Vector3(0, 0, -90);

            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            


            speed = 6;
            pathfinder.GoTowards(target, speed);
        }if (third)
        {
            if (transform.position.x > 12 && transform.position.y < 8)
            {
                if (facingRight)
                {
                    flipY();
                }
                transform.eulerAngles = new Vector3(0, 0, -90);

            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            if (transform.position.x < -10.5)
            {
                if (!facingRight)
                {
                    flipY();
                }
            }
                speed = 6;
            pathfinder.GoTowards(target, speed);
        }
        
    }

    /** Flips the player about the Y - axis.
     * Direction depends on user input.
     */
    void flipY()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        facingRight = !facingRight;
    }

}

