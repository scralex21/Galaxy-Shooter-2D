using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver = false;

    private bool _isPaused;
    private UIManager _uIManager;

    private void Start()
    {
        _uIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uIManager == null)
        {
            Debug.Log("The UI Manager is NULL");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true)
        {
            SceneManager.LoadScene(1); // Game Scene
        }

        if (Input.GetKeyDown(KeyCode.Q) && _isGameOver == true)
        {
            SceneManager.LoadScene(0); // Main Menu
        }

        //Pause System
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (_isPaused == false)
            {
                Time.timeScale = 0;
                _uIManager.PauseMenuOn();
                _isPaused = true;
            }

            else
            {
                Time.timeScale = 1;
                _uIManager.PauseMenuOff();
                _isPaused = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Q) && _isPaused == true)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(0); // Main Menu
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void GameOver()
    {
        _isGameOver = true;
    }
}
