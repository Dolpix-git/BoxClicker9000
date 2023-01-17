using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class UIManager : MonoBehaviour{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject leaderboard;

    [SerializeField] private TextMeshProUGUI displayName;

    private void Start(){
        GameManager.Instance.OnGameOver += DisplayLeaderboard;
        GameManager.Instance.OnGameStart += CloseWindows;
        PlayfabManager.Instance.OnLogin += AfterLogin;
    }

    private void AfterLogin(object sender, OnLoginEventArgs e){
        toggleMainMenu(true);
    }

    private void Update(){
        if (Input.GetKeyDown(KeyCode.Escape)){
            toggleMainMenu(!mainMenu.activeSelf);
        }
    }

    private void toggleMainMenu(bool value){
        Debug.Log("Main menu is " + value);
        mainMenu.SetActive(value);
        CloseLeaderboard();



        if (displayName.text.Length < 3){
            PlayfabManager.Instance.SubmitName("Chump");
        }else{
            PlayfabManager.Instance.SubmitName(displayName.text);
        }
    }

    private void CloseProgram(){
        //Too do: close
    }
    private void CloseWindows(object sender, OnGameOverEventArgs e)
    {
        mainMenu.SetActive(false);
        leaderboard.SetActive(false);
    }
    public void DisplayLeaderboard(object sender, OnGameOverEventArgs e){
        PlayfabManager.Instance.SendLeaderboard(e.score);
        leaderboard.SetActive(true);
        //Too do: generate UI
        //PlayfabManager.Instance.GetLeaderboard();
    }
    public void DisplayLeaderboard(){
        leaderboard.SetActive(true);
        //Too do: generate UI
        //PlayfabManager.Instance.GetLeaderboard();
    }
    public void CloseLeaderboard(){
        leaderboard.SetActive(false);
    }
}
