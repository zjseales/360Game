using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningScene : MonoBehaviour
{
    //the projectile of individual bananas
    [SerializeField] GameObject bullet;

    public GameObject Position1;
    public GameObject Position2;
    public GameObject Position3;

    public GameObject currentPosition;

    private AStarPathfinder pathfinder = null;

    // movement speed
    public float speed;
    bool first = false;
    bool shoot = false;
    public static bool flyaway = false;

    public GameObject Speech1;
    public GameObject Speech2;
    public GameObject Speech3;


    public AudioClip sound1;

    // Start is called before the first frame update
    void Start()
    {
        first = false;
        shoot = false;
        flyaway = false;
        pathfinder = transform.GetComponent<AStarPathfinder>();
        StartCoroutine(startBuffer());

    }
    IEnumerator startBuffer()
    {
        yield return new WaitForSeconds(2);
        first = true;
        AudioSource.PlayClipAtPoint(sound1, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (first)
        {
            currentPosition = Position1;
            pathfinder.GoTowards(currentPosition, speed);

        }
        if (shoot)
        {
            shoot = false;
            StartCoroutine(SecondText());

        }
        if (flyaway)
        {
            currentPosition = Position2;
            pathfinder.GoTowards(currentPosition, speed);
        }
    }
    IEnumerator SecondText()
    {
        Speech1.GetComponent<SpriteRenderer>().enabled = false;
        Speech2.GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(2.5f);
        ShootAtArmadillo();// shoot at armadillo
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("position1"))
        {
            first = false;
            shoot = true;
            Position1.SetActive(false);
            //when armadillo is hit he turns into evil armadillo
            //then he rolls into a tree and a coin is left behind.
            //goo goes to the coin and then change scene to level1
        }
        if (col.CompareTag("position2"))
        {

            flyaway = false;


        }

    }

    void ShootAtArmadillo()
    {
        Instantiate(bullet, transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(sound1, transform.position);
        StartCoroutine(ThirdText());

    }

    IEnumerator ThirdText()
    {

        yield return new WaitForSeconds(1.7f);
        Speech2.GetComponent<SpriteRenderer>().enabled = false;
        Speech3.GetComponent<SpriteRenderer>().enabled = true;
        StartCoroutine(Wait());

    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2.5f);
        Speech3.GetComponent<SpriteRenderer>().enabled = false;
        GooMovement.canMove = true;
        Destroy(gameObject);

    }
}
