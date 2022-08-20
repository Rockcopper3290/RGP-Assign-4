using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // Set In Unity Inspector
    [Header("Movement and Behaviour")]
    [SerializeField] private float moveSpeed = 20.0f;
    [SerializeField] private float invincibleTime = 5.0f;
    [SerializeField] private float jumpCooldownTime = 5.0f; // Time between Jumps
    [Space(10)]

    [Header("Shield")]
    [SerializeField] private GameObject shield;
    [Space(10)]

    // Sounds, Set In Unity Inspector
    [Header("Sounds")]
    [SerializeField] private AudioSource coinSound;
    [SerializeField] private AudioSource invincibleSound;
    [SerializeField] private AudioSource shieldSound;
    [SerializeField] private AudioSource spikeDestroySound;
    [SerializeField] private AudioSource invincibleMusic;
    [Space(10)]

    // Colour, Set In Unity Inspector
    [Header("Colours")]
    [SerializeField] private Color defaultColour;
    [SerializeField] private Color jumpCooldownColour;
    [SerializeField] private Color powerUpColour;

    // Player Properties, Movement
    private Vector3 velocity;
    private Vector3 acceleration;
    private bool isGrounded;
    private bool isMovingLeft;
    private float lastJumpTime;

    // Player Properties, Invincible
    private bool isInvincible;
    private float invincibleTimeRemaining;

    // Player Properties, Shielded
    private bool isShielded;

    // Game Manager
    private GameManager gameManager;
    private InputManager inputManager;
    private GameScreen gameScreen;
    private PickUpManager pickUpManager;
    private SpikeManager spikeManager;

    // Components
    private Renderer playerRenderer;

    private void Awake()
    {
        // Components
        this.playerRenderer = this.GetComponent<Renderer>();
    }

    public void SetGameManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
        this.inputManager = gameManager.GetInputManager();
        this.gameScreen = gameManager.GetGameScreen();
        this.pickUpManager = gameManager.GetPickUpManager();
        this.spikeManager = gameManager.GetSpikeManager();
    }
    
    public bool PlayerIsInvincible() 
    {
        return this.isInvincible;
    }

    public bool PlayerIsShielded()
    {
        return this.isShielded;
    }
    
    public void GameStart()
    {
        // Jump cooldown
        this.lastJumpTime = Time.time - this.jumpCooldownTime;

        // Invincible
        this.isInvincible = false;
        this.invincibleTimeRemaining = 0.0f;

        // Shielded
        this.shield.SetActive(false);
        this.isShielded = false;

        // Randomly choose which side to start on
        // Unless the Tutorial is Running, in which case always start on the Right
        if ((Random.Range(0.0f, 1.0f) < 0.5f) || this.gameManager.TutorialRunning())
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Coin")
        {
            this.pickUpManager.DestroyPickUp(collision.gameObject);

            this.gameManager.ScoreCoinPickup();
            this.coinSound.Play();
        }
       
        if (collision.gameObject.tag == "Invincible")
        {
            this.pickUpManager.DestroyPickUp(collision.gameObject);
            this.gameScreen.PulseScore(this.gameScreen.invincibleColor);

            this.invincibleTimeRemaining = this.invincibleTime;
            this.invincibleSound.Play();

            if (!this.isInvincible)
            {
                this.isInvincible = true;

                this.invincibleMusic.PlayDelayed(this.invincibleSound.clip.length * 0.5f);
                this.gameManager.BackgroundMusicPause();
            }

            // Shield is lost when you become Invincible
            if (this.isShielded)
            {
                this.isShielded = false;
                this.shield.SetActive(false);
            }
        }

        if (collision.gameObject.tag == "Shield")
        {
            this.shieldSound.Play();
            this.gameScreen.PulseScore(this.gameScreen.shieldColor);

            this.pickUpManager.DestroyPickUp(collision.gameObject);

            this.isShielded = true;
            this.shield.SetActive(true);

            // Invincibility is lost when you get a Shield
            if (this.isInvincible)
            {
                this.invincibleTimeRemaining = 0.0f;
            }
        }

        if ((collision.gameObject.tag == "Left Spike") ||
            (collision.gameObject.tag == "Right Spike") )
        {
            if (this.isInvincible)
            {
                this.gameManager.ScoreSpike(this.gameScreen.spikeWhileInvincibleColor);
                spikeDestroySound.Play();
                this.spikeManager.DestroySpike(collision.gameObject);
            }
            else if (this.isShielded)
            {  
                this.gameManager.ScoreSpike(this.gameScreen.spikeColor);
                spikeDestroySound.Play();
                this.spikeManager.DestroySpike(collision.gameObject);

                this.isShielded = false;
                this.shield.SetActive(false);
            }
            else
            {
                this.gameManager.GameOver();
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
                if (this.inputManager.GetButtonDown("Jump"))
                {
                    velocity = new Vector3(this.moveSpeed, 0.0f, 0.0f);
                    acceleration = Vector3.zero;
                    isGrounded = false;
                    isMovingLeft = false;
                }
            }
            else
            {
                // Moving off Right Wall, Do We Get Go Back Click ?
                if (this.inputManager.GetButtonDown("Jump") &&
                    (transform.position.x >= -2.0f) &&
                    (acceleration == Vector3.zero) &&
                    (Time.time >= (lastJumpTime + this.jumpCooldownTime)) )
                {
                    // Go Back to Right Wall and initiate cooldown
                    lastJumpTime = Time.time;
                    acceleration = new Vector3(this.moveSpeed * 0.1f, 0.0f, 0.0f);
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
                    velocity = Vector3.ClampMagnitude(velocity + acceleration, this.moveSpeed);
                    Vector3 newPosition = transform.position + (this.velocity * Time.deltaTime);
                    transform.position = new Vector3(Mathf.Clamp(newPosition.x, -3.0f, 3.0f), newPosition.y, newPosition.z);

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
                if (this.inputManager.GetButtonDown("Jump"))
                {
                    velocity = new Vector3(-this.moveSpeed, 0.0f, 0.0f);
                    acceleration = Vector3.zero;
                    isGrounded = false;
                    isMovingLeft = true;
                }
            }
            else
            {
                // Moving off Left Wall, Do We Get Go Back Click ?
                if (this.inputManager.GetButtonDown("Jump") &&
                    (transform.position.x <= 2.0f) &&
                    (acceleration == Vector3.zero) &&
                    (Time.time >= (lastJumpTime + this.jumpCooldownTime)) )
                    {
                        // Go Back to Left Wall and intiate cooldown
                        lastJumpTime = Time.time;
                        acceleration = new Vector3(-this.moveSpeed * 0.1f, 0.0f, 0.0f);
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
                        velocity = Vector3.ClampMagnitude(velocity + acceleration, this.moveSpeed);
                        Vector3 newPosition = transform.position + (this.velocity * Time.deltaTime);
                        transform.position = new Vector3(Mathf.Clamp(newPosition.x, -3.0f, 3.0f), newPosition.y, newPosition.z);

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
        else if (this.isShielded)
        {

        }
        else
        {
            if (Time.time < (lastJumpTime + this.jumpCooldownTime))
                this.playerRenderer.material.color = jumpCooldownColour;
            else
                this.playerRenderer.material.color = defaultColour;
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