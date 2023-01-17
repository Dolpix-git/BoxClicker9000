using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour{
    private int score;
    float time;
    float strikesRemaining = 3;

    [SerializeField] GameObject clicks;
    [SerializeField] AnimationCurve curve;
    [SerializeField] GameObject parent;

    private bool gameInPlay = false;
    private float waitUntil;

    public event EventHandler<OnGameOverEventArgs> OnGameOver;
    public event EventHandler<OnGameOverEventArgs> OnGameStart;

    public static GameManager Instance { get; private set; }
    public float StrikesRemaining { get => strikesRemaining; set => strikesRemaining = value; }
    public int Score { get => score; set => score = value; }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Update(){
        if (strikesRemaining > 0 && gameInPlay){
            time += Time.deltaTime;
            if (waitUntil < Time.realtimeSinceStartup){
                Debug.Log("Spawn Somthing quick!");
                GameObject c = Instantiate(clicks,new Vector3(UnityEngine.Random.Range(0,20), UnityEngine.Random.Range(0, 20),0),Quaternion.identity);
                c.transform.parent = parent.transform;
                waitUntil = Time.realtimeSinceStartup + curve.Evaluate(time);
            }
        }else if(gameInPlay){
            GameOver();
            gameInPlay = false;
        }
    }


    public void GameOver(){
        Debug.Log("Game is over");
        PlayfabManager.Instance.SendLeaderboard(score);
        foreach (Transform c in parent.transform){
            Destroy(c.gameObject);
        }
        OnGameOver?.Invoke(this, new OnGameOverEventArgs { score = score });
    }

    public void StartGame(){
        Debug.Log("Start Game");
        score = 0;
        OnGameStart.Invoke(this, new OnGameOverEventArgs { score = score});
        strikesRemaining = 3;
        time = 0;
        waitUntil = Time.realtimeSinceStartup;
        gameInPlay = true;
    }
}

public class OnGameOverEventArgs : EventArgs{
    public int score;
}
