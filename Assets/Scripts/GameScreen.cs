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
    [SerializeField] private GameObject shield;
    [SerializeField] private GameObject invincible;
    [SerializeField] private Vector3 coinRotationPerSecond = new Vector3(0.0f, -180.0f, 0.0f);
    [SerializeField] private Vector3 shieldRotationPerSecond = new Vector3(0.0f, -180.0f, 0.0f);
    [SerializeField] private Vector3 invincibleRotationPerSecond = new Vector3(0.0f, -180.0f, 0.0f);
    [Space(10)]

    // Game View
    [Header("Game View")]
    [SerializeField] private GameObject gameView;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private PulseScale pulseScore;
    [Space(10)]

    // Pause View
    [Header("Pause View")]
    [SerializeField] private GameObject pauseView;

    // Game Manager
    private GameManager gameManager;
    private InputManager inputManager;
    private PlayerManager playerManager;

    public void SetGameManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
        this.inputManager = gameManager.GetInputManager();
        this.playerManager = gameManager.GetPlayerManager();
    }

    public void ShowInstructionsView()
    {
        this.instructionsView.SetActive(true);
        this.gameView.SetActive(false);
        this.pauseView.SetActive(false);
    }

    public void ShowGameView()
    {
        this.instructionsView.SetActive(false);
        this.gameView.SetActive(true);
        this.pauseView.SetActive(false);
    }

    public void ShowPauseView()
    {
        this.instructionsView.SetActive(false);
        this.gameView.SetActive(false);
        this.pauseView.SetActive(true);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Splash");
    }

    public void GameStart()
    {
        if (gameManager.GameRunning())
            return;

        ShowGameView();
    }

    public void GameOver()
    {

    }

    public void PulseScore()
    {
        this.pulseScore.Pulse();
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
            this.shield.transform.Rotate(this.shieldRotationPerSecond * Time.deltaTime);
            this.invincible.transform.Rotate(this.invincibleRotationPerSecond * Time.deltaTime);

            return;
        }

        if (this.inputManager.GetButtonDown("Pause"))
        {
            if (!this.gameManager.GamePaused())
                this.gameManager.GamePause();
            else
                this.gameManager.GameResume();
        }

        scoreText.text = gameManager.GameScore().ToString();
    }
}
