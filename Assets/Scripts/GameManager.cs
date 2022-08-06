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
    private float gameTime;             // Time the game is running

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
        this.gameScreen.GameStart();
        this.playerManager.GameStart();
        this.spikeManager.GameStart();
        this.pickUpManager.GameStart();

        this.gameRunning = true;
        this.gameTime = 0.0f;

        this.backgroundMusic.Play();
    }

    public void GameOver()
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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!gameRunning)
        {
            if (Input.GetButtonDown("Jump"))
                GameStart();

            return;
        }

        // Increment the Game Time
        this.gameTime += Time.deltaTime;
    }
    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }
    public void QuitToMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void Exit()
    {
        Application.Quit();
    }
       
}
