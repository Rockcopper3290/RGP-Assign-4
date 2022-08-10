using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Set in Unity Inspector
    public GameObject gameScreenUI;
    public GameObject SNEKBox;
    public SpikeManager spikeManager;
    public PickUpManager pickUpManager;

    private GameScreen gameScreen;
    private PlayerManager playerManager;

    // Movement Speed
    public float initialSpeed = 10.0f;
    public float speedIncreasePerSec = 0.05f;
    public float maxSpeed = 20.0f;

    // Background Music
    public AudioSource backgroundMusic;

    // Game Status
    private bool gameRunning = false;   // Is the game running
    private bool gamePaused = false;    // Is the game paused
    private float gameTime;             // Time the game is running

    public bool gamePauseFunction_wasUsed = false;
    public bool playerMovementIsEnabled = true;


    private void Awake() {
        this.gameScreen = gameScreenUI.GetComponent<GameScreen>();
        this.playerManager = SNEKBox.GetComponent<PlayerManager>();

        this.gameScreen.SetGameManager(this);
        this.playerManager.SetGameManager(this);
        this.spikeManager.SetGameManager(this);
        this.pickUpManager.SetGameManager(this);
    }


    public PlayerManager GetPlayerManager()
    {
        return this.playerManager;
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
        return (int)Mathf.Floor(this.gameTime) + this.playerManager.Score();
    }

    public float GameSpeed()
    {
        return (Mathf.Min(this.initialSpeed + (GameTime() * this.speedIncreasePerSec), this.maxSpeed));
    }

    public void GameStart()
    {
        if (!this.gameRunning)
        {
            // Ensure Time is Running
            Time.timeScale = 1.0f;

            // Ensure AudioListener is Running
            AudioListener.pause = false;

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
            playerMovementIsEnabled = false;
            this.gamePaused = true;

            Time.timeScale = 0.0f;
            AudioListener.pause = true;
        }
    }

    public void GameResume()
    {
        if (this.gameRunning)
        {
            playerMovementIsEnabled = true;
            gamePauseFunction_wasUsed = false;

            this.gamePaused = false;

            Time.timeScale = 1.0f;
            AudioListener.pause = false;
        }
    }

    public void GameOver()
    {
        if (this.gameRunning)
        {
            this.gameRunning = false;

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
        //check to see if a pause button was pressed or if the on screen button was clicked
        if (this.gamePauseFunction_wasUsed == true || Input.GetButtonDown("Pause"))
        {
            playerMovementIsEnabled = false;
            gameScreen.GamePause();
        }
        
        
        if (!this.gameRunning)
        {
            if (Input.GetButtonDown("Jump"))
                GameStart();

            return;
        }

        //If game is not paused then accept player input
        if (!this.gamePaused)
        {
            if (playerMovementIsEnabled)
            {
                this.playerManager.UpdateMovement();
                this.playerManager.UpdateSprite();
            }

            

            // Increment the Game Time
            this.gameTime += Time.deltaTime;
        }
    }
  
}
