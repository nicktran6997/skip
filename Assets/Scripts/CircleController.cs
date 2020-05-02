using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleController : MonoBehaviour {

    protected Rigidbody2D rb;
    protected CameraController myCam;
    private float walkingSpeed;
    private Vector2 walkingVector;
    private EnemyManager em;

    public void Start() {
        initVariables();
    }

    private void initVariables()
    {
        em = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        walkingSpeed = em.getEnemySpeed();
        if (rb.position.x > 0) {
            walkingSpeed *= -1;
        }
        walkingVector = new Vector2(walkingSpeed, 0);
        myCam = Camera.main.GetComponent<CameraController>();
        rb.velocity = walkingVector;
    }

    // Update is called once per frame
    public void FixedUpdate () {
        if (!myCam.inBounds (gameObject.transform.position, 4f)) {
            Destroy(this.gameObject);
        }
    }

    public virtual void HitPlayer(PlayerController player)
    {
        player.TakeDamage(1);
    }

    private void OnTriggerEnter2D(Collider2D player)
    {
        if (player.tag == "Player")
        {
            
            PlayerController playerObj = player.gameObject.GetComponent<PlayerController>();
            if (!playerObj.IsInvincible())
            {
                HitPlayer(playerObj);
                AudioClip clip = GameObject.Find("Damaged").GetComponent<AudioSource>().clip;
                AudioSource.PlayClipAtPoint(clip, Vector3.zero);
            }
        }
    }

}