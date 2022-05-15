using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class TuroPlayerMovement : MonoBehaviour
{
    public Animator playerAnimator;
    [SerializeField] Camera MyCam;
    public CharacterController myCC;
    public float moveSpeed, accelerationTime, dashMoveSpeed, gravity = -9.81f, gravityOffTime, groundDistance = 0.4f, jumpForce, rotationSpeed, startTime, wallforce, boostDuration, gameOverDelay, boxRideOffTime;
    private float returnGravity, currentSpeed, minSpeed, maxSpeed, speedUpTime; //currenSpeed variable is for player speedUp.
    public int pointsToNextLevel, timeToPoints;


    // For UI programmer use!
    public int currentPoints, dashAmount;
    public float currentTime;
    [SerializeField] TMP_Text points, time, finalPointsText, dashAmountText, resultText;


    [SerializeField] Transform groundCheck, teleportSpawnPoint;
    [SerializeField] LayerMask groundMask;
    bool isGrounded, walkingInWall = false, playerCanBoost = false, isOnSpinner = false, canrideWithBox = false, onObstacle = false, playerIsMoving = false, dashItemsSpawned = false, canNotCollectDash = false, timeHasRunOut = false, goalHasBeenReached = false, playerIsDeath = false;
    public Vector3 velocity, movement, turboMove;
    Rigidbody myRB;
    float playerBoosDuration;
    
    GameManager gM;
    MenuController menuController;

    [SerializeField] GameObject runCam, playerAvater, extraCollider;

    private SoundFX sfx;
    float x, z;


    public static Vector3 currentcheckPoint = Vector3.zero;
    GameManager gameManager;
    DashItemSpawner dash;
    
    private void Awake()
    {
        sfx = FindObjectOfType<SoundFX>();
        //Cursor.lockState = CursorLockMode.Locked;
        playerAnimator = GetComponentInChildren<Animator>();
        //currentcheckPoint = Vector3.zero;
        currentcheckPoint = myCC.transform.position;
        gameManager = FindObjectOfType<GameManager>();
        dash = FindObjectOfType<DashItemSpawner>();

        
        
        
    }

    private void Start()
    {
        myCC.enabled = true;
        if (gameManager.newGame == true)
        {
            Debug.Log("PISTEET PITÄISI NOLLAANTUA");
            pointsToNextLevel = 0;
            PlayerPrefs.SetInt("PointsToNextLevel", 0);
        }
        
        if (gameManager.newGame == false)
        { pointsToNextLevel = PlayerPrefs.GetInt("PointsToNextLevel"); Debug.Log("TALLENNETUT PISTEET KÄYTÖSSÄ"); }


        startTime = gameManager.LevelStartTime;
        myRB = GetComponent<Rigidbody>();
        currentTime = startTime;
        currentPoints = pointsToNextLevel;
        gM = FindObjectOfType<GameManager>();
        menuController = FindObjectOfType<MenuController>();
        //Cursor.lockState = CursorLockMode.Locked;
        runCam.SetActive(true);
        canrideWithBox = true;
        returnGravity = gravity;

        if (dashItemsSpawned == false)
        {
            dash.SpawnDashItems();
            dashItemsSpawned = true;
            StartCoroutine(TurnSpawnedDashToFalse());
        }

        // for Player speedup functionality

        minSpeed = 0;
        maxSpeed = moveSpeed;
        speedUpTime = 0;

        timeHasRunOut = false;
        goalHasBeenReached = false;
        playerIsDeath = false;

       
        
    }


    private void Update()
    {
        
        // creates timer and checks if it is needed
        if(goalHasBeenReached == false)
        {
            currentTime -= 1 * Time.deltaTime;
        }
        

        //string tempTimer = string.Format("{0:00}", currentTime);
        //time.text = tempTimer;



        dashAmountText.text = dashAmount.ToString();
        points.text = currentPoints.ToString();

        

        if(currentTime <= 0)
        {
            Cursor.lockState = CursorLockMode.None;
            finalPointsText.text = currentPoints.ToString();
            //resultText.text = "You lost!";
            //gM.resultPanel.SetActive(true);
            //Time.timeScale = 0;
            menuController.lose = true;

            if (timeHasRunOut == false)
            {
                timeHasRunOut = true;
                sfx.gameOver.Play();
            }
            
        }

        
        //Checks if player is in grounded.
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        Vector3 z2 = z * MyCam.transform.forward;
        Vector3 x2 = x * MyCam.transform.right;

        movement = (z2 + x2).normalized;

        //movement = transform.right * x + transform.forward * z;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        
        // transforms player avatars rotation to wanted directions.
        if (Input.GetKey(KeyCode.W))
        {
            Vector3 dir = Camera.main.transform.forward;
            dir.y = 0;
            dir.Normalize();
            transform.rotation = Quaternion.LookRotation(dir, transform.up * Time.deltaTime);

            if (Input.GetKey(KeyCode.S))
            {
                
                playerAvater.transform.localRotation = Quaternion.Euler(0, 180, 0);
                playerAnimator.SetBool("Run", false);
            }
            else
            {
                if (playerIsMoving == false)
                {
                    Debug.Log("KIIHTYY KIIHTYY");
                    currentSpeed = Mathf.SmoothStep(minSpeed, maxSpeed, speedUpTime / accelerationTime);
                    speedUpTime += Time.deltaTime;
                    if (currentSpeed <= moveSpeed)
                    {
                        currentSpeed++;
                    }
                }


                playerAvater.transform.localRotation = Quaternion.Euler(0, 0, 0);
                MovePlayer();
                myCC.enabled = true;
                
            }
            

            //transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up * Time.deltaTime);
             
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            playerAnimator.SetBool("Run", false);
            if (isOnSpinner)
            {
                myCC.enabled = false;

                
            }
            currentSpeed = 0;
            speedUpTime = 0;
            playerIsMoving = false;


        }
        
        if (Input.GetKey(KeyCode.S))
        {
            if (Input.GetKey(KeyCode.W))
            {
                playerAvater.transform.localRotation = Quaternion.Euler(0, 0, 0);
                playerAnimator.SetBool("Run", false);
            }
            else
            {
                if(playerIsMoving == false)
                {
                    currentSpeed = Mathf.SmoothStep(minSpeed, maxSpeed, speedUpTime / accelerationTime);
                    speedUpTime += Time.deltaTime;
                    if (currentSpeed <= moveSpeed)
                    {
                        currentSpeed++;
                    }
                }

                if (MyCam.transform.localEulerAngles.y < 225 && MyCam.transform.localEulerAngles.y > 135)
                {
                    Debug.Log("PITÄISI REVERSATA PELAAJAN");
                    playerAvater.transform.localRotation = Quaternion.Euler(0, 0, 0);
                }
                else if (MyCam.transform.localEulerAngles.y < 135 && MyCam.transform.localEulerAngles.y > 45)
                {
                    playerAvater.transform.localRotation = Quaternion.Euler(0, 270, 0);
                }
                else if (MyCam.transform.localEulerAngles.y < 315 && MyCam.transform.localEulerAngles.y > 225)
                {
                    playerAvater.transform.localRotation = Quaternion.Euler(0, 90, 0);
                }
                else
                {
                    Debug.Log("NORMILLA MENNÄÄN");
                    playerAvater.transform.localRotation = Quaternion.Euler(0, 180, 0);
                }

                //playerAvater.transform.localRotation = Quaternion.Euler(0, 180, 0);
                MovePlayer();
                myCC.enabled = true;
            }
            
            
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            playerAvater.transform.localRotation = Quaternion.Euler(0, 0, 0);
            playerAnimator.SetBool("Run", false);
            if (isOnSpinner)
            {
                myCC.enabled = false;
            }
            currentSpeed = 0;
            speedUpTime = 0;
            playerIsMoving = false;
        }
        
        if (Input.GetKey(KeyCode.A))
        {
            if (Input.GetKey(KeyCode.D))
            {
                playerAvater.transform.localRotation = Quaternion.Euler(0, 90, 0);
                playerAnimator.SetBool("Run", false);
            }
            else
            {
                if (playerIsMoving == false)
                {
                    currentSpeed = Mathf.SmoothStep(minSpeed, maxSpeed, speedUpTime / accelerationTime);
                    speedUpTime += Time.deltaTime;
                    if (currentSpeed <= moveSpeed)
                    {
                        currentSpeed++;
                    }
                }

                if(MyCam.transform.localEulerAngles.y < 225 && MyCam.transform.localEulerAngles.y > 135)
                {
                    Debug.Log("PITÄISI REVERSATA PELAAJAN");
                    playerAvater.transform.localRotation = Quaternion.Euler(0, 90, 0);
                }
                
                else if (MyCam.transform.localEulerAngles.y < 135 && MyCam.transform.localEulerAngles.y > 45)
                {
                    playerAvater.transform.localRotation = Quaternion.Euler(0, 0, 0);
                }
                else if (MyCam.transform.localEulerAngles.y < 315 && MyCam.transform.localEulerAngles.y > 225)
                {
                    playerAvater.transform.localRotation = Quaternion.Euler(0, 180, 0);
                }

                else
                {
                    Debug.Log("NORMILLA MENNÄÄN");
                    playerAvater.transform.localRotation = Quaternion.Euler(0, -90, 0);
                }
                

                MovePlayer();
                myCC.enabled = true;
            }
            

            
            
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            playerAvater.transform.localRotation = Quaternion.Euler(0, 0, 0);
            playerAnimator.SetBool("Run", false);
            if (isOnSpinner)
            {
                myCC.enabled = false;
            }
            currentSpeed = 0;
            speedUpTime = 0;
            playerIsMoving = false;
        }

        if (Input.GetKey(KeyCode.D))
        {
            if (Input.GetKey(KeyCode.A))
            {
                playerAvater.transform.localRotation = Quaternion.Euler(0, -90, 0);
                playerAnimator.SetBool("Run", false);
            }
            else
            {
                if (playerIsMoving == false)
                {
                    currentSpeed = Mathf.SmoothStep(minSpeed, maxSpeed, speedUpTime / accelerationTime);
                    speedUpTime += Time.deltaTime;
                    if (currentSpeed <= moveSpeed)
                    {
                        currentSpeed++;
                    }
                }

                if (MyCam.transform.localEulerAngles.y < 225 && MyCam.transform.localEulerAngles.y > 135)
                {
                    Debug.Log("PITÄISI REVERSATA PELAAJAN");
                    playerAvater.transform.localRotation = Quaternion.Euler(0, -90, 0);
                }
                else if (MyCam.transform.localEulerAngles.y < 135 && MyCam.transform.localEulerAngles.y > 45)
                {
                    playerAvater.transform.localRotation = Quaternion.Euler(0, 180, 0);
                }
                else if (MyCam.transform.localEulerAngles.y < 315 && MyCam.transform.localEulerAngles.y > 225)
                {
                    playerAvater.transform.localRotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    Debug.Log("NORMILLA MENNÄÄN");
                    playerAvater.transform.localRotation = Quaternion.Euler(0, 90, 0);
                }

                //playerAvater.transform.localRotation = Quaternion.Euler(0, 90, 0);
                MovePlayer();
                myCC.enabled = true;
            }
            
            
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            playerAvater.transform.localRotation = Quaternion.Euler(0, 0, 0);
            playerAnimator.SetBool("Run", false);
            if (isOnSpinner)
            {
                myCC.enabled = false;
            }
            currentSpeed = 0;
            speedUpTime = 0;
            playerIsMoving = false;
        }

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
        {
            playerAvater.transform.localRotation = Quaternion.Euler(0, 45, 0);
        }

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
        {
            playerAvater.transform.localRotation = Quaternion.Euler(0, -45, 0);
        }

        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
        {
            playerAvater.transform.localRotation = Quaternion.Euler(0, 135, 0);
        }

        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
        {
            playerAvater.transform.localRotation = Quaternion.Euler(0, -135, 0);
        }

        turboMove = transform.forward;

        velocity.y += gravity * 2.5f * Time.deltaTime;
        myCC.Move(velocity * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            
            //For spinningPlatform functionality -> returning normalStage
            //myCC.enabled = true;
            //transform.SetParent(GameObject.Find("Players").transform);
            //this.transform.rotation = Quaternion.Euler(0, 0, 0);
            //playerAvater.transform.rotation = Quaternion.Euler(0, 0, 0);
            
            /*
            Vector3 dir = Camera.main.transform.forward;
            dir.y = 0;
            if (onObstacle)
            {
                dir.x = 0;
            }
            dir.Normalize();
            this.transform.localRotation = Quaternion.LookRotation(dir, transform.up * Time.deltaTime);
            */

            //ResetPlayerRotations();

            //isOnSpinner = false;

            sfx.Jump.Play();
            playerAnimator.SetTrigger("Jump");
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity / 2);

            //onObstacle = false;
        }
        
        if(Input.GetButtonDown("Fire1") && dashAmount != 0 && menuController.MenuState != MenuStates.PAUSE && menuController.MenuState != MenuStates.SETTINGS)
        {
            
            playerBoosDuration = boostDuration;
            //myCC.enabled = false;
            gravity = -12;
            playerCanBoost = true;
            dashAmount -= 1;
            sfx.dashFX.Play();
            StartCoroutine(PlayerCanNotBoost());
        }
        
        if(playerCanBoost == true)
        {
            myCC.Move(turboMove * (moveSpeed * dashMoveSpeed) * Time.deltaTime);
            //myCC.Move(Vector3.forward * (moveSpeed * dashMoveSpeed) * Time.deltaTime);
        }        
    }
    private void LateUpdate()
    {
        if (Input.GetButton("Jump"))
        {
            //For spinningPlatform functionality -> returning normalStage

            myCC.enabled = true;
            isOnSpinner = false;
            onObstacle = false;
            transform.SetParent(GameObject.Find("Players").transform);

            


        }
        if (Input.GetButtonDown("Jump"))
        {
            //this.transform.rotation = Quaternion.Euler(0, 0, 0);

            Vector3 dir = Camera.main.transform.forward;
            this.transform.rotation = Quaternion.Euler(dir);

            dir.y = 0;
            if (onObstacle)
            {
                dir.x = 0;
            }
            dir.Normalize();

            this.transform.localRotation = Quaternion.LookRotation(dir, transform.up * Time.deltaTime);
        }
    }

    public void ResetPlayerRotations()
    {
        Vector3 dir = Camera.main.transform.forward;
        dir.y = 0;
        dir.x = 0;
        dir.Normalize();
        this.transform.localRotation = Quaternion.LookRotation(dir, transform.up * Time.deltaTime);
    }

    public void MovePlayer()
    {

        //playerIsMoving = true;

            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A) ||
                Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A))
            {
                myCC.Move(movement * (currentSpeed / 2) * Time.deltaTime);

                if (isGrounded)
                {
                    playerAnimator.SetBool("Run", true);
                }
            }
            else
            {
                myCC.Move(movement * currentSpeed * Time.deltaTime);

                if (isGrounded)
                {
                    playerAnimator.SetBool("Run", true);
                }
            }  
    }

    
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Bouncer")
        {
            sfx.jumpPad.Play();
            velocity.y = Mathf.Sqrt(collider.GetComponent<ThrowingPlatform>().BounceForce);
            gravity = -9.81f;

            StartCoroutine(GravityReset(gravityOffTime));

        }
        if(collider.gameObject.tag == "Collectible")
        {
            float addTime = collider.GetComponent<Collectibles>().timeAmount;
            int addPoints = collider.GetComponent<Collectibles>().pointAmount;

            currentTime = currentTime + addTime;
            currentPoints = currentPoints + addPoints;

            //PlayerPrefs.SetInt("PointsToNextLevel", currentPoints);

            

            if(collider.GetComponent<Collectibles>().hasBoosAdOn == true)
            {
                playerBoosDuration = collider.GetComponent<BoostAdOnToCollectible>().boostDuration;
                playerCanBoost = true;
                StartCoroutine(PlayerCanNotBoost());
            }

            sfx.collectingTime.Play();
            Debug.Log(" Extra Time is : " + addTime + " seconds " + " Extra points are : " + addPoints);
            Destroy(collider.gameObject);
        }
        if(collider.gameObject.tag == "EndLine")
        {
            
            

            // Turning leftOver time to points
            timeToPoints = (int) currentTime;
            currentPoints = currentPoints + (timeToPoints * 100);

            Cursor.lockState = CursorLockMode.None;
            finalPointsText.text = currentPoints.ToString();
            //resultText.text = "YOU WIN!";
            sfx.winning.Play();
            //gM.resultPanel.SetActive(true);
            //Time.timeScale = 0;
            menuController.win = true;

            // saving player points

            PlayerPrefs.SetInt("PointsToNextLevel", currentPoints);

            //Stops this player movement
            playerAnimator.SetBool("Run", false);
            this.enabled = false;
            myCC.enabled = false;

            currentTime = 1;
            goalHasBeenReached = true;

        }
        if(collider.gameObject.tag == "WalkableWall")
        {
            gravity = -1f;
            myCC.Move(new Vector3(0, 0, 0));
            walkingInWall = true;
        }
        if(collider.gameObject.tag == "Teleport")
        {
            sfx.Teleport.Play();

            //transform.position = new Vector3(teleportSpawnPoint.position.x, teleportSpawnPoint.position.y, teleportSpawnPoint.position.z);
        }
        if(collider.gameObject.tag == "PlayerDestroyer" && playerIsDeath == false)
        {
            playerIsDeath = true;
            myCC.enabled = false;
            Debug.Log("TULEEKO MITÄÄN OSUMAA?");
            sfx.gameOver.Play();
            playerAvater.SetActive(false);

            dash.ClearDashItemList();

            StartCoroutine(ToCheckPoint());
            /*
            Cursor.lockState = CursorLockMode.None;
            finalPointsText.text = currentPoints.ToString();
            resultText.text = "YOU LOST!";
            gM.resultPanel.SetActive(true);
            Time.timeScale = 0;
            */
        }

        if (collider.gameObject.tag == "Enemy")
        {
            Debug.Log("TULEEKO MITÄÄN OSUMAA?");
            sfx.gameOver.Play();
            playerAvater.SetActive(false);

            dash.ClearDashItemList();

            StartCoroutine(ToCheckPoint());
            
        }

        if (collider.gameObject.tag == "Dash")
        {
            int addDash = collider.GetComponent<DashObeject>().dashIncreaseAmount;
            int addpoints = collider.GetComponent<DashObeject>().pointsFromDashItem;

            currentPoints = currentPoints + addpoints;

            if (dashAmount < 3 && canNotCollectDash == false)
            {
                canNotCollectDash = true;
                dashAmount = dashAmount + addDash;

                StartCoroutine(turnCollectingDashBackToFalse());
            }

            

            if(collider.GetComponent<DashObeject>().hasDashBoostAddOn == true)
            {
                playerBoosDuration = collider.GetComponent<BoostAdOnToCollectible>().boostDuration;
                playerCanBoost = true;
                StartCoroutine(PlayerCanNotBoost());
            }
            sfx.dashObject.Play();

            Destroy(collider.gameObject);
        }

        if(collider.gameObject.tag == "CheckPoint")
        {
            currentcheckPoint = collider.transform.position;
            currentcheckPoint = currentcheckPoint + new Vector3(0, 2, 0);
            
        }
        if (collider.gameObject.tag == "Roller")
        {
            if (canrideWithBox)
            {
                onObstacle = true;
                canrideWithBox = false;
                myCC.enabled = false;
                transform.SetParent(collider.transform);

                StartCoroutine(CanRideWithBoxAgain(boxRideOffTime));
                //extraCollider.SetActive(true);
            }

        }

        if (collider.gameObject.tag == "Spinner")
        {
            // Spinning platform functonality -> enterin platform mode.
            onObstacle = true;
            Debug.Log("OSAAAAAAAN LENTÄÄÄÄÄÄÄÄ!");
            myCC.enabled = false;
            transform.SetParent(collider.transform);
            isOnSpinner = true;
        }

        if (collider.gameObject.tag == "RotatingCylinder")
        {
            onObstacle = true;
            Debug.Log("Capsulessa ollaan!");
            //myCC.enabled = false;
            //transform.SetParent(collider.transform);


        }
        
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "WalkableWall")
        {
            walkingInWall = false;
            gravity = -9.81f;

        }
        if (collider.gameObject.tag == "Roller")
        {
            transform.SetParent(GameObject.Find("Players").transform);
            myCC.slopeLimit = 45;
            myCC.enabled = true;
        }
        if (collider.gameObject.tag == "Spinner")
        {
            Debug.Log("PITÄISI OLLA IRTI COLLIDERISTA = " + collider);
            
            transform.SetParent(GameObject.Find("Players").transform);
            isOnSpinner = false;
            myCC.enabled = true;
        }
    }

 

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.tag == "Spinner")
        {
            Debug.Log("VIHDOINKIN JOTAIN TOIMIVAA!");
        }
    }



    IEnumerator PlayerCanNotBoost()
    {
        yield return new WaitForSeconds(playerBoosDuration);

        //myCC.enabled = true;
        gravity = returnGravity;
        playerCanBoost = false;
    }
    IEnumerator ToCheckPoint()
    {
        yield return new WaitForSeconds(gameOverDelay);

        transform.position = currentcheckPoint == Vector3.zero ? transform.position : currentcheckPoint;

        myCC.enabled = true;

        sfx.Teleport.Play();
        playerAvater.SetActive(true);

        if (dashItemsSpawned == false)
        {
            dash.SpawnDashItems();
            dashItemsSpawned = true;
            StartCoroutine(TurnSpawnedDashToFalse());
        }
        playerIsDeath = false;
        
    }
    IEnumerator CanRideWithBoxAgain(float offTime)
    {
        yield return new WaitForSeconds(offTime);

        canrideWithBox = true;
    }
    IEnumerator GravityReset(float bounceTime)
    {
        yield return new WaitForSeconds(bounceTime);

        gravity = returnGravity;
    }
    IEnumerator TurnSpawnedDashToFalse()
    {
        yield return new WaitForSeconds(1);
        dashItemsSpawned = false;
    }

    IEnumerator turnCollectingDashBackToFalse()
    {
        yield return new WaitForSeconds(.5f);

        canNotCollectDash = false;
    }
}
