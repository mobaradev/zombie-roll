using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BeeController : MonoBehaviour
{
    public GameObject TriggerReactionEffectPrefab;
    private GameObject _player;
    public bool IsChasingPlayer;
    private GameObject _effectObj;
    public bool IsFast = false;

    public GameObject OnDestroyEffectPrefab;
    public Animator animatorX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.IsChasingPlayer = false;
        this._player = GameObject.FindGameObjectWithTag("Player");

        this.GetComponent<EnemyChase>().speed = 10.00f;
    }

    // Update is called once per frame
    void Update()
    {
        //this.SetColor(this.ColorAssignment.AssignedColor);

        if (!this.IsChasingPlayer && Vector3.Distance(this._player.transform.position, this.transform.position) <= (this.IsFast ? 82.0f : 54.0f))
        {
            if (true && !this.IsChasingPlayer)
            {
                this.OnTriggerToChasePlayer();
            }
        }

        if (this.IsChasingPlayer && this._effectObj)
        {
            this._effectObj.transform.position = this.transform.position + Vector3.up * 2.25f;
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            if (other.gameObject.GetComponent<PlayerController>().IsRolling)
            {
                other.gameObject.GetComponent<PlayerController>().OnDestroyedSomething();
            }
            else
            {
                FindFirstObjectByType<GameManager>().SetPlayerHurt();
            }

            Instantiate(this.OnDestroyEffectPrefab, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    public void OnTriggerToChasePlayer()
    {
        this.GetComponent<EnemyChase>().enabled = true;
        //this.animatorX.SetTrigger("Run");
        this.IsChasingPlayer = true;
        this._effectObj = Instantiate(this.TriggerReactionEffectPrefab, this.transform.position + Vector3.down * 1.5f, Quaternion.identity);
        Invoke(nameof(this._enableChasingComponent), this.IsFast ? 0.05f : 0.15f);
    }

    private void _enableChasingComponent()
    {
        this.GetComponent<EnemyChase>().enabled = true;
    }
}
