using UnityEngine;

public class FloatingZombie : Zombie
{
    [Header("Floating Zombie Settings")]
    public float floatHeight = 0.5f;
    public float floatSpeed = 2f;
    private Vector3 startPos;

    protected override void Start()
    {
        base.Start();
        startPos = transform.position;
        Debug.Log($"{name} started floating at height {floatHeight}.");
    }

    void Update()
    {
        if (player == null) return;

        // Floating up/down motion
        transform.position = new Vector3(
            transform.position.x,
            startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight,
            transform.position.z
        );
    }

    public void FloatAround()
    {
        Debug.Log($"{name} is floating at height {floatHeight}");
    }
}
