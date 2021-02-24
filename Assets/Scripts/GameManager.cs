using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    /*
    [SerializeField]
    private bool _isGameOver;
    */
    private void Update()
    {   
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    public void PlayerWins()
    {   
        SceneManager.LoadScene(2);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(1);
    }
}
