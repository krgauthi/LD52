using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using TMPro;

public class LeaderBoardPresenter : MonoBehaviour
{

    int leaderboardID = 10373;
    
    [SerializeField] private TMP_Text lead1;
    [SerializeField] private TMP_Text lead2;
    [SerializeField] private TMP_Text lead3;
    [SerializeField] private TMP_Text lead4;
    [SerializeField] private TMP_Text lead5;
    [SerializeField] private TMP_Text lead6;
    [SerializeField] private TMP_Text lead7;
    [SerializeField] private TMP_Text lead8;
    [SerializeField] private TMP_Text lead9;
    [SerializeField] private TMP_Text lead10;
    void Start()
    {
        if (lead1 != null)
        {
            lead1.text = "";
            lead2.text = "";
            lead3.text = "";
            lead4.text = "";
            lead5.text = "";
            lead6.text = "";
            lead7.text = "";
            lead8.text = "";
            lead9.text = "";
            lead10.text = "";
        }

        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (!response.success)
            {
                Debug.Log("error starting LootLocker session" + response.statusCode);

                
                
                return;
            }
            
            if (lead1 != null)
            {
                fetchLeaderboard();
            } 

            
            Debug.Log("successfully started LootLocker session");
        });


        
        
    }


    void fetchLeaderboard()
    {
        LootLockerSDKManager.GetScoreList(leaderboardID, 10, 0, (response) =>
        {
            if (response.statusCode == 200) {
                Debug.Log("Successful");
                lead1.text = "1. "+response.items[0].member_id + " " + response.items[0].score.ToString("#,#");
                lead2.text = "2. "+response.items[1].member_id + " " + response.items[1].score.ToString("#,#");
                lead3.text = "3. "+response.items[2].member_id + " " + response.items[2].score.ToString("#,#");
                lead4.text = "4. "+response.items[3].member_id + " " + response.items[3].score.ToString("#,#");
                lead5.text = "5. "+response.items[4].member_id + " " + response.items[4].score.ToString("#,#");
                lead6.text = "6. "+response.items[5].member_id + " " + response.items[5].score.ToString("#,#");
                lead7.text = "7. "+response.items[6].member_id + " " + response.items[6].score.ToString("#,#");
                lead8.text = "8. "+response.items[7].member_id + " " + response.items[7].score.ToString("#,#");
                lead9.text = "9. "+response.items[8].member_id + " " + response.items[8].score.ToString("#,#");
                lead10.text = "10. "+response.items[9].member_id + " " + response.items[9].score.ToString("#,#");
            } else {
                Debug.Log("failed: " + response.Error);
                lead1.text = "";
                lead2.text = "";
                lead3.text = "";
                lead4.text = "";
                lead5.text = "";
                lead6.text = "";
                lead7.text = "";
                lead8.text = "";
                lead9.text = "";
                lead10.text = "";
            }
        });
    }
    
    
    
}