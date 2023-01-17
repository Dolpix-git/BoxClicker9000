using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayfabManager : MonoBehaviour{
    public static PlayfabManager Instance { get; private set; }
    private void Awake(){
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this){
            Destroy(this);
        }
        else{
            Instance = this;
        }
    }

    public event EventHandler<OnLoginEventArgs> OnLogin;


    void Start(){
        Login();
    }

    void Login(){
        var request = new LoginWithCustomIDRequest{
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams{
                GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnError);
    }

    private void OnLoginSuccess(LoginResult result){
        Debug.Log("Logged into API");

        OnLogin?.Invoke(this, new OnLoginEventArgs { result = result});
    }

    public void SubmitName(string name){
        var request = new UpdateUserTitleDisplayNameRequest{
            DisplayName = name,
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);
    }
    private void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result){
        Debug.Log("Update display name");

    }

    private void OnError(PlayFabError error){
        Debug.LogWarning("Something went wrong with the API call.  :(");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }

    public void SendLeaderboard(int score){
        Debug.Log("SendLeaderboard!");
        var request = new UpdatePlayerStatisticsRequest{
            Statistics = new List<StatisticUpdate>{
                new StatisticUpdate{
                    StatisticName = "TetrisScore",
                    Value = score 
                }
            }
        };
        Debug.Log("Request made");
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
        Debug.Log("Request over");
        GetLeaderboard();
    }


    private void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result){
        Debug.Log("Sent Leaderboard Information!");
    }

    public void GetLeaderboard(){
        var request = new GetLeaderboardRequest{
            StatisticName = "TetrisScore",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }

    [SerializeField] GameObject rowPrefab;
    [SerializeField] Transform rowsParent;
    private void OnLeaderboardGet(GetLeaderboardResult result){
        foreach (Transform item in rowsParent){
            Destroy(item.gameObject);
        }

        foreach (var item in result.Leaderboard){
            GameObject newGo = Instantiate(rowPrefab, rowsParent);
            TextMeshProUGUI[] texts = newGo.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = item.Position.ToString();
            texts[1].text = item.DisplayName.ToString();
            texts[2].text = item.StatValue.ToString();

            Debug.Log(item.Position + " " + item.DisplayName + " " + item.StatValue);
        }
    }
}
public class OnLoginEventArgs : EventArgs
{
    public LoginResult result;
}

