using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLevel : MonoBehaviour
{
    [SerializeField] private Transform[] levelPart;
    [SerializeField] private Vector3 nextPlatformPosition;

    [SerializeField] private float distanceToSpawn;
    [SerializeField] private float distanceToDelete;
    [SerializeField] private Transform player;

    void Update()
    {
        PlatformDeleter();
        PlatformGenerator();
    }

    private void PlatformGenerator()
    {
        // Check if player is within certain distance to spawn new platform
        while (Vector2.Distance(player.transform.position, nextPlatformPosition) < distanceToSpawn)
        {
            // Makes clone of random platform at the spawn position
            Transform part = levelPart[UnityEngine.Random.Range(0, levelPart.Length)];

            // Makes sure that start point of new platform is at the end point of last platform
            Vector2 newPosition = new Vector2(nextPlatformPosition.x - part.Find("StartPoint").position.x, 0);

            Transform newPart = Instantiate(part, newPosition, transform.rotation, transform);

            nextPlatformPosition = newPart.Find("EndPoint").position;
        }
    }

    private void PlatformDeleter()
    {
        // If level generator has any children, delete it
        if (transform.childCount > 0)
        {
            Transform partToDelete = transform.GetChild(0);

            if (Vector2.Distance(player.transform.position, partToDelete.transform.position) > distanceToDelete)
            {
                Destroy(partToDelete.gameObject);
            }
        }

    }
}