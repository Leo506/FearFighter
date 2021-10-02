﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] PlayerInput _input;
    [SerializeField] Canvas _chooseCanvas;
    GameObject item;
    Rigidbody2D rb2D;
    BoxCollider2D bx;
    RoomGenerator generator;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        bx = GetComponent<BoxCollider2D>();
        generator = FindObjectOfType<RoomGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        var dir = _input.GetSwipe() * generator.xOffset;
        if (dir != Vector2.zero)
        {
            if (AttemptMove(dir))
                rb2D.MovePosition(rb2D.position + dir);
        }
    }

    bool AttemptMove(Vector2 dir)
    {
        Vector2 newPos = rb2D.position + dir;
        bx.enabled = false;
        RaycastHit2D hit = Physics2D.Linecast(rb2D.position, newPos);
        bx.enabled = true;

        if (hit.transform != null)
        {
            if (hit.collider.gameObject.tag == "Item")
            {
                Time.timeScale = 0;
                _chooseCanvas.enabled = true;
                item = hit.collider.gameObject;
            }
            return false;
        }

        return true;
    }

    public void ContinueGame()
    {
        Time.timeScale = 1;
        _chooseCanvas.enabled = false;
        Destroy(item);
    }
}
