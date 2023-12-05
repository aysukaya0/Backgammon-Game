using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionController : MonoBehaviour
{
    public void LoadGameScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game_Scene");
    }

    public void LoadMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main_Menu");
    }
}

