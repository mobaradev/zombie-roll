using UnityEngine;

public class ChunkSectionRandomDestoyer : MonoBehaviour
{
    public float chanceToDestroy = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Random.Range(0.0f, 1.0f) < chanceToDestroy)
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
