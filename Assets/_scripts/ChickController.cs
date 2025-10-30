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
            }
            else
            {
                FindFirstObjectByType<GameManager>().SetPlayerHurt();
            }

            Destroy(this.gameObject);
        }
    }
}
