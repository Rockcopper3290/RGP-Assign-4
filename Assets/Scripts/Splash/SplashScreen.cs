using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SplashScreen: MonoBehaviour
{
    // Set in Unity Inspector
    [SerializeField] private TMP_Text startGameText;
    [SerializeField] private TMP_Text exitButtonText;
    [SerializeField] private Toggle tutorialToggle;
    [Space(10)]

    // Music
    [Header("Music")]
    [SerializeField] private AudioSource splashMusic;

    public void Tutorial()
    {
        //splashMusic.Stop();
        
        SceneManager.LoadScene("Game");
    }

    public void PlayGame()
    {
        //splashMusic.Stop();
        
        GameData.tutorial = this.tutorialToggle.isOn;
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        //splashMusic.Stop();
        
        Application.Quit();
    }

    // Awake is called before any Start methods, even if the GameObject is disabled
    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        this.tutorialToggle.isOn = GameData.tutorial;
        //splashMusic.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }
}

