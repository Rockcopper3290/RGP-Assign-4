using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameScreen : MonoBehaviour
{

    // Instructions View
    [Header("Instructions View")]
    [SerializeField] private GameObject instructionsView;

    // Instructions View, Pickups
    [SerializeField] private GameObject coin;
    [SerializeField] private GameObject invincible;
    [SerializeField] private Vector3 coinRotationPerSecond = new Vector3(0.0f, -180.0f, 0.0f);
    [SerializeField] private Vector3 invincibleRotationPerSecond = new Vector3(0.0f, -180.0f, 0.0f);
    [Space(10)]

    // Game View
    [Header("Game View")]
    [SerializeField] private GameObject gameView;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TweenyScore tweenyScore;
    [Space(10)]

    // Pause View
    [Header("Pause View")]
    [SerializeField] private GameObject pauseView;

    // Game Manager
    private GameManager gameManager;
    private PlayerManager playerManager;

    public void SetGameManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
        this.playerManager = gameManager.GetPlayerManager();
    }

    private void ShowInstructionsView()
    {
        this.instructionsView.SetActive(true);
        this.gameView.SetActive(false);
        this.pauseView.SetActive(false);
    }

    private void ShowGameView()
    {
        this.instructionsView.SetActive(false);
        this.gameView.SetActive(true);
        this.pauseView.SetActive(false);
    }

    private void ShowPauseView()
    {
        this.instructionsView.SetActive(false);
        this.gameView.SetActive(false);
        this.pauseView.SetActive(true);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void GameStart()
    {
        if (gameManager.GameRunning())
            return;

        ShowGameView();
    }

    public void GamePause()
    {
        this.gameManager.GamePause();

        ShowPauseView();
    }

    public void GameResume()
    {
        this.gameManager.GameResume();

        ShowGameView();
    }

    public void GameOver()
    {

    }

    public void BoingScore()
    {
        this.tweenyScore.BoingScore();
    }
    
    // Awake is called before any Start methods, even if the GameObject is disabled
    private void Awake()
    {
        ShowInstructionsView();
    }

    // Update is called once per frame
    void Update()
    {
       // Is The Instructions View Active
       if (this.instructionsView.activeSelf == true) 
       {
            this.coin.transform.Rotate(this.coinRotationPerSecond * Time.deltaTime);
            this.invincible.transform.Rotate(this.invincibleRotationPerSecond * Time.deltaTime);

            return;
        }

        if (Input.GetButtonDown("Pause"))
        {
            if (!this.gameManager.GamePaused())
                this.GamePause();
            else
                this.GameResume();
        }

        scoreText.text = gameManager.GameScore().ToString();
    }
}
