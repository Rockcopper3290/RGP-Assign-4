using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    // Set in Unity Inspector
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text highScoreText;
    //[SerializeField] private TMP_Text clickToReplayText;
    [SerializeField] private AudioSource music;

    // Game Over Screen Management
    //private float timeSinceStart;

    public void PlayGameNoTutorial()
    {
        //splashMusic.Stop();

        GameData.tutorial = false;
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Splash");
    }

    // Awake is called before any Start methods, even if the GameObject is disabled
    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        this.scoreText.text = "SCORE: " + GameOverData.score.ToString();
        this.highScoreText.text = "HIGH SCORE: " + GameOverData.highScore.ToString();
        // this.clickToReplayText.text = "";

        music.Play();

        //this.timeSinceStart = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Increment Time Since Screen Start
        //this.timeSinceStart += Time.deltaTime;

        //if (this.timeSinceStart >= 1.0f)
        //{
        //    this.clickToReplayText.text = "CLICK TO REPLAY";

        //    if (Input.anyKey || Input.anyKeyDown)
        //        SceneManager.LoadScene("Game");
        //}
    }
}
