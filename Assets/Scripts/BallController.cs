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

    public GameObject pauseMenu;
    public GameObject timer;
    private bool Paused;
    public Button ResumeButton;
    public Button MenuButton;
    public Button QuitButton;
    public AudioSource igMusic;
    public float timeRemaining;
    public Text timerText;
    public Text fallText;
    public Text levelTime1;
    public Text levelTime2;
    public Text levelTime3;
    public float temp;
    public int curLvl;
    public Vector3 checkPoint;
    public int fallCtr = 0;
    //bnonus time
    public float lvl1b;
    public float lvl2b;
    public float lvl3b;
    //time completed
    public float lvl1c;
    public float lvl2c;
    public float lvl3c;
    //falls
    public float fall1 = 0;
    public float fall2 = 0;
    public float fall3 = 0;

    public float totalTC;
    public float totalBT;


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

            //player Pause Menu references
            pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
            MenuButton = GameObject.FindGameObjectWithTag("BackButton").GetComponent<Button>();
            MenuButton.onClick.AddListener(() => mainMenu());
            QuitButton = GameObject.FindGameObjectWithTag("QuitButton").GetComponent<Button>();
            QuitButton.onClick.AddListener(() => Scene_Manager.Game_Quit());
            ResumeButton = GameObject.FindGameObjectWithTag("ResumeButton").GetComponent<Button>();
            ResumeButton.onClick.AddListener(() => ResumeGame());
            Paused = false;
            pauseMenu.SetActive(false);

            //player UI references
            timer = GameObject.FindGameObjectWithTag("Timer");
            timerText = GameObject.FindGameObjectWithTag("TimerText").GetComponent<Text>();
            fallText = GameObject.FindGameObjectWithTag("FallCtr").GetComponent<Text>();
            levelTime1 = GameObject.FindGameObjectWithTag("ExtraTime1").GetComponent<Text>();
            levelTime2 = GameObject.FindGameObjectWithTag("ExtraTime2").GetComponent<Text>();
            levelTime3 = GameObject.FindGameObjectWithTag("ExtraTime3").GetComponent<Text>();
            igMusic = GameObject.FindGameObjectWithTag("igMusic").GetComponent<AudioSource>();
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
        if(level == 0)
        {
            igMusic.Stop();
            Destroy(this.gameObject);
        }
        if (level == 3)
        {
            timer.SetActive(true);
            levelTime1.text = "";
            levelTime2.text = "";
            levelTime3.text = "";
            checkPoint = new Vector3(.65f, 1f, .5f);
            ball.transform.position = checkPoint;
            timeRemaining = 105f;
            curLvl = 3;
            igMusic.Play();
        }
        if (level == 4)
        {
            fall1 = fallCtr;
            //time completed previous level
            lvl1c = 105f - timeRemaining;
            //bonus time
            lvl1b = timeRemaining;
            levelTime1.text = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(lvl1c / 60), Mathf.FloorToInt(lvl1c % 60));

            checkPoint = new Vector3(2.5f, 2f, -2.5f);
            ball.transform.position = checkPoint;
            //new timer for current lvl
            timeRemaining = 75f + lvl1b;
            curLvl = 4;
        }
        if (level == 5)
        {
            fall2 = fallCtr - fall1;
            //time completed previous level
            lvl2c = 75f - timeRemaining + lvl1b;
            //bonus time
            lvl2b = timeRemaining;
            levelTime2.text = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(lvl2c / 60), Mathf.FloorToInt(lvl2c % 60));

            checkPoint = new Vector3(135, 33, -2);
            ball.transform.position = checkPoint;
            //new timer for current lvl
            timeRemaining = 120f + lvl2b;
            curLvl = 5;
        } 
        /*
        if (level == 7)
        {
            fall3 = fallCtr;
            //time completed previous level
            lvl3c = 120f - timeRemaining;
            //bonus time
            lvl3b = timeRemaining;
            levelTime3.text = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(lvl3c / 60), Mathf.FloorToInt(lvl3c % 60));
            timer.SetActive(false);
            //create stats display UI here
        }
        */
        if(level == 8)
        {
            igMusic.Stop();
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
                fall3 = fallCtr - fall2 - fall1;
                //time completed previous level
                lvl3c = 120f - timeRemaining + lvl2b;
                //bonus time
                lvl3b = timeRemaining;
                levelTime3.text = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(lvl3c / 60), Mathf.FloorToInt(lvl3c % 60));
                totalTC = lvl1c + lvl2c + lvl3c;
                totalBT = lvl1b + lvl2b + lvl3b;
                timer.SetActive(false);
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
            //if player if behind the object
            if (ball.transform.position.x >= (collision.transform.position.x + .4f) )
            {
                Debug.Log("Player is behind Bouncy.");
                ballRB.AddForce(new Vector3(ball.transform.position.x, 0, 0) * -45f);
            }
            //else if player is in front of object
            else if (ball.transform.position.x <= (collision.transform.position.x -1.05f))
            {
                Debug.Log("Player is in front Bouncy.");
                ballRB.AddForce(new Vector3(ball.transform.position.x, 0, 0) * 45f);
            }
            else if ((ball.transform.position.x < (collision.transform.position.x + .4f)) || (ball.transform.position.x > (collision.transform.position.x - 1.05f)))
            {
                Debug.Log("Player is within side Threshold, checking sides");
                //push right
                if (ball.transform.position.z > collision.transform.position.z)
                {
                    ballRB.AddForce(new Vector3(0, 0, ball.transform.position.z) * 330f);
                    Debug.Log("Bouncing player to the right");
                }
                //else push left
                else if (ball.transform.position.z < collision.transform.position.z)
                {
                    ballRB.AddForce(new Vector3(0, 0, ball.transform.position.z) * 700f);
                    Debug.Log("Bouncing player to the left");
                }
            }
        }

    }

    public void ResumeGame()
    {
        Scene_Manager.ButtonPress.Play();
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        Paused = false;
        igMusic.mute = false;
    }

    public void PauseGame()
    {

        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        Paused = true;
        igMusic.mute = true;
    }

    public void mainMenu()
    {
        pauseMenu.SetActive(false);
        Paused = false;
        Time.timeScale = 1f;
        igMusic.Stop();
        Scene_Manager.LoadScene(0);
    }

    public void timerCD()
    {
        //if the game isnt paused, countdown on time remaining
        if (timeRemaining > 0.05f)
        {
            timeRemaining -= Time.deltaTime;
            timerText.text = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(timeRemaining / 60), Mathf.FloorToInt(timeRemaining % 60));
        }
        else //if (timeRemaining <= 0.01f)
        {
            Destroy(this.gameObject);
            //ran out of time --> Load Lose screen
            Scene_Manager.LoadScene(6);
        }
    }
}
