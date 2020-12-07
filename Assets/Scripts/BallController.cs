using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float ballSpeed;
    private Rigidbody ballRB;
    private Collider ballCol;
    private bool Paused;

    public static BallController Instance { get; private set; }
    public void Awake()
    {
        if (Instance == null)
        {
            //Debug.Log("New Level");
            DontDestroyOnLoad(this);
            Instance = this;
            ballRB = GetComponent<Rigidbody>();
            ballCol = GetComponent<Collider>();
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
        ballRB.AddTorque(Vector3.ClampMagnitude(new Vector3(xSpeed, 0, ySpeed), 1f) * ballSpeed * Time.deltaTime, ForceMode.Acceleration);//balance speed with FPS
        
    }
}
