using UnityEngine;

public class ChunkController : MonoBehaviour
{
    public InfiniteRunControllerWorlds InfiniteRunController;
    public float ChunkSize = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.InfiniteRunController = FindFirstObjectByType<InfiniteRunControllerWorlds>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.z < -80)
        {
            Destroy(this.gameObject);
        }
    }
}
