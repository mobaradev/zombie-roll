using System.Collections.Generic;
using UnityEngine;

public class SideChunksController : MonoBehaviour
{
    public bool IsRunning;
    public float speed;
    public float TargetSpeed;
    public GameObject SideWrapper;
    public float RoadSpawnZ;
    public float ChunkSpawnZ;

    public float ChunkSpawnDeltaZ;
    public float TmpCounter = 0;
    public float LastSpawnedZ;
    public float NextZToSpawn = 0;

    public int LastGeneretedIndex = -1;

    public int WorldIndexToUse = -1;

    [Header("Side chunks")]
    public List<GameObject> SideChunksLeft;
    public List<GameObject> SideChunksRight;


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
            this.SideWrapper.transform.position += Vector3.back * speed * Time.deltaTime;

            if (this.speed > this.TargetSpeed)
            {
                this.speed -= Time.deltaTime * 12f;
            }
        }

        if (this.SideWrapper.transform.position.z + this.NextZToSpawn < 200)
        {
            this.GenerateChunk();
        }
    }

    public void GenerateChunk()
    {
        if (this.WorldIndexToUse == -1) return;

        int currentIndex = this.LastGeneretedIndex + 1;

        //float leftSideY = Random.Range(8f, 24f);
        float leftSideY = Random.Range(0, 0.1f);
        //float rightSideY = Random.Range(28f, 44f);
        float rightSideY = Random.Range(0, 0.1f);

        // left side
        GameObject newLeftChunk = Instantiate(this.SideChunksLeft[this.WorldIndexToUse], new Vector3(0, 0, this.NextZToSpawn), Quaternion.identity);
        newLeftChunk.transform.parent = this.SideWrapper.transform;
        //newLeftChunk.transform.localPosition = new Vector3(-20 - 3.5f*5 - 8.75f, 0, this.NextZToSpawn);
        newLeftChunk.transform.localPosition = new Vector3(-20 - 3.5f * 0 - 8.75f, leftSideY, this.NextZToSpawn);

        // right side
        GameObject newRightChunk = Instantiate(this.SideChunksRight[this.WorldIndexToUse], new Vector3(0, 0, this.NextZToSpawn), Quaternion.identity);
        newRightChunk.transform.parent = this.SideWrapper.transform;
        //newRightChunk.transform.localPosition = new Vector3(20f + 3.5f * 5 - 8.75f, 0, this.NextZToSpawn);
        newRightChunk.transform.localPosition = new Vector3(20f + 3.5f * 0 - 8.75f, rightSideY, this.NextZToSpawn);


        this.NextZToSpawn = this.NextZToSpawn + 18;
        this.LastGeneretedIndex++;
    }
}
