using System.Collections;
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
    //knockback
    private float hurtforce = 20f;
    Vector3 knockBack;

    public GameObject pauseMenu;
    private bool Paused;
    public Button ResumeButton;
    public Button MenuButton;
    public Button QuitButton;
    public AudioSource igMusic;
    public float timeRemaining;
    public Text timer;
    public Text fallText;
    public Text ExtraTime1;
    public Text ExtraTime2;
    public Text ExtraTime3;
    public float temp;
    public int curLvl;
    public Vector3 checkPoint;
    private int fallCtr = 0;
    public float lvl1;
    public float lvl2;
    public float lvl3;


    public static BallController Instance { get; private set; }
    public void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(this);
            Instance = this;

            ball = GameObject.FindGameObjectWithTag("Player1");
            ballRB = GetComponent<Rigidbody>();
            ballCol = GetComponent<Collider>();

            //Level 1 starting position
            checkPoint = new Vector3(.65f, 1f, .5f);

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
            timer = GameObject.FindGameObjectWithTag("TimerText").GetComponent<Text>();
            fallText = GameObject.FindGameObjectWithTag("FallCtr").GetComponent<Text>();
            ExtraTime1 = GameObject.FindGameObjectWithTag("ExtraTime1").GetComponent<Text>();
            ExtraTime2 = GameObject.FindGameObjectWithTag("ExtraTime2").GetComponent<Text>();
            ExtraTime3 = GameObject.FindGameObjectWithTag("ExtraTime3").GetComponent<Text>();
            igMusic = GameObject.FindGameObjectWithTag("PlayerUI").GetComponent<AudioSource>();
        }
        else
        {
            //stop all other versions of this game object
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {   
        if (!Paused)
        {
            timerCD();
        }
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
            ExtraTime1.text = "";
            ExtraTime1.text = "";
            ExtraTime1.text = "";
            checkPoint = new Vector3(.65f, 1f, .5f);
            ball.transform.position = checkPoint;
            timeRemaining = 60f;
            curLvl = 3;
            igMusic.Play();
        }
        if (level == 4)
        {
            lvl1 = timeRemaining;
            ExtraTime1.text = string.Format("+ {0:00}:{1:00}", Mathf.FloorToInt(lvl1 / 60), Mathf.FloorToInt(lvl1 % 60));

            checkPoint = new Vector3(2.5f, 2f, -2.5f);
            ball.transform.position = checkPoint;
            timeRemaining = 120f + lvl1;
            curLvl = 4;
        }
        if (level == 5)
        {
            lvl2 = timeRemaining;
            ExtraTime2.text = string.Format("+ {0:00}:{1:00}", Mathf.FloorToInt(lvl2 / 60), Mathf.FloorToInt(lvl2 % 60));

            checkPoint = new Vector3(135, 33, -2);
            ball.transform.position = checkPoint;
            timeRemaining = 120f + lvl2;
            curLvl = 5;
        }   
        if (level == 7)
        {
            lvl3 = timeRemaining;
            ExtraTime3.text = string.Format("+ {0:00}:{1:00}", Mathf.FloorToInt(lvl3 / 60), Mathf.FloorToInt(lvl3 % 60));
            //create stats display UI here
        }

    }

    //collectables/Boosters
    private void OnTriggerEnter(Collider collision)
    {

    }

    //Collision Tracker --> Enemies/Obstacles/Restarts
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Restart")
        {
            ball.transform.position = checkPoint;
            ballRB.velocity = new Vector3(0, 0, 0);
            fallCtr++;
            fallText.text = fallCtr.ToString();
        }
        if(collision.gameObject.tag == "NextLevel")
        {
            if (curLvl == 5)
            {
                Scene_Manager.LoadScene(7);
            }
            else
            {
                curLvl++;
                Scene_Manager.LoadScene(curLvl);
            }
        }

        //bouncy enemy knockback
        if (collision.gameObject.tag == "Enemy")
        {
            ball.transform.position = checkPoint;
            ballRB.velocity = new Vector3(0, 0, 0);
            fallCtr++;
            fallText.text = fallCtr.ToString();
            //knockBack = Vector3.forward * -10;
            //ballRB.AddForce(knockBack * 100);
            /*ballRB.velocity = new Vector3(hurtforce, ballRB.velocity.y, -hurtforce);
            if (collision.gameObject.transform.position.x > transform.position.x)
            {
                ballRB.velocity = new Vector3(hurtforce, ballRB.velocity.y, -hurtforce);
            }
            else
            {
                ballRB.velocity = new Vector3(-hurtforce, ballRB.velocity.y, hurtforce);
            }*/
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

    public void timerCD()
    {
        //if the game isnt paused, countdown on time remaining
        if (timeRemaining > 0.05f)
        {
            timeRemaining -= Time.deltaTime;
            timer.text = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(timeRemaining / 60), Mathf.FloorToInt(timeRemaining % 60));
        }
        else //if (timeRemaining <= 0.01f)
        {
            Destroy(this.gameObject);
            //ran out of time --> Load Lose screen
            Scene_Manager.LoadScene(6);
        }
    }
}
