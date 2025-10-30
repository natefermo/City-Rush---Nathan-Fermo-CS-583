using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backgroundParallax : MonoBehaviour
{
    private float startPosition, length;
    public GameObject cam;
    public float parallaxEffect;

    void Start()
    {
        // Store the initial position of the background
        startPosition = transform.position.x;

        // Get the width of the background sprite for looping
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        // Calculate the distance background moves based on camera movement
        float distance = cam.transform.position.x * parallaxEffect; // 0 = moves with camera, 1 = stays static
        float movement = cam.transform.position.x * (1 - parallaxEffect);

        // Move the background based on parallax calculation
        transform.position = new Vector3(startPosition + distance, transform.position.y, transform.position.z);

        // If background reached end of its length, adjust its position for infinite scrolling
        if (movement > startPosition + length)
        {
            startPosition += length;
        }
        else if (movement < startPosition - length)
        {
            startPosition -= length;
        }
    }
}