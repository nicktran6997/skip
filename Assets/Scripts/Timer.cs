using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    float timeLeft = 30.0f;
    public Text winText;
    //CoinManager cm;

    // Use this for initialization
    void Start()
    {
        //cm = GameObject.FindGameObjectWithTag ("UI").GetComponent<CoinManager>();
    }

    // Update is called once per frame

    void Update()
    {
        timeLeft -= Time.deltaTime;
        //float val = Mathf.Round(timeLeft, 2);
        float val = timeLeft;
        GameObject.Find("Time").GetComponent<Text>().text = ("Time: " + val.ToString("F1"));

        /*
        if (false)//)timeLeft < 0)
        {   
            int coinCount = cm.getCoinCount();
            winText = (GameObject.Find("WinText").GetComponent<Text>());
            winText.text = "Congratulations, your coinCount: " + coinCount;
            Time.timeScale = 0.0f;

        }*/
    }
}