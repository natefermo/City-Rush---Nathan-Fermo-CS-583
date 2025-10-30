using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadzoneTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object that entered is the player
        if (collision.GetComponent<Player>() != null)
        {
            // Delay restart so audio can play
            StartCoroutine(RestartWithDelay());
        }
    }

    private IEnumerator RestartWithDelay()
    {
        AudioManager.instance.PlaySFX(2);
        yield return new WaitForSeconds(0.7f);
        GameManager.instance.RestartLevel();
    }
}