using UnityEngine;

public class MovingZombie : MonoBehaviour
{
    public float speed = 2f;
    public float leftLimit = -5f;
    public float rightLimit = 5f;
    private bool movingRight = true;

    void Update()
    {
        if (movingRight)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
            if (transform.position.x >= rightLimit)
                movingRight = false;
        }
        else
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
            if (transform.position.x <= leftLimit)
                movingRight = true;
        }
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("MovingZombie collision with: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player detected!");
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(1);
                Debug.Log("Player took damage!");
            }
        }
    }
}
