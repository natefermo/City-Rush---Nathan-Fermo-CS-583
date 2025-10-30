using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UI_InGameScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI distanceText;
    private float distance = 0;

    void Start()
    {
        // Update the distance info every 0.15 seconds
        InvokeRepeating("UpdateInfo", 0, 0.15f);
    }

    private void UpdateInfo()
    {
        distance = GameManager.instance.distance;
        if (distance > 0)
            distanceText.text = distance.ToString("#,#") + " M";

    }
}