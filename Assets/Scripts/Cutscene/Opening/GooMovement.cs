using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooMovement : MonoBehaviour
{
    public static bool canMove = false;
    public Rigidbody2D rigidBody;
    public float speed = 30;

    bool fallen = false;
    public static bool goToCoin = false;

    public static bool hitCoin = false;
    GameObject ArmadilloBoss;
    public GameObject ArmadilloBossRoll;

    public GameObject particlePrefab;
    private ParticleSystem particleCurrent;

    public GameObject Speech5;
    public GameObject Armadillo;

    public AudioClip sound1;
    bool hasBounced = false;

    Animator animator;
    // Start is called before the first frame update
    private void Start()
    {
        canMove = false;
        fallen = false;
        goToCoin = false;
        hitCoin = false;
        hasBounced = false;
    }
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        ArmadilloBoss = GameObject.Find("ArmadilloBoss");
        animator = GetComponent<Animator>();
        //ArmadilloBossRoll.GetComponent<Rigidbody2D>().AddForce(new Vector2(15, 0));
    }

    // Update is called once per frame
    void Update()
    {

        if (canMove && !fallen)
        {

            rigidBody.AddForce(new Vector2(2000, 500));
            animator.SetTrigger("jump");

            canMove = false;
            StartCoroutine(waitForLanding());
        }

        if (hitCoin)
        {
            rigidBody.velocity = Vector3.zero;
            // particle effect and swap to armadillo
            particleCurrent = Instantiate(particlePrefab.GetComponent<ParticleSystem>(), transform.position, Quaternion.identity);
            particleCurrent.Play();
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            Armadillo.GetComponent<SpriteRenderer>().enabled = true;
            Speech5.GetComponent<SpriteRenderer>().enabled = true;
            hitCoin = false;
        }


    }

    void FixedUpdate()
    {
        if (goToCoin)
        {
            rigidBody.AddForce(new Vector2(80, 0));

        }
    }

    IEnumerator waitForLanding()
    {
        yield return new WaitForSeconds(2.4f);
        Destroy(ArmadilloBoss);
        fallen = true;
        canMove = true;
        ArmadilloBossRoll.SetActive(true);
        ArmadilloBossRoll.GetComponent<Rigidbody2D>().AddForce(new Vector2(600, 0));


    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.tag == "Armadillo")
        {
            animator.SetTrigger("hasLanded");

            if (!hasBounced)
            {
                AudioSource.PlayClipAtPoint(sound1, transform.position);
                hasBounced = true;
            }
                       

            Debug.Log("hitArmadilloBoss");
        }


        if (col.collider.tag == "detect")
        {
            animator.SetTrigger("isFalling");

        }
    }

   
}