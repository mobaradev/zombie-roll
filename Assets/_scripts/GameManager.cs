using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int CurrentLevelIndex = -1;
    public int Points = 0;
    public int Coins = 0;
    public float Energy = 0;
    public int StrawberriesCollected = 0;
    public int Lives = 5;
    public int Feathers = 0;
    //public int DoorsWalkedThrough = 0;
    public bool IsPlayerDead = false;

    public PlayerController PlayerController;
    public InfiniteRunControllerWorlds InfiniteRunControllerWorlds;
    public SideChunksController SideChunksController;

    private float _timeSinceLastHurt = 0f;
    private bool _isSlowingDownRunController;

    public float DistanceTraveled;

    public bool ReloadSceneOnGameOver = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
    void Start()
    {
        this._isSlowingDownRunController = false;
    }

    // Update is called once per frame
    void Update()
    {
        this.Energy = 15;
        this.Points += 1;
        this.DistanceTraveled = this.InfiniteRunControllerWorlds.Wrapper.transform.position.z * (-1) / 20.0f;

        this._timeSinceLastHurt += Time.deltaTime;

        if (this._isSlowingDownRunController)
        {
            this.InfiniteRunControllerWorlds.speed -= Time.deltaTime * 21.25f;
            this.InfiniteRunControllerWorlds.speed = Mathf.Clamp(this.InfiniteRunControllerWorlds.speed, 0, 50);

            this.SideChunksController.speed -= Time.deltaTime * 21.25f;
            this.SideChunksController.speed = Mathf.Clamp(this.SideChunksController.speed, 0, 50);
        }

        this.Energy = Mathf.Clamp(this.Energy, 0, 15);
    }

    public void SetPlayerHurt()
    {
        if (this._timeSinceLastHurt <= 2.25f) return;

        this._timeSinceLastHurt = 0f;
        this.Lives -= 1;
        if (this.Lives <= 0)
        {
            this.SetPlayerDead();
            return;
        } else
        {
            this.PlayerController.OnPlayerHurt();
        }
    }

    public void SetPlayerDead()
    {
        
        if (this.IsPlayerDead) return;
        Debug.Log("Set Player Dead");
        this.IsPlayerDead = true;
        this._slowDownRunControllerToStop();
        //FindFirstObjectByType<CurveAdjuster>().IsActive = false;

        int strawberriesCollectedBefore = PlayerPrefs.GetInt("StrawberriesCollected_Level" + this.CurrentLevelIndex, 0);
        PlayerPrefs.SetInt("StrawberriesCollected_Level" + this.CurrentLevelIndex, strawberriesCollectedBefore + this.StrawberriesCollected);

        this.TriggerGameOver();
    }

    private void _slowDownRunControllerToStop()
    {
        this._isSlowingDownRunController = true;
    }

    public void TriggerGameOver()
    {
        if (this.ReloadSceneOnGameOver)
        {
            Invoke(nameof(this._reloadCurrentScene), 3.25f);
        } else
        {
            Invoke(nameof(this._goToMainMenuScene), 3.25f);
        }
    }


    private void _goToMainMenuScene()
    {
        SceneManager.LoadScene(0);
    }

    private void _reloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
