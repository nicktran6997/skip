using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMenu : MonoBehaviour {
	float minY, maxY;
	float deltaY;

	string[] options;
	int currIndex;
	//error bounds
	float e = .000005f;


	void Start() {
		maxY = GameObject.Find("StartButton").transform.position.y;
		minY = GameObject.Find("CreditsButton").transform.position.y;
		deltaY = (maxY-minY) / 2;
		options = new string[] {"StartButton", "InstructionsButton", "CreditsButton"};
		currIndex = 0;
	}

	void Update () {
		float y = this.transform.position.y;
		if (Input.GetKeyDown(KeyCode.DownArrow) && y > minY + e) {
			this.transform.Translate(0, -deltaY, 0, Camera.main.transform);
			currIndex++;
		} else if (Input.GetKeyDown(KeyCode.UpArrow) && y < maxY-e) {
			this.transform.Translate(0, deltaY, 0, Camera.main.transform);
			currIndex--;
		} else if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) {
			GameObject.Find(options[currIndex]).GetComponent<Button>().onClick.Invoke();
		} 
	}

}
