using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class StartMenu: MonoBehaviour
{
    // Set in Unity Inspector
    public TMP_Text startGameText;
    public TMP_Text exitButtonText;
    public AudioSource music;

    // Game Over Screen Management
    private float timeSinceStart;

    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // Awake is called before any Start methods, even if the GameObject is disabled
    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        //music.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }
}

