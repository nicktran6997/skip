using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    //Help from https://unity3d.com/learn/tutorials/projects/survival-shooter/more-enemies

    //General Variables.
    private GridManager grid;
    private PlayerController player;
    public GameObject Circle;                // The enemy prefab to be spawned.
    public GameObject FastCircle;
    public GameObject WallCircle;

    //Difficulty Variables.
	private int difficultyLevel;
    private static int DIFFICULTYSCALINGRATE = 5;

    //Normal Spawn Difficulty Variables.
    private static float STARTINGSPAWNINTERVAL = 2f;
    private static float SPAWNINTERVALSCALE = 0.9f;
    private float spawnInterval;
    private static float STARTINGENEMYSPEED = 1f;
    private static float ENEMYSPEEDSCALE = 1.1f;
    private float enemySpeed;

    //Fast Spawn Difficulty Variables.
    private float fastEnemySpeed;
    private static float STARTINGFASTENEMYPROBABILITY = 0.25f;
    private static float FASTENEMYPROBABILITYSCALE = 1.1f;
    private float fastEnemyProbability;
    private static float FASTENEMYMULTIPLIER = 5f;
    private static float FASTENEMYSPAWNINTERVAL = 3f;
    private bool rightSide;

    //Wall Spawn Difficulty Variables.
    private static float STARTINGWALLENEMYSPEED = 1;
    private float wallEnemySpeed;

	void Start ()
	{
        initVariables();
		// Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time.
		InvokeRepeating ("Spawn", 1, spawnInterval);
	}

    private void initVariables() {
        grid = GameObject.Find("Grid").GetComponent<GridManager>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        difficultyLevel = 0;
        spawnInterval = STARTINGSPAWNINTERVAL+.1f;
        enemySpeed = STARTINGENEMYSPEED;
        fastEnemySpeed = enemySpeed * FASTENEMYMULTIPLIER;
        fastEnemyProbability = STARTINGFASTENEMYPROBABILITY;
        wallEnemySpeed = STARTINGWALLENEMYSPEED;
        rightSide = false;
    }


	void Spawn ()
	{
        int maxY = grid.getMaxY();
        float minX = grid.getCC().getMinX() - 2f;
        float maxX = grid.getCC().getMaxX() + 2f;
        float yVal = Random.Range(0, maxY * 2) + 0.5f - maxY;
        if (Random.Range((int)0, (int)2) == 0)
        {
            Instantiate(Circle, new Vector3(minX, yVal, 0), this.transform.rotation);
        }
        else {
            Instantiate(Circle, new Vector3(maxX, yVal, 0), this.transform.rotation);
        }
	}

    void FastEnemySpawn()
    {
        float minX = grid.getCC().getMinX() - 2f;
        float maxX = grid.getCC().getMaxX() + 2f;
        if (Random.Range(0f,1f) < fastEnemyProbability) {
            if (!rightSide)
            {
                Instantiate(FastCircle, new Vector3(minX, player.getYPOS(), 0), this.transform.rotation);
                rightSide = !rightSide;
            }
            else
            {
                Instantiate(FastCircle, new Vector3(maxX, player.getYPOS(), 0), this.transform.rotation);
                rightSide = !rightSide;
            }
        }
        
    }

    void WallEnemySpawn() {
        int maxY = grid.getMaxY();
        float minX = grid.getCC().getMinX() - 2f;
        float maxX = grid.getCC().getMaxX() + 2f;
        if (Random.Range((int)0, (int)2) == 0)
        {
            for (int i = 0; i < maxY * 2; i++)
            {
                float yVal = i + 0.5f - maxY;
                Instantiate(WallCircle, new Vector3(minX, yVal, 0), this.transform.rotation);
            }
        }
        else
        {
            for (int i = 0; i < maxY * 2; i++)
            {
                float yVal = i + 0.5f - maxY;
                Instantiate(WallCircle, new Vector3(maxX, yVal, 0), this.transform.rotation);
            }
        }
    }

    public void increaseDifficulty(){
        CancelInvoke();
		difficultyLevel += 1;
        GameObject.Find("LevelIndicator").GetComponent<Text>().text = ("Level: " + difficultyLevel);
        updateDifficulty();
    }

    private void updateDifficulty() {
        updateSpawn();
        updateFastEnemy();
        updateWallEnemy();
        updateGrid();
        if (difficultyLevel % 2 == 0 && difficultyLevel >= 8) {
            player.giveLife();
        }
        
    }

    private void updateSpawn() {
        spawnInterval = STARTINGSPAWNINTERVAL * Mathf.Pow(SPAWNINTERVALSCALE, difficultyLevel);
        enemySpeed = STARTINGENEMYSPEED * Mathf.Pow(ENEMYSPEEDSCALE, difficultyLevel);
        InvokeRepeating("Spawn", 0, spawnInterval);
    }

    private void updateFastEnemy() {
        if (difficultyLevel % 4 == 0) {
            fastEnemyProbability = STARTINGFASTENEMYPROBABILITY * Mathf.Pow(FASTENEMYPROBABILITYSCALE, difficultyLevel);
            fastEnemySpeed = enemySpeed * FASTENEMYMULTIPLIER;
        }
        InvokeRepeating("FastEnemySpawn", 0, FASTENEMYSPAWNINTERVAL);
    }

    private void updateWallEnemy() {
        if (difficultyLevel >= 8) {
            WallEnemySpawn();
        }
    }

    private void updateGrid() {
        if (difficultyLevel == 2) {
            grid.ScaleUp();
        }
        if (difficultyLevel == 4)
        {
            grid.ScaleUp();
        }
        if (difficultyLevel == 8)
        {
            grid.ScaleUp();
        }
        if (difficultyLevel == 16)
        {
            grid.ScaleUp();
        }
        if (difficultyLevel == 32)
        {
            grid.ScaleUp();
        }
    }

    public int getDifficultyLevel()
    {
        return difficultyLevel;
    }

    public int getScalingRate() {
        return DIFFICULTYSCALINGRATE;
    }

    public float getEnemySpeed() {
        return enemySpeed;
    }

    public float getFastEnemySpeed() {
        return fastEnemySpeed;
    }

    public float getWallEnemySpeed() {
        return wallEnemySpeed;
    }

	//YOU SHOULD ONLY HAVE A METHOD TO INCREASE DIFFICULTY HERE. COUNT COINS IN THE COIN MANAGER
}