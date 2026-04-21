using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickingArrowToSurface : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private SphereCollider myCollider;

    [SerializeField]
    private GameObject stickingArrow;

    //private Vector3 lastVelocity;

    private void OnCollisionEnter(Collision collision)
    {
        rb.isKinematic = true;
        myCollider.isTrigger = true;

        GameObject arrow = Instantiate(stickingArrow);

        // Place exactly where the collision happened
        arrow.transform.position = transform.position;
        arrow.transform.forward = transform.forward;

        // small offset so it doesn't clip awkwardly
        float pushInAmount = 0.75f;
        arrow.transform.position += arrow.transform.forward * pushInAmount;

        if (collision.collider.attachedRigidbody != null)
        {
            arrow.transform.parent = collision.collider.attachedRigidbody.transform;
        }

        Destroy(gameObject);

    }
}