using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] protected float chanceToSpawn = 60;
    protected virtual void Start()
    {
        // Trap will spawn if random number is less than or equal to 60
        bool canSpawn = chanceToSpawn >= Random.Range(0, 100);

        if (!canSpawn)
        {
            Destroy(gameObject);
        }

    }

    // Allows method to be inherited
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        // Get the Player component from the object that entered the trigger
        Player player = collision.GetComponent<Player>();

        if (player != null)
        {
            // Call the Death coroutine on the Player
            player.StartCoroutine(player.Death());
        }
    }
}