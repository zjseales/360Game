using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GooMovementEnding : MonoBehaviour
{
    public Rigidbody2D rigidBody;

    public static bool first = true;
    public static bool second = false;
    public static bool third = false;


    bool facingRight = false;
    bool hasLanded = false;

    public GameObject Speech1;
    public GameObject Mushroom;
    public AudioClip mushroomLaunch;

    Animator mushroomAnim;
    // Start is called before the first frame update

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        mushroomAnim = Mushroom.GetComponent<Animator>();
    }
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        mushroomAnim = Mushroom.GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (first)
        {
            rigidBody.AddForce(new Vector2(-80, 0));
        }
        else if (second)
        {
            AudioSource.PlayClipAtPoint(mushroomLaunch, transform.position);
            mushroomAnim.SetTrigger("MushroomLaunch");

            rigidBody.AddForce(new Vector2(-200, 6250));
            second = false;
        } else if (hasLanded)
        {

            StartCoroutine(Endscene());
        }


    }
    IEnumerator Endscene()
    {
        yield return new WaitForSeconds(3.5f);
        SceneManager.LoadScene("EndScreen");
    }

        void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.tag == "position1")
        {
            first = false;
            second = true;
        } else if (col.collider.tag == "Platform" && !hasLanded)
        {
            hasLanded = true;
            flipY();
            Speech1.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    void flipY()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        facingRight = !facingRight;
    }
}
