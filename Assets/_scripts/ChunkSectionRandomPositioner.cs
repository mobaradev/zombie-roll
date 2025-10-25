using System.Collections.Generic;
using UnityEngine;

public class ChunkSectionRandomPositioner : MonoBehaviour
{
    public List<float> PossibleX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.transform.position = new Vector3(PossibleX[Random.Range(0, PossibleX.Count)], this.transform.position.y, this.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
