using UnityEngine;
using System.Collections;

public class FinalKey : MonoBehaviour
{
    public GameObject survivedPanel;
    public float spawnDuration = 1f;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();

        gameObject.SetActive(false); // hidden until boss dies
    }

    private void OnEnable()
    {
        BossZombie.OnBossDead += SpawnKey;
    }

    private void OnDisable()
    {
        BossZombie.OnBossDead -= SpawnKey;
    }

    private void SpawnKey()
    {
        gameObject.SetActive(true);
        StartCoroutine(AnimateSpawn());
    }

    private IEnumerator AnimateSpawn()
    {
        float timer = 0f;
        Vector3 initialScale = Vector3.zero;
        Vector3 finalScale = Vector3.one;

        Color color = spriteRenderer.color;
        color.a = 0f;
        spriteRenderer.color = color;

        while (timer < spawnDuration)
        {
            timer += Time.deltaTime;
            float t = timer / spawnDuration;

            transform.localScale = Vector3.Lerp(initialScale, finalScale, t);
            color.a = Mathf.Lerp(0f, 1f, t);
            spriteRenderer.color = color;

            yield return null;
        }

        transform.localScale = finalScale;
        color.a = 1f;
        spriteRenderer.color = color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if (survivedPanel != null)
        {
            survivedPanel.SetActive(true);
            Time.timeScale = 0f; // pause game
        }

        Destroy(gameObject);
    }
}