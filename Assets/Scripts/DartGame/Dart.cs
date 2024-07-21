using UnityEngine;

public class Dart : MonoBehaviour
{
    private bool isStuck = false;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("DartBoard") && !isStuck)
        {
            isStuck = true;
            Debug.Log("Değdi");
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.velocity = Vector3.zero;
                rb.useGravity = false;
            }

            transform.SetParent(collision.transform);
        }
    }
}
