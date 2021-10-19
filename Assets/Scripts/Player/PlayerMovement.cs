using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float speed;
	public int availableRebounds = 1;
	public bool inMove = false;

	Rigidbody2D rb2d;

	Vector2 direction = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
       rb2d.velocity = direction * speed * Time.deltaTime;
    }


    public void SetDir(Vector2 dir) {
    	direction = dir;
    }


    void OnCollisionEnter2D(Collision2D other) {
    	if (availableRebounds > 0) {
    		availableRebounds--;
    		direction = (Vector2)Vector3.Reflect(direction, other.contacts[0].normal).normalized;
    	} else {
    		direction = Vector2.zero;
    		availableRebounds = 1;
    		inMove = false;
    	}
    }
}
