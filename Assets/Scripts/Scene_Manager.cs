using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scene_Manager : MonoBehaviour
{
    private Button Play;
    private Button About;
    private Button Back;
    private Button Quit;
    private Button Start;
    public Button CreditsButton;
    public Button Restart;
    public AudioSource menuMusic;
    public AudioSource creditsMusic;
    public static AudioSource ButtonPress;
    public AudioSource igMusic;
    public bool inGame;
    public bool musicPlaying;

    public Text tc1;
    public Text tc2;
    public Text tc3;
    public Text tct;
    public Text bt1;
    public Text bt2;
    public Text bt3;
    public Text btt;
    public Text f1;
    public Text f2;
    public Text f3;
    public Text ft;
    public Text rank;

    public float tempTR;

    public GameObject BallInst;
    public static Scene_Manager Instance { get; private set; }
    public void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(this);
            Instance = this;

            //Initial Link of Main Menu Buttons
            Play = GameObject.FindGameObjectWithTag("PlayButton").GetComponent<Button>();
            Play.onClick.AddListener(() => LoadScene(2));
            About = GameObject.FindGameObjectWithTag("AboutButton").GetComponent<Button>();
            About.onClick.AddListener(() => LoadScene(1));
            Quit = GameObject.FindGameObjectWithTag("QuitButton").GetComponent<Button>();
            Quit.onClick.AddListener(() => Game_Quit());
            menuMusic = GetComponent<AudioSource>();
            menuMusic.Play();
            ButtonPress = GameObject.FindGameObjectWithTag("ButtonPress").GetComponent<AudioSource>();       
            musicPlaying = true;
            igMusic = GameObject.FindGameObjectWithTag("igMusic").GetComponent<AudioSource>();

        }
        else
        {
            //stop all other versions of this game object
            Destroy(this.gameObject);
        }
    }

    public void Update()
    {

        if(inGame)
        {
            if (!BallInst.GetComponent<BallController>().Paused)
            {
                igMusic.UnPause();
            }
            else
            {
                igMusic.Pause();
            }
        }
    }
    private void OnLevelWasLoaded(int level)
    {
        //MainMenu
        if (level == 0)
        {
            igMusic.Stop();
            inGame = false;
            //only replay the music loop if coming from Play Game scenes
            if (musicPlaying == false)
            {
                menuMusic.Play();
            }
            Play = GameObject.FindGameObjectWithTag("PlayButton").GetComponent<Button>();
            Play.onClick.AddListener(() => LoadScene(2));
            About = GameObject.FindGameObjectWithTag("AboutButton").GetComponent<Button>();
            About.onClick.AddListener(() => LoadScene(1));
            Quit = GameObject.FindGameObjectWithTag("QuitButton").GetComponent<Button>();
            Quit.onClick.AddListener(() => Game_Quit());       
            Destroy(BallInst);
        }
        //About
        if (level == 1)
        {
            musicPlaying = true;
            Back = GameObject.FindGameObjectWithTag("BackButton").GetComponent<Button>();
            Back.onClick.AddListener(() => LoadScene(0));
        }
        //Pre Game Screen
        if (level == 2)
        {
            musicPlaying = true;
            Start = GameObject.FindGameObjectWithTag("StartButton").GetComponent<Button>();
            Start.onClick.AddListener(() => LoadScene(3));
            Back = GameObject.FindGameObjectWithTag("BackButton").GetComponent<Button>();
            Back.onClick.AddListener(() => LoadScene(0));
        }
        //PlayGame/Level 1
        if (level == 3)
        {
            menuMusic.Stop();
            igMusic.Play();
            inGame = true;
            musicPlaying = false;
            BallInst = GameObject.FindGameObjectWithTag("Player1");
        }
        //Game Over
        if (level == 6)
        {
            Back = GameObject.FindGameObjectWithTag("BackButton").GetComponent<Button>();
            Back.onClick.AddListener(() => LoadScene(0));
            Play = GameObject.FindGameObjectWithTag("PlayButton").GetComponent<Button>();
            Play.onClick.AddListener(() => LoadScene(3));
            Quit = GameObject.FindGameObjectWithTag("QuitButton").GetComponent<Button>();
            Quit.onClick.AddListener(() => Game_Quit());
        }
        //player wins
        if (level == 7)
        {
            //BallInst = GameObject.FindGameObjectWithTag("Player1");
             
            //obtain references to display text boxes for scoreboard
            tc1 = GameObject.FindGameObjectWithTag("tc1").GetComponent<Text>();
            tc2 = GameObject.FindGameObjectWithTag("tc2").GetComponent<Text>();
            tc3 = GameObject.FindGameObjectWithTag("tc3").GetComponent<Text>();
            tct = GameObject.FindGameObjectWithTag("tct").GetComponent<Text>();
            bt1 = GameObject.FindGameObjectWithTag("bt1").GetComponent<Text>();
            bt2 = GameObject.FindGameObjectWithTag("bt2").GetComponent<Text>();
            bt3 = GameObject.FindGameObjectWithTag("bt3").GetComponent<Text>();
            btt = GameObject.FindGameObjectWithTag("btt").GetComponent<Text>();
            f1 = GameObject.FindGameObjectWithTag("f1").GetComponent<Text>();
            f2 = GameObject.FindGameObjectWithTag("f2").GetComponent<Text>();
            f3 = GameObject.FindGameObjectWithTag("f3").GetComponent<Text>();
            ft = GameObject.FindGameObjectWithTag("ft").GetComponent<Text>();
            rank = GameObject.FindGameObjectWithTag("Rank").GetComponent<Text>();

            //print Scores
            tc1.text = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(BallInst.GetComponent<BallController>().lvl1c / 60), Mathf.FloorToInt(BallInst.GetComponent<BallController>().lvl1c % 60));
            tc2.text = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(BallInst.GetComponent<BallController>().lvl2c / 60), Mathf.FloorToInt(BallInst.GetComponent<BallController>().lvl2c % 60));
            tc3.text = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(BallInst.GetComponent<BallController>().lvl3c / 60), Mathf.FloorToInt(BallInst.GetComponent<BallController>().lvl3c % 60));
            tct.text = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(BallInst.GetComponent<BallController>().totalTC / 60), Mathf.FloorToInt(BallInst.GetComponent<BallController>().totalTC % 60));
            bt1.text = "+" + ((int)BallInst.GetComponent<BallController>().fall1 * 3).ToString() + "s";
            bt2.text = "+" + ((int)BallInst.GetComponent<BallController>().fall2 * 3).ToString() + "s";
            bt3.text = "+" + ((int)BallInst.GetComponent<BallController>().fall3 * 3).ToString() + "s";
            btt.text = "+" + ((int)BallInst.GetComponent<BallController>().fallCtr * 3).ToString() + "s";
            f1.text = BallInst.GetComponent<BallController>().fall1.ToString();
            f2.text = BallInst.GetComponent<BallController>().fall2.ToString();
            f3.text = BallInst.GetComponent<BallController>().fall3.ToString();
            ft.text = BallInst.GetComponent<BallController>().fallCtr.ToString();

            //time penalty for rank Calc
            tempTR = (BallInst.GetComponent<BallController>().totalTC + BallInst.GetComponent<BallController>().fallCtr * 3);

            if (tempTR < 120f)
            {
                rank.text = "SSS";
            }
            else if (tempTR < 130f)
            {
                rank.text = "SS";
            }
            else if(tempTR < 140f)
            {
                rank.text = "S";
            }
            else if (tempTR < 165f)
            {
                rank.text = "A";
            }
            else if (tempTR < 200f)
            {
                rank.text = "B";
            }
            else if (tempTR < 230f)
            {
                rank.text = "C";
            }
            else if (tempTR < 250f)
            {
                rank.text = "D";
            }
            else
            {
                rank.text = "F";
            }

            CreditsButton = GameObject.FindGameObjectWithTag("Continue").GetComponent<Button>();
            CreditsButton.onClick.AddListener(() => LoadScene(8));
            Restart = GameObject.FindGameObjectWithTag("Restart").GetComponent<Button>();
            Restart.onClick.AddListener(() => LoadScene(3));
            Destroy(BallInst);
        }
        //credits screen
        if (level == 8)
        {
            igMusic.Stop();
            inGame = false;
            creditsMusic = GameObject.FindGameObjectWithTag("CreditsCanvas").GetComponent<AudioSource>();
            creditsMusic.Play();
            Back = GameObject.FindGameObjectWithTag("BackButton").GetComponent<Button>();
            Back.onClick.AddListener(() => LoadScene(0));
            Destroy(BallInst);
        }
    
    }

    public static void LoadScene(int sceneIndex)
    {
        ButtonPress.Play();
        SceneManager.LoadScene(sceneIndex);
    }
    //Quit application and debug for Unity Editor awareness
    public static void Game_Quit()
    {
        Debug.Log("Quitting game.");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
