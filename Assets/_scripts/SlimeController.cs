using UnityEngine;

public class SlimeController : MonoBehaviour
{
    public bool IsUsedUp = false;
    public void OnTriggerEnter(Collider other)
    {
        if (this.IsUsedUp) return;
        

        if (other.gameObject.GetComponent<PlayerController>())
        {
            if (other.gameObject.GetComponent<PlayerController>().IsComboRoll)
            {
                other.gameObject.GetComponent<PlayerController>().OnJumpedOnSlime(7.25f, 2.25f);
            }
            else
            {
                other.gameObject.GetComponent<PlayerController>().OnJumpedOnSlime(3.25f, 1.8f);
            }

            this.IsUsedUp = true;
            //Destroy(this.gameObject);
        }
    }
}
