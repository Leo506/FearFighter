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


    void FixedUpdate()
    {
       rb2d.velocity = direction * speed * Time.fixedDeltaTime;
    }


    public void SetDir(Vector2 dir) {
    	direction = dir;
    }


    void OnCollisionEnter2D(Collision2D other) {
    	var dir = other.gameObject.GetComponent<IChangingDirection>().ChangePlayerDirection(direction, other.contacts[0].normal, ref availableRebounds);

        if (dir == Vector2.zero) {
            direction = dir;
            inMove = false;
        } else {
            direction = dir;
        }
    }
}
