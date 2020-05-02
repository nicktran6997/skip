using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

	//Player Component Variables.
    public ActionState myState;
    protected Rigidbody2D rb;
    protected Animator anim;

	//Player State Variables.
    protected static float WALKINGSPEED = 5;
	protected Vector2 walkingVector;
	protected Vector3 rotationVector;
    protected int hp;
    protected bool invincible = false;
	protected float yPos;
	protected static bool IsInputEnabled = true;

	//Grid Variables.
	protected GridManager grid;


    // Use this for initialization
    void Start() {
		this.gameObject.SetActive(true);
		initVariables ();
        rb.freezeRotation = true;
		rb.velocity = walkingVector;

    }

    // Update is called once per frame
    void Update() {
        if (hp <= 0) {
            //print(hp);
            this.gameObject.SetActive(false);
            //string str = "Better Luck Next Time";
            //GameObject.Find("WinText").GetComponent<Text>().text = str;
            //Time.timeScale = 0.0f;
            GameObject.Find("Main Camera").AddComponent<GameOverWindow>();
            Destroy(GameObject.Find("Canvas"));
        }

        //Add the horizontal skip command down this if/else clause
        if (IsInputEnabled){
	        if (Input.GetKeyDown(KeyCode.DownArrow)) {
	            VerticalDash(-1);
	        } else if (Input.GetKeyDown(KeyCode.UpArrow)) {
	            VerticalDash(1);
	        } else if (Input.GetKeyDown(KeyCode.Space)) {
	            HorizontalDash(1.65f);
	        } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
	            if (walkingVector.x < 0) {
	                switchDirection();
	            }
	        } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
	            if (walkingVector.x > 0) {
	                switchDirection();
	            }
	        } 
        }

        this.gameObject.transform.GetChild(0).Rotate(rotationVector);
    }

	//Initializes Variables.
	private void initVariables(){
		grid = GameObject.Find ("Grid").GetComponent<GridManager>();
		rb = gameObject.GetComponent<Rigidbody2D>();
		walkingVector = new Vector2(WALKINGSPEED, 0);
		rotationVector = Vector3.forward * 10;
		hp = 3;
		yPos = 0.5f;


	}

    //For dashing up/downwards
    void VerticalDash(float direction) {
		if (direction > 0) {
			if(grid.getCC().inBounds(new Vector3(transform.position.x, yPos + 1), 0)){
				yPos += 1;
			}
		} else {	
			if(grid.getCC().inBounds(new Vector3(transform.position.x, yPos - 1), 0)){
				yPos -= 1;
			}
		}
		transform.position = new Vector3 (transform.position.x, yPos);
    }

    void HorizontalDash(float distance) {
        /*if (walkingVector.x > 0)
        {
            this.transform.Translate(distance, 0, 0);
        }
        else {
            this.transform.Translate(-distance, 0, 0);
        }*/
		StartCoroutine ("skipping");
		skipSound ();
    }

    public void OnCollisionEnter2D(Collision2D coll) {
        String tag = coll.gameObject.tag;
        if (tag == "Wall") {
			switchDirection ();
        }
    }

    public void TakeDamage(int power) {
        hp = hp - power;
        GameObject.Find("Life").GetComponent<Text>().text = "x " + hp.ToString();
        StartCoroutine("DamageTaken");
    }

	IEnumerator skipping(){
		setWalkSpeed (WALKINGSPEED * 10);
		invincible = true;
		IsInputEnabled = false;
        //yield return new WaitForSeconds (0.015f);
        int i = 0;
        while (i < 1) {
            i++;
            yield return null;
        }
        IsInputEnabled = true;
        invincible = false;
		setWalkSpeed (WALKINGSPEED);
	}

    IEnumerator DamageTaken() {
		setWalkSpeed (WALKINGSPEED / 2);
        this.transform.localScale = new Vector3(0.5f, 0.5f, 0);
        this.GetComponent<TrailRenderer>().startWidth = this.GetComponent<TrailRenderer>().startWidth / 2;
        invincible = true;
        yield return new WaitForSeconds(1f);
        invincible = false;
		setWalkSpeed(WALKINGSPEED);
        this.transform.localScale = new Vector3(1f, 1f, 0);
        this.GetComponent<TrailRenderer>().startWidth = this.GetComponent<TrailRenderer>().startWidth * 2;
    }

	//Switches the direction of the Player.
	private void switchDirection(){
		walkingVector.Set (walkingVector.x * -1, 0);
		updateVelocity ();
	}

	//Sets the walking speed to the newSpeed; Should be positive number. Direction is adjusted for.
	private void setWalkSpeed(float newSpeed){
		float direction = walkingVector.x / Math.Abs (walkingVector.x);
		walkingVector.Set (direction * newSpeed, 0);
		updateVelocity ();
	}

	//This is necessary because Unity treats vectors as primitives.
	private void updateVelocity(){
		rb.velocity = walkingVector;
	}

    public Boolean IsInvincible() {
        return invincible;
    }

    public float getYPOS() {
        return yPos;
    }

    public void giveLife() {
        hp += 1;
        GameObject.Find("Life").GetComponent<Text>().text = "x " + hp.ToString();
    }

	private void skipSound(){
		AudioClip clip = GameObject.Find ("Skip").GetComponent<AudioSource> ().clip;
		AudioSource.PlayClipAtPoint (clip, Vector3.zero);
	}
	/*
	private static void PauseGame(bool pause) {
		if (pause) {
			Time.timeScale = 0.0f;
		} else {
			Time.timeScale = 1.0f;
		}
	}
	*/

}

