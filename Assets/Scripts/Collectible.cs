using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



/* A collectible should be an item that can only be 
 * picked up by the player and should affect a
 * specific UI element specified. That UI element
 * should be incremented and yield certain results
 * for the player depending on what its desired 
 * to be.
 * 
 */
public class Collectible : MonoBehaviour {

	protected UIManager ui;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void onTriggerEnter2D(Collider2D collider) {

	}
}
