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
    [SerializeField] private AudioSource music;

    public void PlayGameNoTutorial()
    {
        music.Stop();

        GameData.tutorial = false;
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        music.Stop();

        Application.Quit();
    }

    public void MainMenu()
    {
        music.Stop();

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

        music.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
