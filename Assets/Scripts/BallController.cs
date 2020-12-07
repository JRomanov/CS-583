using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float ballSpeed;
    public GameObject ball;
    public Rigidbody ballRB;
    public Collider ballCol;
    private bool Paused;
    public Vector3 checkPoint;

    public static BallController Instance { get; private set; }
    public void Awake()
    {
        if (Instance == null)
        {
            //Debug.Log("New Level");
            DontDestroyOnLoad(this);
            Instance = this;
            ball = GameObject.FindGameObjectWithTag("Player");
            ballRB = GetComponent<Rigidbody>();
            ballCol = GetComponent<Collider>();

            //Level 1 starting position
            checkPoint = new Vector3(135, 33, -2);
        }
        else
        {
            //stop all other versions of this game object
            Destroy(gameObject);
        }
    }
    void Start()
    {

    }

    void FixedUpdate()
    {
        //movement
        float xSpeed = Input.GetAxis("Horizontal");
        float ySpeed = Input.GetAxis("Vertical");

        ballRB.AddForce(Vector3.ClampMagnitude(new Vector3(-ySpeed, 0, xSpeed), 1f) * ballSpeed * Time.deltaTime, ForceMode.Acceleration);

        //balance speed with FPS
        ballRB.AddTorque(Vector3.ClampMagnitude(new Vector3(xSpeed, 0, ySpeed), 1f) * ballSpeed * Time.deltaTime, ForceMode.Acceleration); 
    }
    //collectables/Boosters
    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

    //Collision Tracker --> Enemies/Obstacles/Restarts
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Restart")
        {
            ball.transform.position = checkPoint;
            ballRB.velocity = new Vector3(0, 0, 0);
        }
    }
}
