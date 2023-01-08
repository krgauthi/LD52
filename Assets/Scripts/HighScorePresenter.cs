using System;
using System.Collections;
using System.Collections.Generic;
using LootLocker.Requests;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HighScorePresenter : MonoBehaviour
{
    [SerializeField] TMP_InputField memberIDField;
    [SerializeField] private Image button;
    int leaderboardID = 10373;


    public void Submit()
    {
        var memberID = memberIDField.text;
        if (memberID.Length > 6 && button.color != Color.green)
        {
            LootLockerSDKManager.SubmitScore(memberID, GameStore.State.score, leaderboardID, (response) =>
            {
                if (response.statusCode == 200)
                {
                    Debug.Log("Successful");
                    button.color = Color.green;
                }
                else
                {
                    Debug.Log("failed: " + response.Error);
                }
            });
        }
    }
    
    
}
