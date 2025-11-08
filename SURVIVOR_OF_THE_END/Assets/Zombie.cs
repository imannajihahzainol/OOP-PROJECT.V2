using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;

namespace Assets
{
    public class Zombie : MonoBehaviour
    {
        public int speed = 1;
        public int health = 100;
        public int damage = 10;

        // Base movement for normal zombies
        public virtual void ChasePlayer(Transform player)
        {
            if (player == null) return;
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }

        // Basic attack (for now, just display log)
        public virtual void AttackPlayer()
        {
            Debug.Log(name + " attacks player for " + damage + " damage!");
        }

        // Take damage and check if dead
        public virtual void TakeDamage(int amount)
        {
            health -= amount;
            Debug.Log(name + " took " + amount + " damage. Remaining health: " + health);

            if (IsDead())
            {
                Debug.Log(name + " is dead!");
                Destroy(gameObject); // remove from scene
            }
        }

        // Check if zombie health reaches 0
        public virtual bool IsDead()
        {
            return health <= 0;
        }

    }
}
