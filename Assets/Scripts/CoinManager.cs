using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour {

    protected GridManager grid;
	public GameObject coin;
    private EnemyManager enemyManager;
    private bool firstSpawn;
	private int coinCount;

	// Use this for initialization
	void Start () {
        initVariables();
	}

    private void initVariables() {
        grid = GameObject.Find("Grid").GetComponent<GridManager>();
        enemyManager = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
        coinCount = 0;
        firstSpawn = false;
    }

	public void Spawn() {
        int maxY = grid.getMaxY();
        float minX = grid.getCC().getMinX() + 0.5f;
        float maxX = grid.getCC().getMaxX() - 0.5f;
        float yVal = Random.Range(0, maxY * 2) + 0.5f - maxY;
        float xVal = Random.Range(minX, maxX);

        Instantiate (coin, new Vector3(xVal, yVal, 0), this.transform.rotation);
	}
	
	public void coinIncrement() {
		coinCount++;
		GameObject.Find("CoinText").GetComponent<Text>().text = ("x " + coinCount);
		if (coinCount % enemyManager.getScalingRate() == 0) {
            enemyManager.increaseDifficulty();
            grid.RandomizeColors();
		}
	}

	public int getCoinCount() {
		return coinCount;
	}

	// Update is called once per frame
	void Update () {
        if (!firstSpawn) {
            Spawn();
            firstSpawn = true;
        }
	}
}
