using UnityEngine;

public class Chest : MonoBehaviour
{
    [Header("Optional Key")]
    public string keyType;          // Nama key yang diperlukan, optional

    [Header("Item Settings")]
    public bool hasItem = false;    // Ada item atau tidak
    public GameObject itemPrefab;   // Item spawn bila chest dibuka

    [Header("Animation")]
    public Animator animator;       // Chest Animator

    private bool isOpened = false;  // Cegah chest dibuka lebih dari sekali

    void Start()
    {
        // Ambil animator dari chest sendiri kalau belum assign
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    // Panggil bila player dekat / dari script lain
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isOpened)
            OpenChest();
    }

    public void OpenChest()
    {
        isOpened = true;

        if (animator != null)
            animator.SetTrigger("Open");

        if (hasItem && itemPrefab != null)
            Instantiate(itemPrefab, transform.position + Vector3.up, Quaternion.identity);
        else
            Debug.Log("Chest is empty!");
    }

}
