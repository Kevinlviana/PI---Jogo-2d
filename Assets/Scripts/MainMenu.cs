using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button playButton;


    void Start()
    {


        if (playButton != null)
            playButton.onClick.AddListener(() => SceneManager.LoadScene("Level1"));
    }

    public void QuitGame()
    {
        Application.Quit();
    }


}