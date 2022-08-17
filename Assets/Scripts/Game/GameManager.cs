using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // References Objects and Scripts
    [Header("Objects and Scripts")]
    [SerializeField] private DifficultyManager difficulty;
    [SerializeField] private GameObject gameScreenUI;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private SpikeManager spikeManager;
    [SerializeField] private PickUpManager pickUpManager;
    [Space(10)]

    private GameScreen gameScreen;

    // Music
    [Header("Music")]
    [SerializeField] private AudioSource backgroundMusic;

    // Score
    private const int coinValue = 10;
    private const int spikeValue = 5;

    private int coinScore;
    private int spikeScore;

    // Game Status
    private bool gameRunning = false;   // Is the game running
    private bool gamePaused = false;    // Is the game paused
    private float gameTime;             // Time the game is running

    private void Awake() {
        this.gameScreen = gameScreenUI.GetComponent<GameScreen>();

        this.inputManager.SetGameManager(this);
        this.difficulty.SetGameManager(this);
        this.gameScreen.SetGameManager(this);
        this.playerManager.SetGameManager(this);
        this.spikeManager.SetGameManager(this);
        this.pickUpManager.SetGameManager(this);
    }

    public DifficultyManager GetDifficultyManager()
    {
        return this.difficulty;
    }

    public GameScreen GetGameScreen()
    {
        return this.gameScreen;
    }

    public PlayerManager GetPlayerManager()
    {
        return this.playerManager;
    }

    public InputManager GetInputManager()
    {
        return this.inputManager;
    }

    public SpikeManager GetSpikeManager()
    {
        return this.spikeManager;
    }

    public PickUpManager GetPickUpManager()
    {
        return this.pickUpManager;
    }

    public void BackgroundMusicPause()
    {
        this.backgroundMusic.Pause();
    }

    public void BackgroundMusicUnPause()
    {
        this.backgroundMusic.UnPause();
    }

    public bool GameRunning()
    {
        return this.gameRunning;
    }

    public bool GamePaused()
    {
        return this.gamePaused;
    }

    public float GameTime()
    {
        return this.gameTime;
    }

    public int GameScore()
    {
        return (int)Mathf.Floor(this.gameTime) + (this.coinScore + this.spikeScore);
    }

    public void ScoreCoinPickup()
    {
        this.coinScore += GameManager.coinValue;
        this.gameScreen.PulseScore(this.gameScreen.coinColor);
    }

    public void ScoreSpike(Color32 color)
    {
        this.spikeScore += GameManager.spikeValue;
        this.gameScreen.PulseScore(color);
    }

    public void GameStart()
    {
        if (!this.gameRunning)
        {
            // Ensure Time is Running
            Time.timeScale = 1.0f;

            // Ensure AudioListener is Running
            AudioListener.pause = false;

            // Score
            this.coinScore = 0;
            this.spikeScore = 0;

            this.inputManager.GameStart();
            this.difficulty.GameStart();
            this.gameScreen.GameStart();
            this.playerManager.GameStart();
            this.spikeManager.GameStart();
            this.pickUpManager.GameStart();

            this.gameRunning = true;
            this.gameTime = 0.0f;

            this.backgroundMusic.Play();
        }
    }

    public void GamePause()
    {
        if (this.gameRunning)
        {
            this.gamePaused = true;
            this.gameScreen.ShowPauseView();

            Time.timeScale = 0.0f;
            AudioListener.pause = true;
        }
    }

    public void GameResume()
    {
        if (this.gameRunning)
        {
            this.gamePaused = false;
            this.gameScreen.ShowGameView();

            Time.timeScale = 1.0f;
            AudioListener.pause = false;
        }
    }

    public void GameOver()
    {
        if (this.gameRunning)
        {
            this.gameRunning = false;

            this.inputManager.GameOver();
            this.difficulty.GameOver();
            this.gameScreen.GameOver();
            this.playerManager.GameOver();
            this.spikeManager.GameOver();
            this.pickUpManager.GameOver();

            this.backgroundMusic.Stop();

            // Update High Score
            if (GameScore() > PlayerPrefs.GetInt("highscore"))
                PlayerPrefs.SetInt("highscore", GameScore());

            // Lauch Game Over Scene
            GameOverData.score = GameScore();
            GameOverData.highScore = PlayerPrefs.GetInt("highscore");
            SceneManager.LoadScene("GameOver");
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {       
        if (!this.gameRunning)
        {
            if (this.inputManager.GetButtonDown("Jump"))
                GameStart();

            return;
        }

        if (!this.gamePaused)
        {
            // Increment the Game Time
            this.gameTime += Time.deltaTime;
        }
    }
  
}
