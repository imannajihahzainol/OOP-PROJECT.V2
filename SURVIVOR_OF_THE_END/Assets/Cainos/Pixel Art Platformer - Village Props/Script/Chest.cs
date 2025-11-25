using UnityEngine;

public class Chest : MonoBehaviour
{
    [Header("Optional Key")]
    public string keyType;

    [Header("Item Settings")]
    public bool hasItem = false;
    public GameObject itemPrefab;
    public float spawnOffsetY = 0.5f;   // <-- declared here, accessible everywhere in this class

    [Header("Animation")]
    public Animator animator;

    private bool isOpened = false;

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isOpened)
            OpenChest();
    }

    public void OpenChest()
    {
        if (isOpened) return;
        isOpened = true;

        if (animator != null)
            animator.SetTrigger("Open");

        if (hasItem && itemPrefab != null)
        {
            Vector3 spawnPos = transform.position + new Vector3(0, spawnOffsetY, 0);
            Instantiate(itemPrefab, spawnPos, Quaternion.identity);
        }
        else
        {
            Debug.Log("Chest is empty!");
        }
    }
}

