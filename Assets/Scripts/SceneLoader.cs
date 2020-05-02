using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {


  void Start() {
      GameObject.Find("Instructions").SetActive(false);
      GameObject.Find("Credits").SetActive(false);
  }

	public void LoadScene(int level)
      { 
         SceneManager.LoadScene(level);
         //.LoadLevel(level);
       }

    void Update() {
    	if (Input.GetKeyDown(KeyCode.R)) {
    		LoadScene(1);
    	}
    }
}
