using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameOverWindow : MonoBehaviour {
   
    private Rect windowRect;
    private static float height;//50
    private static float width;//200

    void Start() {
    	initializeRectangles();
    }
    void OnGUI() {
        windowRect = GUI.Window(0, windowRect, DoMyWindow, "GAME OVER");
        GUI.skin.box.wordWrap = true;

       	GUI.skin.box.fontSize = Screen.width / 40;
       	GUI.skin.button.fontSize = Screen.width / 20;
    }
    void DoMyWindow(int windowID) {
    	if (windowID == 0) {
    		Rect rect = windowRect;
    		int level = GameObject.Find("EnemyManager").GetComponent<EnemyManager>().getDifficultyLevel();
    		int coins = GameObject.Find("CoinManager").GetComponent<CoinManager>().getCoinCount();

    		GUI.Box(new Rect(rect.width/16, rect.height/4, rect.width*3/8, rect.height/4), "Your Level: \n" + level);
    		GUI.Box(new Rect(rect.width*9/16, rect.height/4, rect.width*3/8, rect.height/4), "Your Coins: \n " + coins);
    		if (GUI.Button(new Rect(rect.width/8, rect.height*5/8, rect.width*3/4, rect.height/4), "Restart")) {
           		SceneManager.LoadScene(0);
    		}
    	}
    }



    private void initializeRectangles() {
    	height = Screen.height / 2;
    	width = Screen.width / 2;
    	windowRect = new Rect(Screen.width/2 - width/2, Screen.height/2-height/2, width, height);
    	
    }

}
