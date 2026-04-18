using UnityEngine;

public class Target : MonoBehaviour
{
    public TargetManager manager;
    public Transform spawnPoint;

    private bool canBeHit = true;

    private void Start()
    {
        GameTimer.Instance.OnTimerEnd.AddListener(DisableTarget);
    }

    void DisableTarget()
    {
        canBeHit = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Arrow"))
        {
            if (!canBeHit) return;

            ScoreManager.Instance.AddPoint();

            manager.OnTargetDestroyed(gameObject, spawnPoint);

            Destroy(gameObject);
        }
    }
}