using Assembly_CSharp;
using UnityEngine;

public class Gun : Weapons
{
    public int bullets = 6;
    public int gunDamage = 50;
    public ParticleSystem muzzleFlash;
    public AudioSource shootSound;

    protected void Awake()
    {
        Initialize("Gun");
    }

    public void Shoot(Zombie target)
    {
        if (bullets <= 0) return;

        bullets--;

        if (target != null && !target.IsDead())
            target.TakeDamage(gunDamage);

        if (muzzleFlash != null) muzzleFlash.Play();
        if (shootSound != null) shootSound.Play();

        Animator animator = GetComponent<Animator>();
        if (animator != null) animator.SetTrigger("Shoot");
    }

    public override void Attack(Zombie target)
    {
        Shoot(target);
    }
}