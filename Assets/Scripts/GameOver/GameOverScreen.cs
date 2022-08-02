using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    // Set in Unity Inspector
    public TMP_Text scoreText;
    public TMP_Text highScoreText;
    public AudioSource music;

    //public TMP_Text finishText;
    public TMP_Text clickToReplayText;

    // Game Over Screen Management
    private float timeSinceStart;

    public void PlayGame()
    {
        Debug.Log("Play Game");
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    //public void LoadMenu()
    //{
    //    Debug.Log("Load Menu");
    //    SceneManager.LoadScene("Menu");
    //}

    // Awake is called before any Start methods, even if the GameObject is disabled
    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        //this.timeSinceStart = 0.0f;
        music.Play();
        this.scoreText.text = "Score: " + GameOverData.score.ToString();
        this.highScoreText.text = "High Score: " + GameOverData.highScore.ToString();
        this.clickToReplayText.text = "";

    }

    // Update is called once per frame
    void Update()
    {
        // Increment Time Since Screen Start
        this.timeSinceStart += Time.deltaTime;

        if (this.timeSinceStart >= 1.0)
        {
            this.clickToReplayText.text = "Click to Replay";

            if (Input.anyKey || Input.anyKeyDown)
                SceneManager.LoadScene("Game");
        }
    }
}
