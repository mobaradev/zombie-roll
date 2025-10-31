using System.Collections.Generic;
using UnityEngine;

public class ChunkSectionRandomPositioner : MonoBehaviour
{
    public bool UseRange = true;
    public float MinX = 1.75f;
    public float MaxX = 15.75f;
    public float StepX = 3.5f;

    public bool UseList = false;
    public List<float> PossibleX;

    public bool UseDeltaX = false;
    public float DeltaX = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (this.UseRange)
        {
            float range = this.MaxX - this.MinX;
            int steps = Mathf.FloorToInt(range / this.StepX) + 1;
            int randomStep = RandomManager.Instance.RandomPositioner.Next(0, steps);
            float randomX = this.MinX + (randomStep * this.StepX);
            this.transform.localPosition = new Vector3(randomX, this.transform.localPosition.y, this.transform.localPosition.z);
        }
        else if (this.UseList)
        {
            this.transform.localPosition = new Vector3(PossibleX[RandomManager.Instance.RandomPositioner.Next(0, PossibleX.Count)], this.transform.localPosition.y, this.transform.localPosition.z);
        } else if (this.UseDeltaX)
        {
            this.transform.localPosition = new Vector3(this.transform.localPosition.x + this.DeltaX, this.transform.localPosition.y, this.transform.localPosition.z);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
