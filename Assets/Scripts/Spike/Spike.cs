using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spike : MonoBehaviour
{
    // Game Manager
    private GameManager gameManager;
    private SpikeManager spikeManager;
    private PlayerManager playerManager;

    public void SetGameManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
        this.spikeManager = gameManager.GetSpikeManager();
        this.playerManager = gameManager.GetPlayerManager();
    }

    public void Initialise(string spikeType, Vector3 position)
    {
        this.gameObject.tag = spikeType;
        this.gameObject.transform.position = position;
    }

    // Start is called on the first frame of script existing
    private void Start()
    {
        if (gameObject.tag == "Right Spike")
        {
            gameObject.transform.eulerAngles = new Vector3(270, 90, 0);
        }
        else if (gameObject.tag == "Left Spike")
        {
            gameObject.transform.eulerAngles = new Vector3(90, 90, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position += new Vector3(0.0f, (-this.gameManager.GameSpeed() * Time.deltaTime), 0.0f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (this.playerManager.PlayerIsInvincible())
            {
                this.spikeManager.DestroySpike(this.gameObject);
                return;
            }

            this.gameManager.GameOver();
        }

        if (collision.gameObject.tag == "DeathBox")
        {
            this.spikeManager.DestroySpike(this.gameObject);
        }
    }
}