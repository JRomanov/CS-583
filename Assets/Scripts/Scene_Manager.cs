using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoBehaviour
{
   public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void LevelOne()
    {
        SceneManager.LoadScene(2);
    }

    public void QuitGame()
    {
        Debug.Log("QUITTING");
        Application.Quit();
    }
}
