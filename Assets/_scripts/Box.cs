using UnityEngine;
using UnityEngine.SceneManagement;

public class Box : MonoBehaviour
{
    public GameObject DestroyEffectPrefab;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            if (other.gameObject.GetComponent<PlayerController>().IsRolling)
            {
                //GameObject contentObj = Instantiate(this.ContentPrefab, this.transform.position, Quaternion.identity);
                //contentObj.transform.parent = this.transform.parent;
                other.gameObject.GetComponent<PlayerController>().OnDestroyedSomething();
            } else
            {
                FindFirstObjectByType<GameManager>().SetPlayerHurt();
            }

            if (this.DestroyEffectPrefab) Instantiate(this.DestroyEffectPrefab, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
