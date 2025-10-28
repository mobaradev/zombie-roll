using UnityEngine;
using UnityEngine.Events;


public class MobileInputDetector : MonoBehaviour
{
    // --- Public Events ---
    // You can subscribe to these events from other scripts to trigger actions.
    public UnityEvent OnTap;
    public UnityEvent OnSwipeLeft;
    public UnityEvent OnSwipeRight;

    // --- Swipe Configuration ---
    [Tooltip("The minimum distance in screen pixels a swipe must travel to be detected.")]
    [SerializeField] private float minSwipeDistance = 50f;

    [Tooltip("The maximum time in seconds a touch can last to be considered a tap.")]
    [SerializeField] private float maxTapTime = 0.3f;


    // --- Private Variables ---
    private Vector2 fingerDownPosition;
    private Vector2 fingerUpPosition;
    private float touchStartTime;


    public PlayerController PlayerController;

    void Update()
    {
        // Check if there is any touch input
        if (Input.touchCount > 0)
        {
            // Get the first touch
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                // --- Finger Down ---
                // Record the starting time and position of the touch.
                case TouchPhase.Began:
                    fingerDownPosition = touch.position;
                    fingerUpPosition = touch.position; // Initialize to the same position
                    touchStartTime = Time.time;
                    break;

                // --- Finger Lifted ---
                // The finger has been lifted from the screen.
                // This is where we will determine if the gesture was a tap or a swipe.
                case TouchPhase.Ended:
                    fingerUpPosition = touch.position;
                    float touchDuration = Time.time - touchStartTime;

                    // Check if it's a tap
                    if (IsTap(touchDuration))
                    {
                        DetectTap();
                    }
                    // Check if it's a swipe
                    else
                    {
                        DetectSwipe();
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// Checks if the gesture qualifies as a tap based on duration and distance.
    /// </summary>
    /// <param name="duration">The total time the finger was on the screen.</param>
    /// <returns>True if it's a tap, false otherwise.</returns>
    private bool IsTap(float duration)
    {
        // A tap must be short in duration and the finger shouldn't have moved much.
        float distance = Vector2.Distance(fingerDownPosition, fingerUpPosition);
        return duration < maxTapTime && distance < minSwipeDistance;
    }

    /// <summary>
    /// Determines the direction of the swipe and invokes the corresponding event.
    /// </summary>
    private void DetectSwipe()
    {
        float swipeDistance = Vector2.Distance(fingerDownPosition, fingerUpPosition);

        Vector2 direction = fingerUpPosition - fingerDownPosition;
        float horizontalDistance = direction.x;
        float verticalDistance = direction.y;

        // Only detect swipes that exceed the minimum distance threshold.
        if (swipeDistance > minSwipeDistance)
        {

            // Prioritize horizontal swipes over vertical ones
            if (Mathf.Abs(horizontalDistance) > Mathf.Abs(verticalDistance))
            {
                if (horizontalDistance < 0)
                {
                    SwipeLeft();
                }
                else
                {
                    SwipeRight();
                }
            }
            else
            {
                if (verticalDistance > 0)
                {
                    SwipeUp();
                } else
                {
                    SwipeDown();
                }
                // You could add swipe down detection here if needed
            }
            // You could add vertical swipe detection here if needed (e.g., SwipeUp/SwipeDown)
            // else { /* Vertical swipe logic */ }
        }
    }

    // --- Action Methods ---

    private void DetectTap()
    {
        Debug.Log("Tap Detected!");
        OnTap?.Invoke(); // Fire the OnTap event
        //this.PlayerShootingController.TryFireBullet();
        //this.PlayerShootingController.TryFireBullet();
    }

    private void SwipeLeft()
    {
        Debug.Log("Swipe Left Detected!");
        OnSwipeLeft?.Invoke(); // Fire the OnSwipeLeft event
        this.PlayerController.OnMoveLeft();
    }

    private void SwipeRight()
    {
        Debug.Log("Swipe Right Detected!");
        OnSwipeRight?.Invoke(); // Fire the OnSwipeRight event
        this.PlayerController.OnMoveRight();
    }

    private void SwipeUp()
    {
        Debug.Log("Swipe Up Detected!");
        this.PlayerController.OnJump();
    }

    private void SwipeDown()
    {
        Debug.Log("Swipe Down Detected!");
        this.PlayerController.SpeedUp();
    }
}

