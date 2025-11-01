using UnityEngine;

/// <summary>
/// This script makes a GameObject (the enemy) chase another GameObject (the player).
/// </summary>
public class EnemyChase : MonoBehaviour
{
    // The target the enemy will chase. Assign the player's transform to this in the Unity Inspector.
    public Transform player;

    // The speed at which the enemy moves towards the player.
    public float speed = 5.0f;

    public bool AllowYMovement = false;

    public bool AutoSetDirection = true;
    public Vector3 Direction;

    private void Start()
    {
        this.enabled = false;
    }

    private void Awake()
    {
        this.player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    void Update()
    {
        // First, check if the 'player' variable has been set.
        // If not, this prevents errors from being thrown every frame.
        if (player == null)
        {
            // You can optionally log a warning to remind you to set the player in the inspector.
            // Debug.LogWarning("Player target not set for " + gameObject.name);
            return; // Exit the Update method if there's no player to chase.
        }

        // --- Movement ---

        if (this.AutoSetDirection)
        {
            this.Direction = player.position - transform.position;
            Direction.Normalize();
            transform.LookAt(player);
        }

        transform.position += Direction * speed * Time.deltaTime;

        if (this.AllowYMovement)
        {
            //transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, this.transform.position.y, transform.position.z);
        }


        // --- Rotation (Optional) ---

        // This line will make the enemy's forward vector (its "front") point directly at the player.
        // This is a simple way to make the enemy face what it's chasing.



        if (this.AllowYMovement)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);
        }
    }
}
