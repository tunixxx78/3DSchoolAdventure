using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class TuroPlayerMovement : MonoBehaviour
{
    [SerializeField] Animator playerAnimator;
    [SerializeField] Camera MyCam;
    public CharacterController myCC;
    public float moveSpeed, gravity = -9.81f, groundDistance = 0.4f, jumpForce, rotationSpeed, currentTime, startTime, wallforce;
    public int currentPoints, dashAmount;
    [SerializeField] Transform groundCheck, teleportSpawnPoint;
    [SerializeField] LayerMask groundMask;
    bool isGrounded, walkingInWall = false, playerCanBoost = false;
    public Vector3 velocity, movement, turboMove;
    Rigidbody myRB;
    float playerBoosDuration;
    [SerializeField] TMP_Text points, time, gameOverPoints, winningPoints, dashAmountText;
    GameManager gM;

    [SerializeField] GameObject runCam, standCam, playerAvater;

    private SoundFX sfx;


    float x, z;

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
            gameOverPoints.text = currentPoints.ToString();
            gM.gameOverPanel.SetActive(true);
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
            transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up * Time.deltaTime);
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
        /*
        if(Input.GetButton("Fire1") && GetComponent<RocketPack>().canFly == true)
        {
            myCC.Move(turboMove * moveSpeed * Time.deltaTime);
        }
        */
        if(playerCanBoost == true)
        {
            myCC.Move(turboMove * moveSpeed * Time.deltaTime);
        }

        /*
        if( isGrounded && Input.GetButton("Fire2"))
        {
            runCam.SetActive(false);
            standCam.SetActive(true);
            
        }
        else { runCam.SetActive(true); standCam.SetActive(false); }
        */
        
        
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
            winningPoints.text = currentPoints.ToString();
            gM.winningPanel.SetActive(true);
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

            Cursor.lockState = CursorLockMode.None;
            gameOverPoints.text = currentPoints.ToString();
            gM.gameOverPanel.SetActive(true);
            Time.timeScale = 0;
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

            Cursor.lockState = CursorLockMode.None;
            gameOverPoints.text = currentPoints.ToString();
            gM.gameOverPanel.SetActive(true);
            Time.timeScale = 0;
        }
    }
    IEnumerator PlayerCanNotBoost()
    {
        yield return new WaitForSeconds(playerBoosDuration);

        playerCanBoost = false;
    }
}
