using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : ReboundObject
{
	public float speed;       // Скорость движения персонажа
    bool inMove = false;      // Определяет, двигается ли персонаж
    public bool roundEnd = false;    // Определяет, зачищен ли уровень

	Rigidbody2D rb2d;
    BoxCollider2D box;


    public delegate void MovementDelegate();
    public event MovementDelegate EndMove;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
    }


    void FixedUpdate()
    {
       rb2d.velocity = direction * speed * Time.fixedDeltaTime;
    }


    public void SetDir(Vector2 dir) {
        if (dir == Vector2.zero  && !roundEnd) {
            EndMove();
            inMove = false;
        }

        Vector2 forCheck;
        ContactPoint2D[] contacts = new ContactPoint2D[10];

        if (Mathf.Abs(dir.x) >= Mathf.Abs(dir.y)) {
            if (dir.x > 0) {
                forCheck = Vector2.right;
            }

            else {
                forCheck = Vector2.left;
            }
        }

        else {
            if (dir.y > 0)
                forCheck = Vector2.up;
            else
                forCheck = Vector2.down;
        }

        int contactsCount = box.GetContacts(contacts);

        for (int i = 0; i < contactsCount; i++) {
            if (contacts[i].normal + forCheck == Vector2.zero) {
                dir *= -1;
                break;
            }
        }

       
    	direction = dir;
        inMove = true;
        
    }


    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Exit")
            SceneManager.LoadScene("MainScene");


        // Если уровень не зачищен и персонаж в движении
        if (!roundEnd && inMove) {

            // Отскакиваем
        	Rebound(other);
            
            if (direction == Vector2.zero) {
                inMove = false;
                EndMove();
            }
        }
    }



    public void GoToExit(GameObject exitObj) {
        Vector2 dir = (exitObj.transform.position - this.transform.position).normalized;
        roundEnd = true;

        speed *= 0.25f;

        SetDir(dir);
    }
}
