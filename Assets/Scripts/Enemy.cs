using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour {

    protected Rigidbody2D rb;
	protected CameraController myCam;
   // protected Animator anim;
    protected Collider2D[] myColliders;
    System.Random rnd = new System.Random();
    public float walkingSpeed;
    protected bool dead = false;
	public bool facingRight;
    private int power;

    // Use this for initialization
    public virtual void Start () {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        //anim = GetComponent<Animator>();
        myColliders = GetComponentsInChildren<Collider2D>();
        walkingSpeed = rnd.Next(2, 7);
        power = 1;
		myCam = Camera.main.GetComponent<CameraController>();
    }

    public virtual void FixedUpdate () {
		
    }

    public virtual void HitPlayer(PlayerController player)
    {
        player.TakeDamage(power);
    }

    private void OnTriggerEnter2D(Collider2D player)
    {
        if(player.tag == "Player")
        {
			AudioClip clip = GameObject.Find ("Damaged").GetComponent<AudioSource> ().clip;
			AudioSource.PlayClipAtPoint (clip, Vector3.zero);
            PlayerController playerObj = player.gameObject.GetComponent<PlayerController>();
            if (!playerObj.IsInvincible())
            {
                HitPlayer(playerObj);
            }
        }
    }

}