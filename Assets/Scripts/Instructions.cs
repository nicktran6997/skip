using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instructions : MonoBehaviour {
	GameObject off;
	GameObject on;
	// Use this for initialization

	public void MenuSelect(GameObject off, GameObject on) {
		off.SetActive(false);
		on.SetActive(true);
	}
	

}
