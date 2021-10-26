using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
	public float speed;
	public int availableRebounds = 1;
	public bool inMove = false;
    bool roundEnd = false;

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
        if (other.gameObject.tag == "Exit")
            SceneManager.LoadScene("MainScene");


        if (!roundEnd) {
        	var dir = other.gameObject.GetComponent<IChangingDirection>().ChangePlayerDirection(direction, other.contacts[0].normal, ref availableRebounds);

            if (dir == Vector2.zero) {
                direction = dir;
                inMove = false;
            } else {
                direction = dir;
            }
        }
    }


    public void GoToExit() {
        GameObject exitObj = GameObject.FindWithTag("Exit");
        Vector2 dir = (exitObj.transform.position - this.transform.position).normalized;
        roundEnd = true;

        speed *= 0.25f;

        SetDir(dir);
    }
}
