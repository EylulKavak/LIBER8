using System.Collections;
using UnityEngine;

public class AutoMoveDartBoard : MonoBehaviour
{
    public float moveRange = 1f;
    public float moveSpeed = 0.1f;
    private Vector3 startPosition;
    private bool movingRight = true;

    void Start()
    {
        startPosition = transform.position;
        StartCoroutine(MoveDartBoard());
    }

    IEnumerator MoveDartBoard()
    {
        while (true)
        {
            if (movingRight)
            {
                transform.position += Vector3.right * moveSpeed * Time.deltaTime;
                if (transform.position.x >= startPosition.x + moveRange)
                {
                    movingRight = false;
                }
            }
            else
            {
                transform.position += Vector3.left * moveSpeed * Time.deltaTime;
                if (transform.position.x <= startPosition.x - moveRange)
                {
                    movingRight = true;
                }
            }
            yield return null;
        }
    }
}
