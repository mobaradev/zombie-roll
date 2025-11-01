using System.Collections.Generic;
using UnityEngine;

public class GroundChunksController : MonoBehaviour
{
    public bool IsRunning;
    public float speed;
    public float TargetSpeed;
    public GameObject GroundWrapper;
    public float RoadSpawnZ;
    public float ChunkSpawnZ;

    public float ChunkSpawnDeltaZ;
    public float TmpCounter = 0;
    public float LastSpawnedZ;
    public float NextZToSpawn = 0;

    public int LastGeneretedIndex = -1;

    public List<GameObject> GroundChunks;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.IsRunning = true;
        //Time.timeScale = 1f;
        this.LastGeneretedIndex = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.IsRunning)
        {
            this.GroundWrapper.transform.position += Vector3.back * speed * Time.deltaTime;

            if (this.speed > this.TargetSpeed)
            {
                this.speed -= Time.deltaTime * 12f;
            }
        }

        if (this.GroundWrapper.transform.position.z + this.NextZToSpawn < 200)
        {
            this.GenerateChunk();
        }
    }

    public void GenerateChunk()
    {
        int currentIndex = this.LastGeneretedIndex + 1;

        // left side
        GameObject newLeftChunk = Instantiate(this.GroundChunks[0], new Vector3(0, 0, this.NextZToSpawn), Quaternion.identity);
        newLeftChunk.transform.parent = this.GroundWrapper.transform;
        //newLeftChunk.transform.localPosition = new Vector3(-20 - 3.5f*5 - 8.75f, 0, this.NextZToSpawn);
        newLeftChunk.transform.localPosition = new Vector3(0, -3.0f, this.NextZToSpawn);


        this.NextZToSpawn = this.NextZToSpawn + 150;
        this.LastGeneretedIndex++;
    }
}