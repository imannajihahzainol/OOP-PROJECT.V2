using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Boss Settings")]
    public GameObject bossZombie;    // drag the boss GameObject here
    public Transform bossSpawnPoint; // optional: spawn point for boss

    [Header("Pre-requisites")]
    public int totalNormalZombies = 5; // number of normal zombies to kill
    private int zombiesKilled = 0;
    public bool hasSizePotion = false;

    void Start()
    {
        if (bossZombie != null)
            bossZombie.SetActive(false); // hide boss at start
    }

    // Call this when a normal zombie dies
    public void NormalZombieKilled()
    {
        zombiesKilled++;
        TryEnableBoss();
    }

    // Call this when player picks up size potion
    public void PlayerGotSizePotion()
    {
        hasSizePotion = true;
        TryEnableBoss();
    }

    // Check if boss can appear
    private void TryEnableBoss()
    {
        if (zombiesKilled >= totalNormalZombies && hasSizePotion && bossZombie != null)
        {
            bossZombie.transform.position = bossSpawnPoint.position; // optional
            bossZombie.SetActive(true);
            Debug.Log("Boss Zombie has appeared!");
        }
    }
}
