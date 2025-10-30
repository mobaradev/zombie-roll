using UnityEngine;

public class SideOfABlock : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player hit the side of a block!");
            other.gameObject.GetComponent<PlayerController>().OnPlayerHitBlockSide();
            FindAnyObjectByType<GameManager>().SetPlayerDead();
        }
    }
}
