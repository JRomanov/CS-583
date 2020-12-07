﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BallController : MonoBehaviour
{
    public float ballSpeed;
    public GameObject ball;
    public Rigidbody ballRB;
    public Collider ballCol;

    public GameObject pauseMenu;
    private bool Paused;
    public Button ResumeButton;
    public Button MenuButton;
    public Button QuitButton;
    public AudioSource igMusic;

    public Vector3 checkPoint;

    public static BallController Instance { get; private set; }
    public void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(this);
            Instance = this;

            ball = GameObject.FindGameObjectWithTag("Player");
            ballRB = GetComponent<Rigidbody>();
            ballCol = GetComponent<Collider>();

            //Level 1 starting position
            checkPoint = new Vector3(135, 33, -2);

            //igMusic = GameObject.FindGameObjectWithTag("InGameMusic").GetComponent<AudioSource>();
            pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
            MenuButton = GameObject.FindGameObjectWithTag("BackButton").GetComponent<Button>();
            MenuButton.onClick.AddListener(() => mainMenu());
            QuitButton = GameObject.FindGameObjectWithTag("QuitButton").GetComponent<Button>();
            QuitButton.onClick.AddListener(() => Scene_Manager.Game_Quit());
            ResumeButton = GameObject.FindGameObjectWithTag("ResumeButton").GetComponent<Button>();
            ResumeButton.onClick.AddListener(() => ResumeGame());
            Paused = false;
            pauseMenu.SetActive(false);
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
    private void Update()
    {
        //PauseMenu Functionality
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Paused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    void FixedUpdate()
    {
        //movement
        float xSpeed = Input.GetAxis("Horizontal");
        float ySpeed = Input.GetAxis("Vertical");

        ballRB.AddForce(Vector3.ClampMagnitude(new Vector3(-ySpeed, 0, xSpeed), 1f) * ballSpeed * Time.deltaTime, ForceMode.Acceleration);

        //Balance speed with FPS
        ballRB.AddTorque(Vector3.ClampMagnitude(new Vector3(xSpeed, 0, ySpeed), 1f) * ballSpeed * Time.deltaTime, ForceMode.Acceleration);
    }
    
    //Set new checkpoint/reset timers/Add extra time
    private void OnLevelWasLoaded(int level)
    {

        if (level == 3)
        {
            ball.transform.position = new Vector3(135, 33, -2); ;
        }
        if (level == 4)
        {
            //ball.transform.position = new Vector3(-8, -3f, 0);

        }
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

    public void ResumeGame()
    {
        Scene_Manager.ButtonPress.Play();
        pauseMenu.SetActive(false);
        //igMusic.mute = false;
        Time.timeScale = 1f;
        Paused = false;

    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        //igMusic.mute = true;
        Time.timeScale = 0f;
        Paused = true;
    }

    public void mainMenu()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        Scene_Manager.LoadScene(0);
    }
}
