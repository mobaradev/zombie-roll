using System.Collections.Generic;
using UnityEngine;

public class InfiniteRunControllerWorlds : MonoBehaviour
{
    public bool IsRunning;
    public float speed;
    public float TargetSpeed;
    public GameObject Wrapper;
    public float RoadSpawnZ;
    public float ChunkSpawnZ;

    public float ChunkSpawnDeltaZ;
    public float TmpCounter = 0;
    public float LastSpawnedZ;
    public float NextZToSpawn = 0;

    public int LastGeneretedIndex = -1;
    public class ChunkType
    {
        public ChunkType(int a, int b)
        {
            this.typeId = a;
            this.worldId = b;
        }
        public int typeId;
        public int worldId;
    }
    public List<ChunkType> ChunkTypeList = new List<ChunkType>();

    [System.Serializable]
    public class World
    {
        public string Name;
        public List<GameObject> NormalChunks;
        public List<GameObject> TrapChunks;
        public List<GameObject> EmptyChunks;
        public GameObject EndChunk;
        public GameObject IChunk;
    }

    public List<World> Worlds = new List<World>();

    public int CurrentWorldIndex = 0;

    public int LastYPosIndex = 0;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.IsRunning = true;
        this.LastGeneretedIndex = -1;
        this._generateChunkTypeList();
    }

    private void _generateChunkTypeList()
    {
        // E = empty chunk [3]
        // I = indicator [0]
        // End - end [1]
        // A - normal chunks [2]
        // T - trap chunks [4]




        for (int i = 0; i < 99; i++)
        {
            this.CurrentWorldIndex = (this.CurrentWorldIndex + 1) % this.Worlds.Count;
            List<ChunkType> l = new List<ChunkType>();

            // start with an E
            l.Add(new ChunkType(3, this.CurrentWorldIndex));

            // start with I
            l.Add(new ChunkType(0, this.CurrentWorldIndex));

            // 3x A
            for (int j = 0; j < 2; j++) l.Add(new ChunkType(2, this.CurrentWorldIndex));

            // T
            //l.Add(4);

            // 3x A
            for (int j = 0; j < 3; j++) l.Add(new ChunkType(2, this.CurrentWorldIndex));

            // End
            l.Add(new ChunkType(1, this.CurrentWorldIndex));
            l.Add(new ChunkType(3, this.CurrentWorldIndex));

            // 2x A
            for (int j = 0; j < 2; j++) l.Add(new ChunkType(2, this.CurrentWorldIndex));

            // T
            l.Add(new ChunkType(4, this.CurrentWorldIndex));

            // 3x A
            for (int j = 0; j < 3; j++) l.Add(new ChunkType(2, this.CurrentWorldIndex));

            // End
            l.Add(new ChunkType(1, this.CurrentWorldIndex));
            l.Add(new ChunkType(3, this.CurrentWorldIndex));

            // 2x A
            for (int j = 0; j < 2; j++) l.Add(new ChunkType(2, this.CurrentWorldIndex));

            // T
            l.Add(new ChunkType(4, this.CurrentWorldIndex));

            // 3x A
            for (int j = 0; j < 3; j++) l.Add(new ChunkType(2, this.CurrentWorldIndex));

            // End
            l.Add(new ChunkType(1, this.CurrentWorldIndex));
            l.Add(new ChunkType(3, this.CurrentWorldIndex));

            this.ChunkTypeList.AddRange(l);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (this.IsRunning)
        {
            this.Wrapper.transform.position += Vector3.back * speed * Time.deltaTime;

            if (this.speed > this.TargetSpeed)
            {
                this.speed -= Time.deltaTime * 12f;
            }
        }

        if (this.Wrapper.transform.position.z + this.NextZToSpawn < 200)
        {
            this.GenerateChunk();
        }
    }

    public void GenerateChunk()
    {
        // decide which chunk should be generated next
        int currentIndex = this.LastGeneretedIndex + 1;
        int typeOfChunkToGenerate = this.ChunkTypeList[currentIndex].typeId;
        int worldId = this.ChunkTypeList[currentIndex].worldId;

        this.GetComponent<SideChunksController>().WorldIndexToUse = worldId;

        if (typeOfChunkToGenerate == 0)
        {
            this.GenerateIChunk(worldId);
        }
        else if (typeOfChunkToGenerate == 1)
        {
            this.GenerateEndChunk(worldId);
        }
        else if (typeOfChunkToGenerate == 2)
        {
            this.GenerateNormalChunk(worldId);
        }
        else if (typeOfChunkToGenerate == 3)
        {
            this.GenerateEmptyChunk(worldId);
        }
        else if (typeOfChunkToGenerate == 4)
        {
            this.GenerateTrapChunk(worldId);
        }



        this.LastGeneretedIndex++;
    }

    public void GenerateEmptyChunk(int worldId)
    {
        int randomChunkId = Random.Range(0, this.Worlds[worldId].EmptyChunks.Count);
        GameObject newChunk = Instantiate(this.Worlds[worldId].EmptyChunks[randomChunkId], new Vector3(-8.75f, this._getChunksRandomPosY(), this.NextZToSpawn), Quaternion.identity);
        newChunk.transform.parent = this.Wrapper.transform;
        newChunk.transform.localPosition = new Vector3(-8.75f, this._getChunksRandomPosY(), this.NextZToSpawn);

        this.NextZToSpawn = this.NextZToSpawn + newChunk.GetComponent<ChunkController>().ChunkSize;
    }

    public void GenerateTrapChunk(int worldId)
    {
        int randomChunkId = Random.Range(0, this.Worlds[worldId].TrapChunks.Count);
        //if (this.LastGeneretedIndex < 20)
        //{
        //    randomChunkId = 0;
        //}
        GameObject newChunk = Instantiate(this.Worlds[worldId].TrapChunks[randomChunkId], new Vector3(-8.75f, this._getChunksRandomPosY(), this.NextZToSpawn), Quaternion.identity);
        newChunk.transform.parent = this.Wrapper.transform;
        newChunk.transform.localPosition = new Vector3(-8.75f, this._getChunksRandomPosY(), this.NextZToSpawn);

        this.NextZToSpawn = this.NextZToSpawn + newChunk.GetComponent<ChunkController>().ChunkSize;
    }

    public void GenerateNormalChunk(int worldId)
    {

        int randomChunkId = Random.Range(0, this.Worlds[worldId].NormalChunks.Count);
        GameObject newChunk = Instantiate(this.Worlds[worldId].NormalChunks[randomChunkId], new Vector3(-8.75f, this._getChunksRandomPosY(), this.NextZToSpawn), Quaternion.identity);
        newChunk.transform.parent = this.Wrapper.transform;
        newChunk.transform.localPosition = new Vector3(-8.75f, this._getChunksRandomPosY(), this.NextZToSpawn);

        this.NextZToSpawn = this.NextZToSpawn + newChunk.GetComponent<ChunkController>().ChunkSize;
    }

    public void GenerateEndChunk(int worldId)
    {
        GameObject newChunk = Instantiate(this.Worlds[worldId].EndChunk, new Vector3(-8.75f, this._getChunksRandomPosY(), this.NextZToSpawn), Quaternion.identity);
        newChunk.transform.parent = this.Wrapper.transform;
        newChunk.transform.localPosition = new Vector3(-8.75f, this._getChunksRandomPosY(), this.NextZToSpawn);

        this.NextZToSpawn = this.NextZToSpawn + newChunk.GetComponent<ChunkController>().ChunkSize;
    }

    public void GenerateIChunk(int worldId)
    {

        GameObject newChunk = Instantiate(this.Worlds[worldId].IChunk, new Vector3(-8.75f, this._getChunksRandomPosY(), this.NextZToSpawn), Quaternion.identity);
        newChunk.transform.parent = this.Wrapper.transform;
        newChunk.transform.localPosition = new Vector3(-8.75f, this._getChunksRandomPosY(), this.NextZToSpawn);

        this.NextZToSpawn = this.NextZToSpawn + newChunk.GetComponent<ChunkController>().ChunkSize;
    }

    private float _getChunksRandomPosY()
    {
        float randomValue = Random.value;

        if (this.LastYPosIndex == 0)
        {
            if (randomValue <= 0.8f)
            {

            }
            else if (randomValue <= 0.97f)
            {
                this.LastYPosIndex = 1;
            }
            else
            {
                this.LastYPosIndex = 2;
            }
        } else if (this.LastYPosIndex == 1)
        {
            if (randomValue <= 0.65f)
            {

            }
            else if (randomValue <= 0.875f)
            {
                this.LastYPosIndex = 0;
            } else
            {
                this.LastYPosIndex = 2;
            }
        } else if (this.LastYPosIndex == 2)
        {
            if (randomValue <= 0.5f)
            {

            }
            else if (randomValue <= 0.785f)
            {
                this.LastYPosIndex = 1;
            }
            else
            {
                this.LastYPosIndex = 0;
            }
        }



        if (this.LastYPosIndex == 0)
        {
            // low
            return 0;
        }
        else if (this.LastYPosIndex == 1)
        {
            // medium
            return 3.5f;
        }
        else if (this.LastYPosIndex == 2)
        {
            // high
            return 7.5f;
        }

        return 0;
    }
}
