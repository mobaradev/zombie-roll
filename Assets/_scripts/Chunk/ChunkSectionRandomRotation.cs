using UnityEngine;

public class ChunkSectionRandomRotation : MonoBehaviour
{
    public bool RandomRotateY = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (this.RandomRotateY)
        {
            float randomY = RandomManager.Instance.ChunkSectionRandomRotation.Next(0, 4) * 90;
            this.transform.Rotate(0, randomY, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
