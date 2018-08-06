using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public enum GameState
{
    Menu, Pause, Playing, GameOver
}

public class GameManager : MonoBehaviour
{

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GoToMainMenu();
        }
    }

    public static GameState state = GameState.Playing;
    public static int currentLevel = 0;

    public static void GameOver()
    {
        if (state != GameState.GameOver)
        {
            state = GameState.GameOver;
            GameObject obj = GameObject.FindGameObjectWithTag("GameOverScreen");
            obj.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public void _StartGame()
    {
        StartGame();
    }

    public static void StartGame()
    {
        state = GameState.Playing;
        currentLevel = 1;
        StartLevel(currentLevel);
    }

    public void _StartLevel(int level)
    {
        StartLevel(level);
    }

    public static void StartLevel(int level)
    {
        currentLevel = level;
        state = GameState.Playing;
        SceneManager.LoadScene("Level" + level);
    }

    public static void PauseGame()
    {
        state = GameState.Pause;
    }

    public static void NextLevel()
    {
        currentLevel++;
        StartLevel(currentLevel);
    }

    public static void UnPause()
    {
        state = GameState.Playing;
    }

    public static void GoToMainMenu()
    {
        state = GameState.Menu;
        SceneManager.LoadScene("MainMenu");
    }

    public static GameManager Instance;

    void Awake()
    {
        Instance = this;
    }
}

