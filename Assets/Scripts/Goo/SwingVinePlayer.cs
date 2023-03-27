using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingVinePlayer : MonoBehaviour
{
    public Rigidbody2D rb;
    private HingeJoint2D hj;

    public float pushForce;

    public Vector2 detachBoost;

    public bool attached = false;
    public Transform attachedTo;
    public GameObject disregard;

    static public SwingVinePlayer instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        rb = gameObject.GetComponent<Rigidbody2D>();
        hj = gameObject.GetComponent<HingeJoint2D>();

    }

    // Update is called once per frame
    void Update()
    {

        if (attached) { 
            CheckKeyboardInputs();
        }
    }

    private void CheckKeyboardInputs()
    {
        if (Input.GetKey("a"))
        {
            if (FindObjectOfType<PlayerManager>().facingRight){
                FindObjectOfType<PlayerManager>().flipY();
            }
            if (attached)
            {
                rb.AddRelativeForce(new Vector3(-1, 0, 0) * pushForce);
            }
        }
        if (Input.GetKey("d"))
        {
            if (!FindObjectOfType<PlayerManager>().facingRight){
                FindObjectOfType<PlayerManager>().flipY();
            }
            if (attached)
            {
                rb.AddRelativeForce(new Vector3(1, 0, 0) * pushForce);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Detach();
            rb.velocity = new Vector2(rb.velocity.x * detachBoost.x, rb.velocity.y * detachBoost.y);
        }
    }

    public void Attach(Rigidbody2D ropeBone)
    {
        ropeBone.gameObject.GetComponent<RopeSegment>().isPlayerAttached = true;
        hj.connectedBody = ropeBone;
        hj.enabled = true;
        attached = true;
        attachedTo = ropeBone.gameObject.transform.parent;
        for(int i = 0; i < 7; i++){
            Slide();
        }
        
    }

    public void Detach()
    {
        hj.connectedBody.gameObject.GetComponent<RopeSegment>().isPlayerAttached = false;
        hj.enabled = false;
        hj.connectedBody = null;
        attachedTo = null;
        StartCoroutine(AttachedNull());
    }

    IEnumerator AttachedNull()
    {
        yield return new WaitForSeconds(0.25f);
        attached = false;

    }

    public void Slide()
    {
        RopeSegment myConnection = hj.connectedBody.gameObject.GetComponent<RopeSegment>();
        GameObject newSeg = null;
        if (myConnection.connectedBelow != null) {
            newSeg = myConnection.connectedBelow;
        }

        if (newSeg != null)
        {
            transform.position = newSeg.transform.position;
            myConnection.isPlayerAttached = false;
            newSeg.GetComponent<RopeSegment>().isPlayerAttached = true;
            hj.connectedBody = newSeg.GetComponent<Rigidbody2D>();
        }

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        
        if (GetComponent<PlayerManager>().currentIndex == 1)
        {
            {
                if (!attached)
                {
                    if (col.gameObject.tag == "SwingyVine")
                    {
                        if (attachedTo != col.gameObject.transform.parent)
                        {
                            if (disregard == null || col.gameObject.transform.parent.gameObject != disregard)
                            {
                                Attach(col.gameObject.GetComponent<Rigidbody2D>());
                            }
                        }
                    }

                }

            }
        }
    }
}
    
    

    
        
    



