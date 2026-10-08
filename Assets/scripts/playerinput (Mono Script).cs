using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 10.0f;
    public float rotationSpeed = 720f;

    [Header("Timer Settings")]
    public float timeRemaining = 60f;
    public bool isTimerRunning = true;

    [Header("UI Elements")]
    public TMP_Text countText;
    public TMP_Text wintext;
    public TMP_Text timerText;

    private Rigidbody rb;
    private Animator animator;
    private int count = 0;
    private float movementX;
    private float movementY;
    private bool hasIsWalkingParam = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        // Check if 'isWalking' exists in the Animator to avoid warning spam
        if (animator != null)
        {
            foreach (AnimatorControllerParameter param in animator.parameters)
            {
                if (param.name == "isWalking")
                {
                    hasIsWalkingParam = true;
                    break;
                }
            }
        }

        SetCountText();
        DisplayTime(timeRemaining);

        if (wintext != null)
        {
            wintext.gameObject.SetActive(false);
        }
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        if (movement.magnitude > 0.1f)
        {
            // Move player position smoothly across the ground
            rb.MovePosition(transform.position + movement * speed * Time.fixedDeltaTime);

            // Turn creature face towards movement direction
            Quaternion targetRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

            // Trigger walk animation if parameter exists
            if (hasIsWalkingParam) 
            {
                animator.SetBool("isWalking", true);
            }
        }
        else
        {
            // Trigger idle animation if parameter exists
            if (hasIsWalkingParam) 
            {
                animator.SetBool("isWalking", false);
            }
        }
    }

    void Update()
    {
        if (isTimerRunning)
        {
            if (count >= 13)
            {
                isTimerRunning = false;
                WinGame();
            }
            else if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                isTimerRunning = false;
                GameOver();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("pickup 1"))
        {
            other.gameObject.SetActive(false);
            count++;
            SetCountText();
        }
    }

    void SetCountText()
    {
        if (countText != null)
        {
            countText.text = "Count: " + count.ToString();
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        if (timerText != null)
        {
            int seconds = Mathf.CeilToInt(timeToDisplay);
            timerText.text = "Time Left: " + seconds.ToString();
        }
    }

    void WinGame()
    {
        if (wintext != null)
        {
            wintext.gameObject.SetActive(true);
        }
        Debug.Log("You Won!");
    }

    void GameOver()
    {
        if (timerText != null)
        {
            timerText.text = "Game Over!";
        }
        Debug.Log("Game Over!");

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}