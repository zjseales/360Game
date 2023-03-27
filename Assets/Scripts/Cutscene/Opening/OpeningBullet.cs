using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningBullet : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    public float moveSpeed = 10f;
    public float spinSpeed = 5f;

    Rigidbody2D rb;
    GameObject target;
    GameObject ArmadilloBoss;
    Vector2 moveDirection;

    public GameObject particlePrefab;
    private ParticleSystem particleCurrent;

    public AudioClip sound1;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //the projectile's target (the player)
        target = GameObject.Find("Armadillo");
        ArmadilloBoss = GameObject.Find("ArmadilloBoss");
        //calculates vector direction to player
        moveDirection = (target.transform.position - transform.position).normalized * moveSpeed;
        //applies force in that direction, 
        rb.AddForce(new Vector2(moveDirection.x, moveDirection.y), ForceMode2D.Impulse);
        //Adds rotation to the banana
        rb.AddTorque(spinSpeed, ForceMode2D.Impulse);
        Destroy(gameObject, 3f);
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Armadillo")
        {
            particleCurrent = Instantiate(particlePrefab.GetComponent<ParticleSystem>(), transform.position, Quaternion.identity);
            particleCurrent.Play();
            AudioSource.PlayClipAtPoint(sound1, transform.position);
            Debug.Log("Hit!");
            Destroy(gameObject);
            OpeningScene.flyaway = true;
            target.GetComponent<SpriteRenderer>().enabled = false;
            target.GetComponent<CircleCollider2D>().enabled = false;
            
            ArmadilloBoss.GetComponent<SpriteRenderer>().enabled = true;
            StartCoroutine(waitForMovement());

        }

    }
    IEnumerator waitForMovement()
    {
        yield return new WaitForSeconds(2f);


    }
}
