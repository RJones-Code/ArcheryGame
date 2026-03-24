using UnityEngine;

public class TargetHit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Arrow"))
        {
            ScoreManager.Instance.AddPoint();
            other.enabled = false;
        }
    }
}
