using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // Set In Unity Inspector
    public float moveSpeed = 20.0f;
    public float invincibleTime = 5.0f;

    // Sounds, Set In Unity Inspector
    public AudioSource coinSound;
    public AudioSource invincibleSound;
    public AudioSource spikeDestroySound;
    public AudioSource invincibleMusic;

    // Colour, Set In Unity Inspector
    public Color defaultColour;
    public Color powerUpColour;

    // Double Click Detection
    private float moveOffWallTime;
    private float doubleClickDelay = 0.15f;

    // Player Properties, Movement
    private Vector3 velocity;
    private Vector3 acceleration;
    private bool isGrounded;
    private bool isMovingLeft;

    // Player Properties, Invincible
    private bool isInvincible;
    private float invincibleTimeRemaining;

    // Score
    private const int coinValue = 10;
    private const int spikeValue = 5;

    private int coinScore;
    private int spikeScore;

    // Game Manager
    private GameManager gameManager;
    private GameScreen gameScreen;
    private PickUpManager pickUpManager;

    // Components
    private Renderer playerRenderer;

    private void Awake()
    {
        // Components
        this.playerRenderer = this.GetComponent<Renderer>();

        // minScale = transform.localScale;
    }

    public void SetGameManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
        this.gameScreen = gameManager.GetGameScreen();
        this.pickUpManager = gameManager.GetPickUpManager();

    }
    
    public bool PlayerIsInvincible() 
    {
        return this.isInvincible;
    }
    
    public int Score()
    {
        return this.coinScore + this.spikeScore;
    }

    public void GameStart()
    {
        // Invincible
        this.isInvincible = false;
        this.invincibleTimeRemaining = 0.0f;

        // Score
        this.coinScore = 0;
        this.spikeScore = 0;

        // Randomly choose which side to start on
        if (Random.Range(0.0f, 1.0f) < 0.5f)
        {
            // Initial Move Right
            this.velocity = new Vector3(this.moveSpeed, 0.0f, 0.0f);
            this.isMovingLeft = false;
            this.isGrounded = false;
        }
        else
        {
            // Initial Move Left
            this.velocity = new Vector3(-this.moveSpeed, 0.0f, 0.0f);
            this.isMovingLeft = true;
            this.isGrounded = false;
        }
    }

    public void GameOver()
    {

    }

    private void OnHitCoin(Collision collision)
    {
        this.pickUpManager.DestroyPickUp(collision.gameObject);

        this.coinScore += PlayerManager.coinValue;
        this.gameScreen.BoingScore();
        this.coinSound.Play();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Coin")
        {
            OnHitCoin(collision);
        }
       
        if (collision.gameObject.tag == "Invincible")
        {
            this.pickUpManager.DestroyPickUp(collision.gameObject);

            this.invincibleTimeRemaining = this.invincibleTime;
            this.invincibleSound.Play();


            if (!this.isInvincible)
            {
                this.isInvincible = true;

                this.invincibleMusic.PlayDelayed(this.invincibleSound.clip.length * 0.5f);
                this.gameManager.BackgroundMusicPause();
            }
        }

        if (this.isInvincible)
        {
            if ((collision.gameObject.tag == "Left Spike") ||
                (collision.gameObject.tag == "Right Spike") )
            {
                this.spikeScore += PlayerManager.spikeValue;
                this.gameScreen.BoingScore();
                spikeDestroySound.Play();
            }
        }
    }

    private void UpdateMovement()
    {
        if (isMovingLeft)
        {
            if (this.isGrounded)
            {
                // On Left Wall, does the Player leave the Left Wall
                if (Input.GetButtonDown("Jump"))
                {
                    moveOffWallTime = Time.time;

                    velocity = new Vector3(this.moveSpeed, 0.0f, 0.0f);
                    acceleration = Vector3.zero;
                    isGrounded = false;
                    isMovingLeft = false;
                }
            }
            else
            {
                // Moving off Right Wall, Do We Get Double Click ?
                if (Input.GetButtonDown("Jump") &&
                    ((this.moveOffWallTime - Time.time) < this.doubleClickDelay) &&
                    (acceleration == Vector3.zero))
                {
                    // Jump over Spike, on Right Wall
                    acceleration = new Vector3(this.moveSpeed * 0.125f, 0.0f, 0.0f);
                    isMovingLeft = false;
                }

                // Has the Player reached the Left Wall
                if (transform.position.x <= -3.0f)
                {
                    // Fix Player position to the Left Wall
                    transform.position = new Vector3(-3.0f, 0.0f, 0.0f);
                    velocity = Vector3.zero;
                    acceleration = Vector3.zero;
                    isGrounded = true;
                }
                // The Player is moving Left
                else
                {
                    velocity += acceleration;
                    Vector3 newPosition = transform.position + (this.velocity * Time.deltaTime);
                    transform.position = new Vector3(Mathf.Max(-3.0f, newPosition.x), newPosition.y, newPosition.z);

                    // Rotate as we move
                    float xPosition = transform.position.x;
                    float zAngle = Mathf.Lerp(90, -90, ((xPosition + 3.0f) / 6.0f));
                    transform.eulerAngles = new Vector3(0.0f, 0.0f, zAngle);
                }
            }
        }
        // Moving Right
        else
        {
            if (this.isGrounded)
            {
                // On Right Wall, does the Player leave the Right Wall
                if (Input.GetButtonDown("Jump"))
                {
                    this.moveOffWallTime = Time.time;

                    velocity = new Vector3(-this.moveSpeed, 0.0f, 0.0f);
                    acceleration = Vector3.zero;
                    isGrounded = false;
                    isMovingLeft = true;
                }
            }
            else
            {
                // Moving off Left Wall, Do We Get Double Click ?
                if (Input.GetButtonDown("Jump") &&
                    ((this.moveOffWallTime - Time.time) < this.doubleClickDelay) &&
                    (acceleration == Vector3.zero))
                {
                    // Jump over Spike, on Left Wall
                    acceleration = new Vector3(-this.moveSpeed * 0.125f, 0.0f, 0.0f);
                    isMovingLeft = true;
                }

                // Has the Player reached the Right Wall
                if (transform.position.x >= 3.0f)
                {
                    // Fix Player position to the Right Wall
                    transform.position = new Vector3(3.0f, 0.0f, 0.0f);
                    velocity = Vector3.zero;
                    acceleration = Vector3.zero;
                    isGrounded = true;
                }
                // The Player is moving Right
                else
                {
                    velocity += acceleration;
                    Vector3 newPosition = transform.position + (this.velocity * Time.deltaTime);
                    transform.position = new Vector3(Mathf.Min(3.0f, newPosition.x), newPosition.y, newPosition.z);

                    // Rotate as we move
                    float xPosition = transform.position.x;
                    float zAngle = Mathf.Lerp(90, -90, ((xPosition + 3.0f) / 6.0f));
                    transform.eulerAngles = new Vector3(0.0f, 0.0f, zAngle);
                }
            }
        }
    }

    private void UpdateSprite()
    {
        if (this.isInvincible)
        {
            this.invincibleTimeRemaining -= Time.deltaTime;
            if (this.invincibleTimeRemaining <= 0.0f)
            {
                this.isInvincible = false;
                this.playerRenderer.material.color = defaultColour;

                this.invincibleMusic.Stop();
                this.gameManager.BackgroundMusicUnPause();

                return;
            }

            float rate;

            switch ((int)Mathf.Floor((this.invincibleTimeRemaining / this.invincibleTime) * 5))
            {
                default:
                case 4: rate =  1.0f; break;
                case 3: rate =  2.0f; break;
                case 2: rate =  4.0f; break;
                case 1: rate =  8.0f; break;
                case 0: rate = 16.0f; break;
            }
   
            this.playerRenderer.material.color =
                Color.Lerp(this.defaultColour, this.powerUpColour, Mathf.PingPong(Time.time * rate, 1.0f));
        }
    }

    void Update()
    {
        if (!gameManager.GameRunning())
            return;

        if (gameManager.GamePaused())
            return;

        UpdateMovement();
        UpdateSprite();
    }
}