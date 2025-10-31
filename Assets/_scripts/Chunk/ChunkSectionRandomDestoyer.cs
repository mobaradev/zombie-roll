using UnityEngine;

public class ChunkSectionRandomDestoyer : MonoBehaviour
{
    public float chanceToDestroy = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (RandomManager.Instance.RandomDestroyer.NextDouble() < chanceToDestroy)
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
