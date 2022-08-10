using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameScreen : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _pauseButton;

    // Set in Unity Inspector
    public TMP_Text scoreText;
    public TMP_Text clickToStartText;
    public TMP_Text instructionsText;
    public TMP_Text coinInfoText;
    public TMP_Text powerupInfoText;

    // PickUps
    public GameObject coin;
    public GameObject invincible;

    public Vector3 coinRotationPerSecond = new Vector3(0.0f, 180.0f, 0.0f);
    public Vector3 invincibleRotationPerSecond = new Vector3(0.0f, 0.0f, 0.0f);

    // Game Manager
    private GameManager gameManager;
    private PlayerManager playerManager;



    public void SetGameManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
        this.playerManager = gameManager.GetPlayerManager();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void GameStart()
    {
        if (gameManager.GameRunning())
            return;

        // Set Pause Menu
        _pauseMenu?.SetActive(false);
        _pauseButton.SetActive(true);

        // Show Score
        scoreText.enabled = true;

        // Hide Instructions
        clickToStartText.enabled = false;
        instructionsText.enabled = false;
        coinInfoText.enabled = false;
        powerupInfoText.enabled = false;

        // Hide Display PickUps
        coin.GetComponent<Renderer>().enabled = false;
        invincible.GetComponent<Renderer>().enabled = false;
    }

    public void GamePause()
    {
        this.gameManager.gamePauseFunction_wasUsed = true;

        this.gameManager.GamePause();

        _pauseMenu.SetActive(true);
        _pauseButton.SetActive(false);
    }

    public void GameResume()
    {
        this.gameManager.GameResume();

        _pauseMenu?.SetActive(false);
        _pauseButton?.SetActive(true);
    }

    public void GameOver()
    {

    }
    
    // Awake is called before any Start methods, even if the GameObject is disabled
    private void Awake()
    {
        // Hide Score
        scoreText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.GameRunning())
        {
            this.coin.transform.Rotate(this.coinRotationPerSecond * Time.deltaTime);
            this.invincible.transform.Rotate(this.invincibleRotationPerSecond * Time.deltaTime);

            return;
        }

        scoreText.text = gameManager.GameScore().ToString();
    }
}
