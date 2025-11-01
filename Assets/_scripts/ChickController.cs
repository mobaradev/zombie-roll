using UnityEngine;

public class ChickController : MonoBehaviour
{
    public GameObject TriggerReactionEffectPrefab;
    private GameObject _player;
    public bool IsChasingPlayer;
    private GameObject _effectObj;

    void Start()
    {
        this.IsChasingPlayer = false;
        this._player = GameObject.FindGameObjectWithTag("Player");
        this.GetComponent<EnemyChase>().speed = Random.Range(8.5f, 14.5f);
    }

    void Update()
    {


        if (!this.IsChasingPlayer && Vector3.Distance(this._player.transform.position, this.transform.position) <= 38.0f)
        {
            this.OnTriggerToChasePlayer();
        }

        if (this.IsChasingPlayer)
        {
            this._effectObj.transform.position = this.transform.position + Vector3.up * 3f;
        }
    }

    public void OnTriggerToChasePlayer()
    {
        this.GetComponent<EnemyChase>().enabled = true;
        this.IsChasingPlayer = true;
        this._effectObj = Instantiate(this.TriggerReactionEffectPrefab, this.transform.position + Vector3.up * 3f, Quaternion.identity);
        Invoke(nameof(this._enableChasingComponent), 0.25f);
    }

    private void _enableChasingComponent()
    {
        this.GetComponent<EnemyChase>().enabled = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            if (other.gameObject.GetComponent<PlayerController>().IsRolling)
            {
                // effect
                Destroy(this.gameObject);
            }
            else
            {
                if (!other.gameObject.GetComponent<PlayerController>().IsGrounded && other.gameObject.GetComponent<PlayerController>().TimeSinceNotGrounded >= 0.35f)
                {
                    // no hurt when flying
                    other.gameObject.GetComponent<PlayerController>().OnJumpedOnSlime(2.25f, 1.8f);
                }
                else
                {
                    FindFirstObjectByType<GameManager>().SetPlayerSlowedDown(0.93f);
                }

                Destroy(this.gameObject);
            }
        }
    }
}