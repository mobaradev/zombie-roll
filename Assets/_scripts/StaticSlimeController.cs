using UnityEngine;

public class StaticSlimeController : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            if (other.gameObject.GetComponent<PlayerController>().IsGrounded && (other.gameObject.transform.position.y - 0.28f) <= this.transform.position.y)
            {
                other.gameObject.GetComponent<PlayerController>().OnJumpedOnSlime(5.30f, 1.3f);
                Debug.Log("Case A");
            }
            else
            {
                if (other.gameObject.GetComponent<PlayerController>().IsRolling)
                {
                    other.gameObject.GetComponent<PlayerController>().OnJumpedOnSlime(29.25f, 3.65f);
                    Debug.Log("Case B");
                }
                else
                {
                    other.gameObject.GetComponent<PlayerController>().OnJumpedOnSlime(9.25f, 2.1f);
                    Debug.Log("Case C");
                }
            }
            //Destroy(this.gameObject);
        }
    }
}
