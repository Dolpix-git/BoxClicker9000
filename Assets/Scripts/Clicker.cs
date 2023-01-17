using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clicker : MonoBehaviour{
    [SerializeField] float timeTillDeath;
    float killTime;

    private void Start(){
        killTime = Time.realtimeSinceStartup + timeTillDeath;
    }
    private void Update(){
        if (Time.realtimeSinceStartup >= killTime){
            GameManager.Instance.StrikesRemaining -= 1;
        }
    }
    void OnMouseDown(){
        Debug.Log("Click!");
        GameManager.Instance.Score++;
        Destroy(gameObject);
    }
}
