using UnityEngine;

public class ChickController : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            if (other.gameObject.GetComponent<PlayerController>().IsRolling)
            {
                // effect
                Destroy(this.gameObject);
            }
            else
            {
                if (!other.gameObject.GetComponent<PlayerController>().IsGrounded && other.gameObject.GetComponent<PlayerController>().TimeSinceNotGrounded >= 0.35f)
                {
                    // no hurt when flying
                    other.gameObject.GetComponent<PlayerController>().OnJumpedOnSlime(2.25f, 1.8f);
                }
                else
                {
                    FindFirstObjectByType<GameManager>().SetPlayerHurt();
                }

                Destroy(this.gameObject);
            }
        }
    }
}