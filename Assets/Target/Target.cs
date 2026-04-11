using UnityEngine;

public class Target : MonoBehaviour
{
    public TargetManager manager;
    public Transform spawnPoint;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Arrow"))
        {
            ScoreManager.Instance.AddPoint();

            manager.OnTargetDestroyed(gameObject, spawnPoint);

            Destroy(gameObject);
        }
    }
}