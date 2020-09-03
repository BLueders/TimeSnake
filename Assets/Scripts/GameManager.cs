using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public enum GameState
{
    Menu, Pause, Playing, GameOver, LevelComplete
}

public class GameManager : MonoBehaviour
{
    public GameObject gameOverScreen;
    public GameObject levelCompleteScreen;
    public GameState startState = GameState.Playing;

    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GameManager>();
            }
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }

    void Awake()
    {
        Instance = this;
        Instance.gameOverScreen.SetActive(false);
        Instance.levelCompleteScreen.SetActive(false);
        state = startState;
        string currentLevelName = SceneManager.GetActiveScene().name;
        
        if (currentLevelName.Contains("Level")) {
            currentLevel = int.Parse(currentLevelName[currentLevelName.Length - 1].ToString());
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (state)
            {
                case GameState.Menu:
                    break;
                case GameState.Pause:
                    UnPause();
                    break;
                case GameState.Playing:
                    PauseGame();
                    break;
                case GameState.GameOver:
                    RestartLevel();
                    break;
                case GameState.LevelComplete:
                    RestartLevel();
                    break;
            }
        }
        if (Input.GetKeyDown(KeyCode.Return)) {
            switch (state)
            {
                case GameState.Menu:
                    break;
                case GameState.Pause:
                    RestartLevel();
                    break;
                case GameState.Playing:
                    RestartLevel();
                    break;
                case GameState.GameOver:
                    RestartLevel();
                    break;
                case GameState.LevelComplete:
                    NextLevel();
                    break;
            }
        }
    }

    public static GameState state = GameState.Playing;
    public static int currentLevel = 0;

    public static void GameOver()
    {
        if (state != GameState.GameOver)
        {
            state = GameState.GameOver;
            Instance.gameOverScreen.SetActive(true);
            Instance.levelCompleteScreen.SetActive(false);
        }
    }

    public static void CheckWinCondition() {
        Treat[] remaining = GameObject.FindObjectsOfType<Treat>();
        if (remaining.Length == 1)
        {
            GameManager.LevelComplete();
        }
    }

    public static void LevelComplete()
    {
        if (state != GameState.LevelComplete && state != GameState.GameOver)
        {
            state = GameState.LevelComplete;
            Instance.levelCompleteScreen.SetActive(true);
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

    public static void RestartLevel() {
        if (currentLevel != 0) {
            StartLevel(currentLevel);
        }
    }

    public static void PauseGame()
    {
        state = GameState.Pause;
    }

    public static void NextLevel()
    {
        currentLevel++;
        if (currentLevel <= 10)
            StartLevel(currentLevel);
        else {
            GoToMainMenu();
        }
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
}

