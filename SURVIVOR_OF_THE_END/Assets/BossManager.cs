using UnityEngine;

public class BossManager : MonoBehaviour
{
    public GameObject survivedUI; // drag your Survived UI here

    private void OnEnable()
    {
        BossZombie.OnBossDead += HandleBossDead;
    }

    private void OnDisable()
    {
        BossZombie.OnBossDead -= HandleBossDead;
    }

    private void Start()
    {
        if (survivedUI != null)
            survivedUI.SetActive(false); // hide at start
    }

    private void HandleBossDead()
    {
        Debug.Log("Boss defeated — showing SURVIVED UI");

        if (survivedUI != null)
            survivedUI.SetActive(true);
        else
            Debug.LogWarning("Survived UI not assigned!");
    }
}

