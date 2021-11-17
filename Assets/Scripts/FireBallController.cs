﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallController : MonoBehaviour
{
    public float speed = 100;
    Vector2 direction = Vector2.zero;

    Rigidbody2D rb2d;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.isKinematic = true;
        rb2d.useFullKinematicContacts = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb2d.velocity = direction * speed * Time.fixedDeltaTime;
    }

    public void SetDir(Vector2 dir)
    {
        direction = dir;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag == "Player")
        {
            collision.collider.GetComponent<PlayeLogic>().GetDamage(25);
            Destroy(this.gameObject);
        } 
        else if (collision.collider.gameObject.GetComponent<BossController>() == null && collision.collider.gameObject.GetComponent<FireBallController>() == null)
            Destroy(this.gameObject);
    }
}