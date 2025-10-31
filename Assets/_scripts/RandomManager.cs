using UnityEngine;

public class RandomManager : MonoBehaviour
{
    public static RandomManager Instance { get; private set; }

    public System.Random RandomA;
    public System.Random RandomB;
    public System.Random RandomC;
    public System.Random RandomFeathers;
    public System.Random RandomDestroyer;
    public System.Random RandomPositioner;
    public System.Random ChunkSectionRandomRotation;
    public System.Random RandomBlockTransformY;
    public System.Random TreeScatter;

    private void Awake()
    {
        // Check if an instance of this singleton already exists.
        if (Instance != null && Instance != this)
        {
            // If an instance already exists, destroy this new one to enforce the singleton pattern.
            Debug.LogWarning("Another instance of RandomManager was found and has been destroyed.");
            Destroy(this.gameObject);
            return; // Stop execution of Awake for this duplicate instance.
        }

        // If this is the first instance, set it as the singleton instance.
        Instance = this;

        // Mark this singleton to not be destroyed when reloading scenes.
        DontDestroyOnLoad(this.gameObject);

        // Initialize the random number generators. This logic now only runs once.
        InitializeRandomGenerators();
    }

    public void InitializeRandomGenerators()
    {
        this.RandomA = new System.Random(134);
        this.RandomB = new System.Random(135);
        this.RandomC = new System.Random(136);
        this.RandomFeathers = new System.Random(136);
        this.RandomDestroyer = new System.Random(136);
        this.RandomPositioner = new System.Random(136);
        this.ChunkSectionRandomRotation = new System.Random(136);
        this.RandomBlockTransformY = new System.Random(136);
        this.TreeScatter = new System.Random(136);
    }
    public float NextFloatRange(System.Random random, float min, float max)
    {
        // NextDouble() returns a double in the range [0.0, 1.0).
        double val = (random.NextDouble() * (max - min) + min);
        return (float)val;
    }
}

