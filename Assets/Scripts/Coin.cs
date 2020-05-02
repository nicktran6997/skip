using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Collectible {
	Collectible parent;
	CoinManager cm;
	// Use this for initialization
	void Start () {
		cm = GameObject.FindGameObjectWithTag ("UI").GetComponent<CoinManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D player) {
		
		if (player.gameObject.tag == "Player") {
			AudioClip clip = GameObject.Find ("CoinCollect").GetComponent<AudioSource> ().clip;
			AudioSource.PlayClipAtPoint (clip, Vector3.zero);
			cm.coinIncrement();
			cm.Spawn();
			Destroy(this.gameObject);
		}
	}
}
