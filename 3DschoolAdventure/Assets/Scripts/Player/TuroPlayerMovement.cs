using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class TuroPlayerMovement : MonoBehaviour
{
    [SerializeField] Camera MyCam;
    public CharacterController myCC;
    public float moveSpeed, gravity = -9.81f, groundDistance = 0.4f, jumpForce, rotationSpeed, currentPoints, currentTime, startTime;
    [SerializeField] Transform groundCheck, teleportSpawnPoint;
    [SerializeField] LayerMask groundMask;
    bool isGrounded, canTurn = true;
    public Vector3 velocity, movement;
    Rigidbody myRB;

    [SerializeField] TMP_Text points, time, gameOverPoints, winningPoints;
    GameManager gM;

    [SerializeField] GameObject runCam, standCam;

    private SoundFX sfx;


    float x, z, xRot = 0f, mouseX;

    private void Awake()
    {
        sfx = FindObjectOfType<SoundFX>();
        Cursor.lockState = CursorLockMode.Locked;
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

        mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * 300;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        transform.Rotate(Vector3.up * mouseX);
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

        

        velocity.y += gravity * Time.deltaTime;
        myCC.Move(velocity * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            sfx.Jump.Play();
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
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
            velocity.y = Mathf.Sqrt(jumpForce * collider.GetComponent<ThrowingPlatform>().BounceForce);
        }
        if(collider.gameObject.tag == "Collectible")
        {
            float addTime = collider.GetComponent<Collectibles>().timeAmount;
            float addPoints = collider.GetComponent<Collectibles>().pointAmount;

            currentTime = currentTime + addTime;
            currentPoints = currentPoints + addPoints;

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
            myRB.constraints = RigidbodyConstraints.FreezeAll;
            gravity = 0;
        }
        if(collider.gameObject.tag == "Teleport")
        {
            sfx.Teleport.Play();
            transform.position = new Vector3(teleportSpawnPoint.position.x, teleportSpawnPoint.position.y, teleportSpawnPoint.position.z);
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
}
