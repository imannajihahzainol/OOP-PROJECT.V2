using UnityEngine;

public class MovingZombie : MonoBehaviour
{
    public float speed = 2f;      // Movement speed
    public float leftLimit = -0f; // Left boundary
    public float rightLimit = 0f; // Right boundary

    private bool movingRight = true;

    void Update()
    {
        // Move right
        if (movingRight)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
            if (transform.position.x >= rightLimit)
                movingRight = false;
        }
        // Move left
        else
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
            if (transform.position.x <= leftLimit)
                movingRight = true;
        }

        // Flip zombie to face movement direction
        FlipZombie(movingRight);
    }

    void FlipZombie(bool movingRight)
    {
        if (movingRight)
        {
            transform.localScale = new Vector3(
                Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z
            );
        }
        else
        {
            transform.localScale = new Vector3(
                -Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z
            );
        }
    }
}