using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    // Movement
    [SerializeField] private Vector3 rotatePerSecond = new Vector3(0.0f, 0.0f, 0.0f);

    // Game Manager
    private GameManager gameManager;
    private DifficultyManager difficulty;
    private PickUpManager pickUpManager;

    public void SetGameManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
        this.difficulty = gameManager.GetDifficultyManager();
        this.pickUpManager = gameManager.GetPickUpManager();
    }

    // Start is called on the first frame of script existing
    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position += new Vector3(0.0f, (-this.difficulty.GameSpeed() * Time.deltaTime), 0.0f);
        gameObject.transform.Rotate(this.rotatePerSecond * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "DeathBox")
        {
            this.pickUpManager.DestroyPickUp(this.gameObject);
        }
    }
}
