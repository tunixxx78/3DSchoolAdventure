using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class TuroPlayerMovement : MonoBehaviour
{
    [SerializeField] Animator playerAnimator;
    [SerializeField] Camera MyCam;
    public CharacterController myCC;
    public float moveSpeed, gravity = -9.81f, groundDistance = 0.4f, jumpForce, rotationSpeed, currentPoints, currentTime, startTime, wallforce;
    [SerializeField] Transform groundCheck, teleportSpawnPoint;
    [SerializeField] LayerMask groundMask;
    bool isGrounded, canTurn = true, walkingInWall = false, playerCanBoost = false;
    public Vector3 velocity, movement, turboMove;
    Rigidbody myRB;
    float playerBoosDuration;
    [SerializeField] TMP_Text points, time, gameOverPoints, winningPoints;
    GameManager gM;

    [SerializeField] GameObject runCam, standCam, jumpCam
        ;

    private SoundFX sfx;


    float x, z, xRot = 0f, mouseX;

    private void Awake()
    {
        sfx = FindObjectOfType<SoundFX>();
        Cursor.lockState = CursorLockMode.Locked;
        playerAnimator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {

        myRB = GetComponent<Rigidbody>();
        currentTime = startTime;
        gM = FindObjectOfType<GameManager>();
        Cursor.lockState = CursorLockMode.Locked;
        
    }

    private void Update()
    {
        

        currentTime -= 1 * Time.deltaTime;

        string tempTimer = string.Format("{0:00}", currentTime);
        time.text = tempTimer;

        //time.text = currentTime.ToString();
        points.text = currentPoints.ToString();

        if(currentTime <= 0)
        {
            Cursor.lockState = CursorLockMode.None;
            gameOverPoints.text = currentPoints.ToString();
            gM.gameOverPanel.SetActive(true);
            Time.timeScale = 0;
        }

        

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        //mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * 300;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if(velocity.y <= -5)
        {
            jumpCam.SetActive(true);
            runCam.SetActive(false);
            standCam.SetActive(false);
        }

        //transform.Rotate(Vector3.up * mouseX);
        /*
        if(mouseX > 0)
        {
            xRot += mouseX;

            transform.localRotation = Quaternion.Euler(0, xRot * 300 * Time.deltaTime, 0);
            transform.Rotate(Vector3.up * mouseX);
        }

        if (mouseX < 0)
        {
            xRot += -mouseX;

            transform.localRotation = Quaternion.Euler(0, -xRot, 0);

            transform.Rotate(Vector3.up * mouseX);
        }

        
        if(x > 0 && canTurn)
        {
            canTurn = false;

            xRot += 90;

            transform.localRotation = Quaternion.Euler(0, xRot, 0);

            StartCoroutine(TurningBackToTrue());
        }

        if (x < 0 && canTurn)
        {
            canTurn = false;

            xRot -= 90;

            transform.localRotation = Quaternion.Euler(0, xRot, 0);

            StartCoroutine(TurningBackToTrue());
        }
        
        */
        movement = transform.right * x + transform.forward * z;
        myCC.Move(movement * moveSpeed * Time.deltaTime);

        turboMove = transform.forward;

        velocity.y += gravity * 2 * Time.deltaTime;
        myCC.Move(velocity * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            sfx.Jump.Play();
            playerAnimator.SetTrigger("Jump");
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity / 2);
        }

        if(Input.GetButton("Fire1") && GetComponent<RocketPack>().canFly == true)
        {
            myCC.Move(turboMove * moveSpeed * Time.deltaTime);
        }

        if(playerCanBoost == true)
        {
            myCC.Move(turboMove * moveSpeed * Time.deltaTime);
        }

       
        /*
        if(x == 0 && z == 0 && velocity.y <= 0)
        {
            Debug.Log("Pitäisi olla paikallaan");
            standCam.SetActive(true);
            runCam.SetActive(false);
        }
        else
        {
            Debug.Log("Pitäisi olla liikkeessä");
            runCam.SetActive(true);
            standCam.SetActive(false);
        }
        */

        if(isGrounded && Input.GetKey(KeyCode.W) || isGrounded && Input.GetKey(KeyCode.S))
        {
            playerAnimator.SetBool("Run", true);
        }
        else { playerAnimator.SetBool("Run", false); }

        if (isGrounded && Input.GetKey(KeyCode.A))
        {
            playerAnimator.SetBool("LeftRun", true);
        }
        else { playerAnimator.SetBool("LeftRun", false); }

        if (isGrounded && Input.GetKey(KeyCode.D))
        {
            playerAnimator.SetBool("RightRun", true);
        }
        else { playerAnimator.SetBool("RightRun", false); }

        if( isGrounded && Input.GetButton("Fire2"))
        {
            runCam.SetActive(false);
            standCam.SetActive(true);
            jumpCam.SetActive(false);
        }
        else if (isGrounded && velocity.y > -5) { runCam.SetActive(true); standCam.SetActive(false); jumpCam.SetActive(false); }
        
        else { runCam.SetActive(false); standCam.SetActive(false); jumpCam.SetActive(true); }
    }

    IEnumerator TurningBackToTrue()
    {
        yield return new WaitForSeconds(.5f);
        canTurn = true;
    }

    void BasicRotation()
    {
        

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
            float addPoints = collider.GetComponent<Collectibles>().pointAmount;

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
            gravity = 0;
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
