using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This script controls the player's horizontal (left/right) movement across three lanes.
/// It uses the old Unity Input Manager for keyboard input.
/// The player smoothly moves to the target lane position over time.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("The speed at which the player slides between lanes.")]
    public float moveSpeed = 10.0f;

    [Tooltip("The distance from the center to the left and right lanes.")]
    public float laneDistance = 3.5f;

    // -1 for left lane, 0 for middle, 1 for right lane.
    public int HorizontalPositionIndex = 0;
    public int VerticalPositionIndex = 0;

    // This will be the position we are smoothly moving towards.
    public Vector3 targetPosition;

    public GameManager GameManager;
    public Animator Animator;

    public float JumpForce = 8.0f;
    public float TorqueForce = 10.0f;

    public bool IsRolling;
    public float RollingTime;
    private float _rollingTimeLeft;
    public float TimeSicneRollStarted;

    public bool IsFlying;
    public bool IsInDoubleJump;

    public bool IsComboRoll;
    public bool DidComboRollAlreadyHitTheGround;

    public float TimeSinceJumpRequested; // to remove
    public bool NotGroundedAndJumped;

    public float TimeSinceNotGrounded;

    // INVICIBILITY
    public bool IsTemporarilyInvincible;
    private float _invincibleTimeLeft;
    public Renderer Renderer;
    [SerializeField] private float flickerInterval = 0.1f;

    [Tooltip("Which layers should be considered 'ground' by the raycast.")]
    [SerializeField] private LayerMask groundLayer;

    [Tooltip("How far down the raycast should check for ground.")]
    [SerializeField] private float groundCheckDistance = 0.2f;

    [Tooltip("The transform from which the raycast will originate. Should be placed at the object's feet.")]
    [SerializeField] private Transform groundCheckPoint;

    public bool IsGrounded;

    public GameObject OnGroundEffectPrefab;
    public GameObject ComboEffectPrefab;

    // Events
    public UnityEvent OnJumpEvent;
    public UnityEvent OnDoubleJumpEvent;
    public UnityEvent OnMoveLeftEvent;
    public UnityEvent OnMoveRightEvent;
    public UnityEvent OnRollStartEvent;
    public UnityEvent OnComboRollStartEvent;


    /// <summary>
    /// Start is called before the first frame update.
    /// We initialize the target position to the player's starting position.
    /// </summary>
    void Start()
    {
        this.IsFlying = false;
        this.IsInDoubleJump = false;
        this.IsRolling = false;
        this.IsComboRoll = false;
        this.TimeSinceJumpRequested = 0.0f;
        this.NotGroundedAndJumped = false;
        this.TimeSicneRollStarted = 0.0f;

        this.DidComboRollAlreadyHitTheGround = false;
        // Set the initial target position to where the player currently is.
        targetPosition = transform.position;
    }

    /// <summary>
    /// Update is called once per frame.
    /// We use it to handle input and update the player's position smoothly.
    /// </summary>
    void Update()
    {
        this.TimeSinceJumpRequested += Time.deltaTime;
        this.TimeSicneRollStarted += Time.deltaTime;
        this.TimeSinceNotGrounded += Time.deltaTime;

        bool wasGrounded = this.IsGrounded;
        CheckIfGrounded();
        if (wasGrounded == false && this.IsGrounded == true)
        {
            if (this.OnGroundEffectPrefab) Instantiate(this.OnGroundEffectPrefab, this.transform.position + Vector3.down * 0.41f, Quaternion.identity);
        }

        if (this.IsGrounded)
        {
            this.TimeSinceNotGrounded = 0.0f;
        }

        if (this.IsGrounded && this.DidComboRollAlreadyHitTheGround == false && this.IsComboRoll)
        {
            this.DidComboRollAlreadyHitTheGround = true;
            Rigidbody rbb = this.GetComponent<Rigidbody>();
            // This will immediately stop all linear movement caused by any previous forces.
            rbb.linearVelocity = Vector3.zero;

            // If the impulse might have also caused rotation, you can stop that too.
            rbb.angularVelocity = Vector3.zero;
            //Debug.Break();
        }

        if (this.transform.position.y <= -4f)
        {
            if (!this.GameManager.IsPlayerDead)
            {
                this.GameManager.SetPlayerDead();
            }

            return;
        }

        this.IsFlying = !this.IsGrounded;

        if (!this.IsFlying)
        {
            this.IsInDoubleJump = false;
        }

        if (this.IsRolling)
        {
            this._rollingTimeLeft -= Time.deltaTime;

            if (this._rollingTimeLeft <= 0.0f)
            {
                this.IsRolling = false;
            }
        }


        if (this.IsFlying && this.IsRolling)
        {
            //if (this.NotGroundedAndJumped)
            if (this.TimeSicneRollStarted <= 0.12f)
            {
                if (!this.IsComboRoll)
                {
                    Rigidbody rb = this.GetComponent<Rigidbody>();
                    rb.AddForce(Vector3.down * 24f * 2f, ForceMode.Impulse);
                    this._rollingTimeLeft += 1.0f;
                    this.GameManager.InfiniteRunControllerWorlds.speed += 3.25f;
                    //this.GameManager.InfiniteRunControllerWorlds.TempTargetSpeed += 4.0f;
                    this.OnComboRollStartEvent?.Invoke();
                    //if (this.ComboController) this.ComboController.OnCombo();
                    if (this.ComboEffectPrefab) Instantiate(this.ComboEffectPrefab, this.transform.position + Vector3.down * 1.25f, Quaternion.Euler(0, 180f, 180));

                    //Debug.Break();

                }
                this.IsComboRoll = true;
                this.DidComboRollAlreadyHitTheGround = false;
            }
        }
        else
        {
            if (!this.IsRolling) this.IsComboRoll = false;
        }

        //this.BearObj.transform.eulerAngles = new Vector3(0, 0, 0);
        // Check for left/right input.
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            this.OnMoveLeft();
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            this.OnMoveRight();
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.W))
        {
            this.OnJump();
        }
        else if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.S))
        {
            this.SpeedUp();
        }

        // Clamp the position index to ensure it's always -1, 0, or 1.
        this.HorizontalPositionIndex = Mathf.Clamp(this.HorizontalPositionIndex, -3, 3);
        this.VerticalPositionIndex = Mathf.Clamp(this.VerticalPositionIndex, -1, 1);

        // Calculate the target X coordinate based on the current lane index.
        float targetX = HorizontalPositionIndex * laneDistance;

        //float targetY = VerticalPositionIndex * laneDistance + 6.0f;

        // Update the target position. We maintain the player's current Y and Z position.
        // The original script used hardcoded Y=0 and Z=-7. This approach is more flexible.
        targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);
        //targetPosition = new Vector3(targetX, targetY, transform.position.z);

        // Smoothly move the player from its current position to the target position.
        // Vector3.Lerp() handles the interpolation. Time.deltaTime makes it frame-rate independent.
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);
    }

    private void CheckIfGrounded()
    {
        // The Physics.Raycast function returns true if the ray intersects with a collider, and false otherwise.
        // We are casting a ray from the 'groundCheckPoint' position, downwards (Vector3.down),
        // for a specific 'groundCheckDistance', and only looking for colliders in the 'groundLayer'.
        IsGrounded = Physics.Raycast(groundCheckPoint.position, Vector3.down, groundCheckDistance, groundLayer);

        if (this.NotGroundedAndJumped && IsGrounded)
        {
            if (this.TimeSinceJumpRequested >= 0.15f)
            {
                this.NotGroundedAndJumped = false;
            }
        }
    }


    /// <summary>
    /// Decrements the position index to move one lane to the left.
    /// </summary>
    public void OnMoveLeft()
    {
        this.HorizontalPositionIndex--;
    }

    /// <summary>
    /// Increments the position index to move one lane to the right.
    /// </summary>
    public void OnMoveRight()
    {
        this.HorizontalPositionIndex++;
    }

    public void OnMoveUp()
    {
        this.VerticalPositionIndex++;
    }

    public void OnMoveDown()
    {
        this.VerticalPositionIndex--;
    }

    public void SpeedUp()
    {
        if (this.GameManager.Energy <= 0)
        {
            return;
        }
        //if (this.IsRolling) return;

        this.IsRolling = true;
        this.TimeSicneRollStarted = 0.0f;
        this._rollingTimeLeft = this.RollingTime;
        this.Animator.SetTrigger("PlayerRun");
        //this.GameManager.InfiniteRunControllerWorlds.speed = 38.0f;
        this.GameManager.InfiniteRunControllerWorlds.speed += 2.25f;

        this.GameManager.Energy -= 0.5f;
    }

    public void OnJump()
    {
        if (this.IsInDoubleJump) return; // triple jump not allowed

        if (this.IsComboRoll)
        {
            Rigidbody rbb = this.GetComponent<Rigidbody>();
            // This will immediately stop all linear movement caused by any previous forces.
            rbb.linearVelocity = Vector3.zero;

            // If the impulse might have also caused rotation, you can stop that too.
            rbb.angularVelocity = Vector3.zero;
            this.IsRolling = false;
            this.IsComboRoll = false;
        }

        if (this.IsFlying)
        {
            // if (this.TimeSinceNotGrounded > 0.03f) // 0.35f // if not jumped but falling for less than t, treat it as a single jump
            if (this.TimeSinceNotGrounded > 0.01f) // 0.35f // if not jumped but falling for less than t, treat it as a single jump
            {
                if (!this.IsInDoubleJump)
                {
                    // double jump
                    this.IsInDoubleJump = true;
                    this.OnDoubleJumpEvent?.Invoke();
                }
                else
                {
                    return;
                }
            }

        }

        this.Animator.SetTrigger("PlayerJump");
        this.OnJumpEvent?.Invoke();

        Rigidbody rb = this.GetComponent<Rigidbody>();

        rb.AddForce(Vector3.up * JumpForce * 1.65f, ForceMode.Impulse);

        // Apply a random rotational force to make the player tumble.
        Vector3 randomTorque = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        );
        rb.AddTorque(randomTorque.normalized * TorqueForce * JumpForce, ForceMode.Impulse);

        this.TimeSinceJumpRequested = 0.0f;
        this.NotGroundedAndJumped = true;
    }

    private void _makeInvincible(float time)
    {
        this.IsTemporarilyInvincible = true;
        this._invincibleTimeLeft = time;

        StartCoroutine(InvincibilityCoroutine());
    }

    private IEnumerator InvincibilityCoroutine()
    {
        IsTemporarilyInvincible = true;

        float endTime = Time.time + 2f;

        // Loop until the invincibility duration is over.
        while (Time.time < endTime)
        {
            // Toggle the renderer's visibility.
            this.Renderer.enabled = !this.Renderer.enabled;

            // Wait for the specified flicker interval before toggling again.
            yield return new WaitForSeconds(flickerInterval);
        }

        // --- Cleanup ---
        // Ensure the renderer is enabled when the coroutine finishes.
        this.Renderer.enabled = true;
        IsTemporarilyInvincible = false;
    }

    public void OnPlayerHurt()
    {
        if (this.IsTemporarilyInvincible) return;

        //this.GameManager.InfiniteRunControllerWorlds.speed = this.GameManager.InfiniteRunControllerWorlds.TargetSpeed;
        //this.GameManager.InfiniteRunControllerWorlds.TempTargetSpeed = this.GameManager.InfiniteRunControllerWorlds.TargetSpeed;
        //this.GameManager.SoundManager.PlaySound(Random.Range(10, 13));
        this.Animator.SetTrigger("PlayerHit");
        Rigidbody rb = this.GetComponent<Rigidbody>();

        rb.AddForce(Vector3.up * 1.0f * 0.8f, ForceMode.Impulse);
        Vector3 randomTorque = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        );
        rb.AddTorque(randomTorque.normalized * 1.0f * 0.8f, ForceMode.Impulse);

        //Invoke(nameof(this._spawnPlayerHitEffect), 0.3f);

        this._makeInvincible(2.0f);
    }

    public void OnDestroyedSomething()
    {
        //if (this.ComboController) this.ComboController.OnCombo();
        //Instantiate(this.ComboEffectPrefab, this.transform.position + Vector3.down * 0.25f, Quaternion.Euler(0, 180f, 180));
    }
}
