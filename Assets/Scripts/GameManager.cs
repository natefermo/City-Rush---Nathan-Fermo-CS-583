using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    // Reference to the player
    public Player player;
    // Furthest distance traveled by player
    public float distance;

    // Singleton instance for easy access from other scripts
    public static GameManager instance;

    private void Awake()
    {
        // Set this as the singleton instance
        instance = this;
    }

    private void Update()
    {
        // Update distance if player moved further
        if (player.transform.position.x > distance)
            distance = player.transform.position.x;
    }

    // Allow player to start moving
    public void UnlockPlayer() => player.playerUnlocked = true;

    // Restart the level
    public void RestartLevel()
    {
        // Save high score before restarting
        SavedInfo();
        // Load the first scene
        SceneManager.LoadScene(0);
    }

    // Save high score to PlayerPrefs
    public void SavedInfo()
    {
        // Only save if current distance is higher than saved high score
        if (PlayerPrefs.GetFloat("HighScore") < distance)
            PlayerPrefs.SetFloat("HighScore", distance);
    }
}