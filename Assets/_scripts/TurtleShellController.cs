using UnityEngine;

public class TurtleShellController : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            if (other.gameObject.GetComponent<PlayerController>().IsRolling)
            {
                // effect

                //if (other.gameObject.GetComponent<PlayerController>().IsGrounded && other.gameObject.GetComponent<PlayerController>().TimeSinceGrounded >= 0.075f)
                if (other.gameObject.GetComponent<PlayerController>().IsGrounded && (other.gameObject.transform.position.y - 0.28f) <= this.transform.position.y)
                {
                    // destro shell no hurt for the player
                }
                else
                {
                    Debug.Log("Shell y = " + this.transform.position.y + " Player y = " + other.gameObject.transform.position.y);
                    //Debug.Break();
                    FindFirstObjectByType<GameManager>().SetPlayerDead();
                }
            }
            else
            {
                Debug.Log("Shell player not grounded");
                //Debug.Break();
                FindFirstObjectByType<GameManager>().SetPlayerDead();
            }

            Destroy(this.gameObject);
        }
    }
}
