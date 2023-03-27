using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** The struct of values that are unique for each animal's movement.
  */
struct Animal {
    public GameObject current;
    public string name;
    public float speed;
    public float jumpPower;
    public float maxSpeed;
    public Vector2 groundedRay1;
    public Vector2 groundedRay2;
}

/** PlayerManager.cs
  * COSC360 Game Assignment
  * Team Goold
  *
  * Controls movement of a central object while switching between 
  * an array of child prefab GameObjects.
  *
  * Index key : 0 = Armadillo
  *             1 = Monkey
  *             2 = Snake
  */

public class PlayerManager : MonoBehaviour {

    //The common rigidbody component of the parent player
    public Rigidbody2D rigidBody;
    //The deceleration of the player
    public float slowSpeed = 10;
    //Used to prevent multiple jump forces within a short time
    public int jumpCurrentWait = 0;
    public int jumpResetTime = 45;
    //current speed
    public float currentVelocity;
    //used for monkey swinging
    public bool attached = false;
    public Transform attachedTo;
    public GameObject disregard;
    public GameObject particlePrefab;
    public GameObject splashPrefab;
    private bool changing = false;

    //The array of Animals and their unique values
    private Animal[] animal = new Animal[3]; 
    // Index of the current animal on screen
    public int currentIndex = 0;
    //the direction the character is facing
    public bool facingRight = true;
    //the layer mask of the ground (resets jump and aligns snake and monkey)
    private int groundLayer = 1 << 9;
    //hinge joint used for swinging
    private HingeJoint2D hj;
    private bool atRamp = false;
    private bool playSound;
    private ParticleSystem particleCurrent;

    public float maxSpin = 1500;
    public float spinSpeed = 8;

    public AudioClip jumpSound;
    public AudioClip rampSound;
    public AudioClip drownSound;
    public AudioClip collideSound;
    public AudioClip monkeySound;
    public AudioClip snakeSound;
    public AudioClip fallSound;

    private bool canPlay = true;
    private bool playDrown;

    Animator MonkeyAnim;
    public GameObject Monkey;
    Animator SnakeAnim;
    public GameObject Snake;


    /** Instantiates and initializes array of animal objects.
      */
    void Awake(){
        rigidBody = GetComponent<Rigidbody2D>();
        hj = gameObject.GetComponent<HingeJoint2D>();
        setUpStructArray();
        playDrown = true;
    }
 
    /** Sets up player as an armadillo
    */
    void Start(){
        changeForm(0);
        playSound = true;
    }

    /** Sets up the array of structs containing unique values for each animal.
      */
    private void setUpStructArray(){
        //Retrieve the child game objects
        for(int i = 0; i < 3; i++){
            animal[i].current = this.transform.GetChild(i).gameObject;
            animal[i].current.SetActive(false);
        }

        //set up armadillo values
        animal[0].name = "Armadillo";
        animal[0].speed = 11;
        animal[0].maxSpeed = 13;
        animal[0].jumpPower = 6.5f;
        animal[0].groundedRay1 = new Vector2(-0.50f, -0.53f);
        animal[0].groundedRay2 = new Vector2(0.50f, -0.53f);

        //set up monkey values
        animal[1].name = "Monkey";
        animal[1].speed = 8;
        animal[1].maxSpeed = 7;
        animal[1].jumpPower = 7;
        animal[1].groundedRay1 = new Vector2(-0.5f, -0.57f);
        animal[1].groundedRay2 = new Vector2(0.5f, -0.57f);

        //set up snake values
        animal[2].name = "Snake";
        animal[2].speed = 7;
        animal[2].maxSpeed = 6;
        animal[2].jumpPower = 6;
        animal[2].groundedRay1 = new Vector2(-0.6f, -0.8f);
        animal[2].groundedRay2 = new Vector2(0.6f, -0.8f);
    }

    /** Changes the players physical form dependent on the index argument.
      * turns off current (if any), reassigns the global variables, and turns it on.
      * Z-axis rotation is frozen for monkey and snake.
      * @param index - the animal index being changed to.
      */
    void changeForm(int index){
        animal[currentIndex].current.SetActive(false);
        currentIndex = index;
        animal[index].current.SetActive(true);
        if(!changing){
            changing = true;
            particleCurrent = Instantiate(particlePrefab.GetComponent<ParticleSystem>(), transform.position, Quaternion.identity);
            particleCurrent.Play();
        }
        StartCoroutine(changeTime());
        //Sets the max speed and acceleration of the current form
        //resets rotation, and freezes z rotation - for the monkey and snake
        if(currentIndex == 0){
            rigidBody.constraints = RigidbodyConstraints2D.None;
        } else if (index == 1){
            if(canPlay){
                canPlay = false;
                AudioSource.PlayClipAtPoint(monkeySound, transform.position);
                StartCoroutine(soundTime());
            }
            rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            Monkey = gameObject.transform.Find("Monkey").gameObject; 
            MonkeyAnim = Monkey.GetComponent<Animator>();
            MonkeyAnim.SetBool("MonkeyWalking", true);
            MonkeyAnim.SetBool("MonkeyClimbing", false);
        } else {
            if (canPlay){
                canPlay = false;
                AudioSource.PlayClipAtPoint(snakeSound, transform.position);
                StartCoroutine(soundTime());
            }
            rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            Snake = gameObject.transform.Find("Snake").gameObject;
            SnakeAnim = Monkey.GetComponent<Animator>();
            SnakeAnim.SetBool("SnakeWalking", true);
            
        }
    }

    IEnumerator soundTime(){
        yield return new WaitForSeconds(1f);
        canPlay = true;
    }
    IEnumerator changeTime(){
        yield return new WaitForSeconds(0.3f);
        changing = false;
    }

    /** Returns true if the player is in contact with the ground.
      * Uses Ray casts to detect collisions below the player.
      */
    public bool grounded(){
        Vector2[] rays = new Vector2[2];
        int checks = 0;
        Vector2 centre = GetComponent<BoxCollider2D>().bounds.center;
        rays[0] = animal[currentIndex].groundedRay1;
        rays[1] = animal[currentIndex].groundedRay2;
        //Draws the rays from the centre of the rigidbody
        for (int i = 0; i < 2; i++){
            RaycastHit2D hit = Physics2D.Raycast(centre, rays[i], rays[i].magnitude, groundLayer);
            Color rayColor;
            //display green ray if player is grounded, else red
            if(hit.collider != null){
               rayColor = Color.green;
            } else {
                rayColor = Color.red;
            }
            Debug.DrawRay(centre, rays[i], rayColor);
            if (rayColor == Color.red){
                checks++;
            }
        }
        if(checks > 1){
            return false;
        }
        return true;
    }

    /** Flips the player about the Y - axis.
      * Direction depends on user input.
      */
    public void flipY(){
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        facingRight = !facingRight;
    }

    /** Controls rotation of the snake and monkey.
      * Creates raycasts around the bottom half of the player 
      * and rotates to match the surface of the ground.
    */
    void rotateWithGround(){
        Vector2[] rays = new Vector2[3];
        Vector2[] hitPoints = new Vector2[3];

        rays[0] = new Vector2(-0.8f, -1.75f);
        rays[1] = new Vector2(0.8f, -1.75f);
        rays[2] = new Vector2(0, -1.75f);
        Vector2 centre = GetComponent<BoxCollider2D>().bounds.center;

        for(int i = 0; i < 3; i++){
            RaycastHit2D hit = Physics2D.Raycast(centre, rays[i], rays[i].magnitude, groundLayer);
            if (hit.collider == null){
                hitPoints[i] = Vector2.zero;
            } else {
                hitPoints[i] = hit.point;
            }
            Debug.DrawRay(centre, rays[i]);
        }

        Vector2 distance = Vector2.zero;
        if(hitPoints[2] == Vector2.zero){
            return;
        } else if (hitPoints[0] != Vector2.zero && hitPoints[1] != Vector2.zero){
            distance = hitPoints[1] - hitPoints[0];
        } else if (hitPoints[0] != Vector2.zero){
            distance = hitPoints[2] - hitPoints[0];
        } else if (hitPoints[1] != Vector2.zero){
            distance = hitPoints[1] - hitPoints[2];
        }
        float angle = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    //Gives the Armadillo extra speed when in contact with ramps
    void OnCollisionEnter2D(Collision2D other){
        if(other.collider.tag == "Ramp" && currentIndex == 0){
            animal[currentIndex].speed = 50;
            animal[currentIndex].maxSpeed = 50;
            atRamp = true;
        } else if(this.transform.GetChild(1).GetComponent<Monkey>().atVine && (other.collider.tag == "Platform") ){
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y * 0.9f);
            jumpCurrentWait = 1;
        }
        if(currentIndex == 0 && other.collider.tag == "Wall" && playSound){
            playSound = false;
            AudioSource.PlayClipAtPoint(collideSound, transform.position);
            StartCoroutine(rampSoundWait());
        }
    }
    //Reverts armadillo speed once no longer in contact with the ramp
    void OnCollisionExit2D(Collision2D other) {
        if (other.collider.tag == "Ramp"){
            animal[0].speed = 11;
            animal[0].maxSpeed = 13;
            atRamp = false;
        }
    }

    void OnTriggerEnter2D(Collider2D col){
        
        //player drowns when hit water
        if (col.tag == "Water"){
            if(playDrown){
                Instantiate(splashPrefab.GetComponent<ParticleSystem>(), transform.position, Quaternion.identity);
                AudioSource.PlayClipAtPoint(drownSound, transform.position);
                playDrown = false;
            }
            GameMaster.playerDrown();
        }
        if (col.tag == "MapEdge")
        {   
            if(playDrown){
                AudioSource.PlayClipAtPoint(fallSound, transform.position);
                playDrown = false;
            }
            GameMaster.fallOffMap();
        }

        if(col.tag == "tutorialFinish"){
            CheckPointMaster.firstPosition = new Vector2(-17.39f,-5.48f);
            CheckPointMaster.lastCheckPointPos = CheckPointMaster.firstPosition;
            GameMaster.loadLevel("Level1_ArmadilloOnly");
        } else if (col.tag == "Level1Finish"){
            CheckPointMaster.firstPosition = new Vector2(-10.94f,-7.9f);
            CheckPointMaster.lastCheckPointPos = CheckPointMaster.firstPosition;
            GameMaster.loadLevel("MonkeyBoss");
        } else if (col.tag == "Level2Finish") {
            CheckPointMaster.firstPosition = new Vector2(-11.12f, -7.77f);
            CheckPointMaster.lastCheckPointPos = CheckPointMaster.firstPosition;
            GameMaster.loadLevel("SnakeBoss");
        } else if (col.tag == "Level3Finish"){
            CheckPointMaster.firstPosition = new Vector2(-7f,-5.2f);
            CheckPointMaster.lastCheckPointPos = CheckPointMaster.firstPosition;
            GameMaster.loadLevel("FinalBoss");
        }
    }

    IEnumerator rampSoundWait(){
        yield return new WaitForSeconds(0.9f);
        playSound = true;
    }

    /** Listens for user input to implement controls.
      * Movement of the player and transitioning occurs here.
      */
    void FixedUpdate()
    {
            if (GetComponent<SwingVinePlayer>().attached)         // if attached to vine stop checking for player movement and transformation  in
                                                                  //this script as it will be done in swing vine script.
            {
            MonkeyAnim.SetBool("MonkeyClimbing", true);
            MonkeyAnim.SetBool("MonkeyWalking", false);
            return;
            } else if(currentIndex == 1){
                MonkeyAnim.SetBool("MonkeyClimbing", false);
                MonkeyAnim.SetBool("MonkeyWalking", true);
            }

                                                                //Player morph controls//

        //hold w to be monkey, or s to be snake, otherwise become armadillo when moving
        //ensures player only changes if they are currently in a different animal form.
        //checks if multiple buttons are pressed to stop constant form changes when multiple buttons held.
        if (Input.GetKey(KeyCode.W) && currentIndex != 1 && !Input.GetKey(KeyCode.S) && GameMaster.numAnimals >= 2) {

            changeForm(1);
        } else if (Input.GetKey(KeyCode.S) && currentIndex != 2 && !Input.GetKey(KeyCode.W) && GameMaster.numAnimals >= 3) { 

            changeForm(2);
        } else if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && currentIndex != 0 && !(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))) {
            
            changeForm(0); 
        }

        //can not move during boss instructions
        if (!GameMaster.canMove){
            return;
        }
            
            Vector2 direction = rigidBody.velocity;
            currentVelocity = direction.magnitude;
            //can not pick up speed if airborne
            rigidBody.velocity = Vector2.ClampMagnitude(direction, animal[currentIndex].maxSpeed);

            //used to control 'super jump'
            if (jumpCurrentWait != 0)
            {
                jumpCurrentWait++;
            }
            if (jumpCurrentWait == jumpResetTime)
            {
                jumpCurrentWait = 0;
            }

            if (currentIndex == 0 && !grounded() && !atRamp){
                rigidBody.angularVelocity = Mathf.Clamp(rigidBody.angularVelocity, -maxSpin, maxSpin);
            } else if (currentIndex == 0){
                rigidBody.angularVelocity = Mathf.Clamp(rigidBody.angularVelocity, -maxSpin, maxSpin);
            }
            //Monkey and snake animation conditions
            if (currentIndex == 1){
                if (!grounded()){
                    MonkeyAnim.SetBool("MonkeyWalking", false);
                } else if (rigidBody.velocity.x > 0.5f){
                    MonkeyAnim.SetBool("MonkeyWalking", true);
                }
            } else if (currentIndex == 2){
                if(!grounded()){
                    SnakeAnim.SetBool("SnakeWalking", false);
                } else if (rigidBody.velocity.x > 0.5f){
                    SnakeAnim.SetBool("SnakeWalking", true);
                }
            }
            //jump with space (can not jump if already airborne)
            if (grounded() && Input.GetKey(KeyCode.Space) && jumpCurrentWait == 0 && Shrooms.canBounce && GameMaster.canJump)
            {
                AudioSource.PlayClipAtPoint(jumpSound, transform.position);
                jumpCurrentWait++;
                rigidBody.AddForce(new Vector2(0, animal[currentIndex].jumpPower), ForceMode2D.Impulse);
                //resets rotation of snake and monkey when airborne
                if (currentIndex != 0)
                {
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                }
            } 
            //Horizontal movement (a and d keys) - also controls direction the image is facing
            if (((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && direction.magnitude < animal[currentIndex].maxSpeed))
            {
                float movementX = Input.GetAxis("Horizontal");
                
                //armadillo ball moves with angular velocity
                if(currentIndex == 0){
                    // armadillo speed on ground or ramps
                    if (Input.GetKey(KeyCode.D) && (grounded() || atRamp) ){
                        rigidBody.AddTorque(-(animal[0].speed / 2));
                    } else if (Input.GetKey(KeyCode.A) && (grounded() || atRamp) ){
                        rigidBody.AddTorque((animal[0].speed / 2));
                    //armadillo spin speed in the air
                    } else if (Input.GetKey(KeyCode.D)){
                        rigidBody.AddTorque(-spinSpeed / 5);
                    } else if (Input.GetKey(KeyCode.A)){
                        rigidBody.AddTorque(spinSpeed / 5);
                    }
                }
                //monkey and snake speed, also armadillo force when airborne
                if (currentIndex != 0 || !grounded()){
                    rigidBody.AddForce(new Vector2(animal[currentIndex].speed * movementX - 2, 0));
                }
            
                if (movementX > 0 && !facingRight)
                {
                    flipY();
                }
                else if (movementX < 0 && facingRight)
                {
                    flipY();
                }
                //Rotates monkey and snake with ground level
                if (currentIndex != 0 && grounded() && (jumpCurrentWait == 0 || jumpCurrentWait > jumpResetTime / 4))
                {
                    rotateWithGround();
                }
                //adds armadillo ramp sound
                if(atRamp && rigidBody.velocity.magnitude >= 8.6f && playSound && currentIndex == 0){
                    playSound = false;
                    AudioSource.PlayClipAtPoint(rampSound, transform.position);
                    StartCoroutine(rampSoundWait());
                }
            }

        // Deceleration to satisfy the teams request for better responsiveness XD
        if ((direction.x > 0 && Input.GetKey(KeyCode.A)) || (direction.x > 0 && (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))) && !atRamp) {
            rigidBody.AddForce(new Vector2(-slowSpeed, 0));
        } else if ((direction.x < 0 && Input.GetKey(KeyCode.D)) || (direction.x < 0 && (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))) && !atRamp){
            rigidBody.AddForce(new Vector2(slowSpeed, 0));
        }
            
    }//end FixedUpdate method

}//end PlayerManager class