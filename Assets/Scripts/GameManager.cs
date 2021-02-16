using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver;

    private void Update()
    {   // Scene management/selection: Scene 0, Main Menu; Scene 1, Game Level; Scene 2, End Credits.
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true)
        {
            SceneManager.LoadScene(1); // Current scene set in build manager
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    public void GameOver()
    {
        _isGameOver = true;
    }
    public void PlayerWins()
    {   
        // load scene 2 for content credit.
        SceneManager.LoadScene(2);
    }
}
