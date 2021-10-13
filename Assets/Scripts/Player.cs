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
    	
    }

    void FixedUpdate() {
    	if (canMove) {
    		rb2D.velocity = (Vector2)direction * speed * Time.fixedDeltaTime;
    		Debug.DrawRay((Vector2)this.transform.position, direction, Color.red);
    		Debug.Log("Движется? " + canMove);
    	} else
    		rb2D.velocity = Vector2.zero;
    }

    void OnCollisionEnter2D(Collision2D other) {
    	
    	float magnitude = direction.magnitude;
    	direction = Vector3.Reflect(direction, other.contacts[0].normal) * magnitude;
    	
    	if (availableTouches > 0)
    		availableTouches--;
    	else {
    		canMove = false;
    		availableTouches = 1;
    	}

    	Debug.DrawRay((Vector2)this.transform.position, direction, Color.green);
    }

    public void SetDirection(Vector2 dir) {
    	direction = dir;
    }
}
