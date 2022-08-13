using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spike : MonoBehaviour
{
    // Spike colour infomation
    [Header("Spike Materials")]
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material playerIsInvincibleMaterial;

    private Renderer spikeRenderer;

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

        spikeRenderer = GetComponent<Renderer>();
        spikeRenderer.enabled = true;
        spikeRenderer.sharedMaterial = defaultMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameManager.GameRunning())
        {
            // Move Spike
            gameObject.transform.position += new Vector3(0.0f, (-this.gameManager.GameSpeed() * Time.deltaTime), 0.0f);

            // Should Spike be the defaultMaterial ?
            if (!this.playerManager.PlayerIsInvincible() && spikeRenderer.sharedMaterial != this.defaultMaterial)
            {
                spikeRenderer.sharedMaterial = this.defaultMaterial;
            }

            // Should Spike be the PlayerIsInvincible Material ?
            if (this.playerManager.PlayerIsInvincible() && spikeRenderer.sharedMaterial != this.playerIsInvincibleMaterial)
            {
                spikeRenderer.sharedMaterial = this.playerIsInvincibleMaterial;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "DeathBox")
        {
            this.spikeManager.DestroySpike(this.gameObject);
        }
    }
}