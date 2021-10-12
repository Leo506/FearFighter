using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] PlayerInput _input;
    public float speed;
    public static bool canMove = false;

    Rigidbody2D rb2D;

    Vector3 direction;

    int multiplier = 1;

    int availableTouches = 1;

    void Start() {
    	rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
    	direction = _input.GetDir() * multiplier;
        if (direction != Vector3.zero && canMove) {
        	rb2D.MovePosition(rb2D.position + (Vector2)direction * speed * Time.deltaTime);
        }
        Debug.Log(direction);
    }

    void OnCollisionEnter2D(Collision2D other) {
    	multiplier *= -1;
    	
    	if (availableTouches > 0)
    		availableTouches--;
    	else {
    		canMove = false;
    		availableTouches = 1;
    	}
    }
}
