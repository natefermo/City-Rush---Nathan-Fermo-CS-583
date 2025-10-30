using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_MainScript : MonoBehaviour
{
    private bool gamePause;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private TextMeshProUGUI highScore;
    void Start()
    {
        SwitchMenuPage(mainMenu);
        Time.timeScale = 1;

        highScore.text = PlayerPrefs.GetFloat("HighScore", 0).ToString("#,#") + " M";
    }

    public void SwitchMenuPage(GameObject uiMenu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        uiMenu.SetActive(true);
        AudioManager.instance.PlaySFX(3);
    }

    public void StartGame() => GameManager.instance.UnlockPlayer();

    public void PauseGame()
    {
        if (gamePause)
        {
            Time.timeScale = 1;
            gamePause = false;
        }
        else
        {
            Time.timeScale = 0;
            gamePause = true;
        }
    }

    public void RestartGame()
    {
        GameManager.instance.RestartLevel();
    }
}
