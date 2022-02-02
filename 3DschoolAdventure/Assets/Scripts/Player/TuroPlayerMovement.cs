using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class TuroPlayerMovement : MonoBehaviour
{
    public CharacterController myCC;
    [SerializeField] float moveSpeed, gravity = -9.81f, groundDistance = 0.4f, jumpForce, rotationSpeed, currentPoints, currentTime, startTime;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    bool isGrounded;
    Vector3 velocity, movement;
    Rigidbody myRB;

    [SerializeField] TMP_Text points, time, gameOverPoints, winningPoints;
    GameManager gM;

    [SerializeField] GameObject runCam, standCam;

    private void Start()
    {
        myRB = GetComponent<Rigidbody>();
        currentTime = startTime;
        gM = FindObjectOfType<GameManager>();
        
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
            gameOverPoints.text = currentPoints.ToString();
            gM.gameOverPanel.SetActive(true);
            Time.timeScale = 0;
        }

        BasicRotation();

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        movement = transform.right * x + transform.forward * z;
        myCC.Move(movement * Time.deltaTime * moveSpeed);

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        myCC.Move(velocity * Time.deltaTime);
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

    void BasicRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * rotationSpeed;
        transform.Rotate(new Vector3(0, mouseX, 0));
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Bouncer")
        {
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
            winningPoints.text = currentPoints.ToString();
            gM.winningPanel.SetActive(true);
            Time.timeScale = 0;
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("PlayerDestroyer"))
        {
            Debug.Log("TULEEKO MITÄÄN OSUMAA?");

            gameOverPoints.text = currentPoints.ToString();
            gM.gameOverPanel.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
