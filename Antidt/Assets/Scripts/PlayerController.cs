using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpSpeed = 8f;
    private float direction = 0f;
    private Rigidbody2D player;

    public UnityEngine.Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    public bool isTouchingGround;
    private Vector3 respawnPoint;
    public GameObject fallDetector;
    public Text scoreText;
    public HealthBar healthBar;
    [SerializeField] private AudioSource jumpeff;
    [SerializeField] private AudioSource collecteff;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        respawnPoint = transform.position;
        scoreText.text = "SCORE: "+Scoring.totalScore;
    }

    // Update is called once per frame
    void Update()
    {
        isTouchingGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        direction = Input.GetAxis("Horizontal");
        if (direction > 0f)
        {
            player.velocity = new Vector2(direction*speed, player.velocity.y);
            transform.localScale = new Vector2(0.4459955f, 0.4459955f);
        }
        else if (direction < 0f)
        {
            player.velocity = new Vector2(direction * speed, player.velocity.y);
            transform.localScale = new Vector2(-0.4459955f, 0.4459955f);
        }
        else
        {
            player.velocity = new Vector2(0, player.velocity.y);
        }

        if (Input.GetButtonDown("Jump") && isTouchingGround)
        {
            jumpeff.Play();
            player.velocity = new Vector2(player.velocity.x, jumpSpeed);
        }

        fallDetector.transform.position = new Vector2(transform.position.x, fallDetector.transform.position.y);
        if (Health.totalHealth == 0f)
        {
            Health.totalHealth = 1f;
            transform.position = respawnPoint;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "FallDetector")
        {
            Health.totalHealth = 1f;
            transform.position = respawnPoint;
        }

        else if (collision.tag == "Checkpoint")
        {
            respawnPoint = transform.position;
        }

        else if (collision.tag == "PreviousLevel")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1);
            respawnPoint = transform.position;
        }

        else if (collision.tag == "NextLevel")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            respawnPoint = transform.position;
        }

        else if(collision.tag=="Antidote")
        {
            collecteff.Play();
            Scoring.totalScore += 1;
            scoreText.text = "SCORE: " + Scoring.totalScore;
            collision.gameObject.SetActive(false);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Spike")
        {
            healthBar.Damage(0.002f);
        }
    }
}
