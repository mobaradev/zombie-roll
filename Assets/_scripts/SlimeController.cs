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
                other.gameObject.GetComponent<PlayerController>().OnJumpedOnSlime(true);
            }
            else
            {
                other.gameObject.GetComponent<PlayerController>().OnJumpedOnSlime();
            }

            this.IsUsedUp = true;
            //Destroy(this.gameObject);
        }
    }
}
