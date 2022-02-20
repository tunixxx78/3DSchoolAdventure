using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class TuroPlayerMovement : MonoBehaviour
{
    [SerializeField] Animator playerAnimator;
    [SerializeField] Camera MyCam;
    public CharacterController myCC;
    public float moveSpeed, gravity = -9.81f, groundDistance = 0.4f, jumpForce, rotationSpeed, startTime, wallforce, boostDuration;

    // For UI programmer use!
    public int currentPoints, dashAmount;
    public float currentTime;
    [SerializeField] TMP_Text points, time, finalPointsText, dashAmountText, resultText;


    [SerializeField] Transform groundCheck, teleportSpawnPoint;
    [SerializeField] LayerMask groundMask;
    bool isGrounded, walkingInWall = false, playerCanBoost = false;
    public Vector3 velocity, movement, turboMove;
    Rigidbody myRB;
    float playerBoosDuration;
    
    GameManager gM;

    [SerializeField] GameObject runCam, playerAvater;

    private SoundFX sfx;
    float x, z;

    
    public static Vector3 currentcheckPoint = Vector3.zero;
    
    private void Awake()
    {
        sfx = FindObjectOfType<SoundFX>();
        //Cursor.lockState = CursorLockMode.Locked;
        playerAnimator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {

        myRB = GetComponent<Rigidbody>();
        currentTime = startTime;
        currentPoints = 0;
        gM = FindObjectOfType<GameManager>();
        //Cursor.lockState = CursorLockMode.Locked;
        runCam.SetActive(true);

    }

    private void Update()
    {
        
        // creates timer
        currentTime -= 1 * Time.deltaTime;

        string tempTimer = string.Format("{0:00}", currentTime);
        time.text = tempTimer;



        dashAmountText.text = dashAmount.ToString();
        points.text = currentPoints.ToString();

        if(currentTime <= 0)
        {
            Cursor.lockState = CursorLockMode.None;
            finalPointsText.text = currentPoints.ToString();
            resultText.text = "You lost!";
            gM.resultPanel.SetActive(true);
            Time.timeScale = 0;
        }

        
        //Checks if player is in grounded.
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
        

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

            //transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up * Time.deltaTime);
             MovePlayer();
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            playerAnimator.SetBool("Run", false);
        }
        
        if (Input.GetKey(KeyCode.S))
        {
            playerAvater.transform.localRotation = Quaternion.Euler(0, 180, 0);
            MovePlayer();
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            playerAvater.transform.localRotation = Quaternion.Euler(0, 0, 0);
            playerAnimator.SetBool("Run", false);
        }

        if (Input.GetKey(KeyCode.A))
        {
            playerAvater.transform.localRotation = Quaternion.Euler(0, -90, 0);
            MovePlayer();
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            playerAvater.transform.localRotation = Quaternion.Euler(0, 0, 0);
            playerAnimator.SetBool("Run", false);
        }

        if (Input.GetKey(KeyCode.D))
        {
            playerAvater.transform.localRotation = Quaternion.Euler(0, 90, 0);
            MovePlayer();
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            playerAvater.transform.localRotation = Quaternion.Euler(0, 0, 0);
            playerAnimator.SetBool("Run", false);
        }



        movement = transform.right * x + transform.forward * z;

        turboMove = transform.forward;

        velocity.y += gravity * 2 * Time.deltaTime;
        myCC.Move(velocity * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            sfx.Jump.Play();
            playerAnimator.SetTrigger("Jump");
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity / 2);
        }
        
        if(Input.GetButtonDown("Fire1") && dashAmount != 0)
        {
            playerBoosDuration = boostDuration;
            playerCanBoost = true;
            dashAmount -= 1;
            StartCoroutine(PlayerCanNotBoost());
        }
        
        if(playerCanBoost == true)
        {
            myCC.Move(turboMove * moveSpeed * Time.deltaTime);
        }        
    }


    public void MovePlayer()
    {
        myCC.Move(movement * moveSpeed * Time.deltaTime);

        if (isGrounded)
        {
            playerAnimator.SetBool("Run", true);
        }
        
    }

    
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Bouncer")
        {
            sfx.Jump.Play();
            velocity.y = Mathf.Sqrt(collider.GetComponent<ThrowingPlatform>().BounceForce);
        }
        if(collider.gameObject.tag == "Collectible")
        {
            float addTime = collider.GetComponent<Collectibles>().timeAmount;
            int addPoints = collider.GetComponent<Collectibles>().pointAmount;

            currentTime = currentTime + addTime;
            currentPoints = currentPoints + addPoints;

            if(collider.GetComponent<Collectibles>().hasBoosAdOn == true)
            {
                playerBoosDuration = collider.GetComponent<BoostAdOnToCollectible>().boostDuration;
                playerCanBoost = true;
                StartCoroutine(PlayerCanNotBoost());
            }

            Debug.Log(" Extra Time is : " + addTime + " seconds " + " Extra points are : " + addPoints);
            Destroy(collider.gameObject);
        }
        if(collider.gameObject.tag == "EndLine")
        {
            Cursor.lockState = CursorLockMode.None;
            finalPointsText.text = currentPoints.ToString();
            resultText.text = "YOU WIN!";
            gM.resultPanel.SetActive(true);
            Time.timeScale = 0;
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
            transform.position = new Vector3(teleportSpawnPoint.position.x, teleportSpawnPoint.position.y, teleportSpawnPoint.position.z);
        }
        if(collider.gameObject.tag == "PlayerDestroyer")
        {
            Debug.Log("TULEEKO MITÄÄN OSUMAA?");

            transform.position = currentcheckPoint == Vector3.zero ? transform.position : currentcheckPoint;
            /*
            Cursor.lockState = CursorLockMode.None;
            finalPointsText.text = currentPoints.ToString();
            resultText.text = "YOU LOST!";
            gM.resultPanel.SetActive(true);
            Time.timeScale = 0;
            */
        }

        if(collider.gameObject.tag == "Dash")
        {
            int addDash = collider.GetComponent<DashObeject>().dashIncreaseAmount;

            dashAmount = dashAmount + addDash;

            if(collider.GetComponent<DashObeject>().hasDashBoostAddOn == true)
            {
                playerBoosDuration = collider.GetComponent<BoostAdOnToCollectible>().boostDuration;
                playerCanBoost = true;
                StartCoroutine(PlayerCanNotBoost());
            }

            Destroy(collider.gameObject);
        }

        if(collider.gameObject.tag == "CheckPoint")
        {
            currentcheckPoint = collider.transform.position;
        }
        
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "WalkableWall")
        {
            walkingInWall = false;
            gravity = -9.81f;

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("PlayerDestroyer"))
        {
            Debug.Log("TULEEKO MITÄÄN OSUMAA?");

            transform.position = currentcheckPoint == Vector3.zero ? transform.position : currentcheckPoint;
            /*
            Cursor.lockState = CursorLockMode.None;
            finalPointsText.text = currentPoints.ToString();
            resultText.text = "YOU LOST!";
            gM.resultPanel.SetActive(true);
            Time.timeScale = 0;
            */
        }
    }
    IEnumerator PlayerCanNotBoost()
    {
        yield return new WaitForSeconds(playerBoosDuration);

        playerCanBoost = false;
    }
}
