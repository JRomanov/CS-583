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
    public bool musicPlaying;

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
            //menuMusic = GetComponent<AudioSource>();
            //menuMusic.Play();
            ButtonPress = GameObject.FindGameObjectWithTag("ButtonPress").GetComponent<AudioSource>();
            /*
            musicPlaying = true;
            */
        }
        else
        {
            //stop all other versions of this game object
            Destroy(this.gameObject);
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        //MainMenu
        if (level == 0)
        {
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
            musicPlaying = false;
            BallInst = GameObject.FindGameObjectWithTag("Player1");
        }
        //Level 2
        if (level == 4)
        {

        }
        //Level 3
        if (level == 5)
        {

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
            CreditsButton = GameObject.FindGameObjectWithTag("Continue").GetComponent<Button>();
            CreditsButton.onClick.AddListener(() => LoadScene(8));
            Restart = GameObject.FindGameObjectWithTag("Restart").GetComponent<Button>();
            Restart.onClick.AddListener(() => LoadScene(3));
        }
        //credits screen
        if (level == 8)
        {
            creditsMusic = GameObject.FindGameObjectWithTag("CreditsCanvas").GetComponent<AudioSource>();
            creditsMusic.Play();
            Back = GameObject.FindGameObjectWithTag("BackButton").GetComponent<Button>();
            Back.onClick.AddListener(() => LoadScene(0));
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
