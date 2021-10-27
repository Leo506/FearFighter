using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
	public float speed;
	public int availableRebounds = 1;
	public bool inMove = false;  //TODO: убрать
    bool roundEnd = false;

	Rigidbody2D rb2d;

	Vector2 direction = Vector2.zero;

    public delegate void MovementDelegate();
    public event MovementDelegate EndMove;

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
        if (dir == Vector2.zero)
            EndMove();

        Vector2 forCheck;
        ContactPoint2D[] contacts = new ContactPoint2D[10];
        BoxCollider2D box = GetComponent<BoxCollider2D>();

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
        Debug.Log("Dir: " + dir);
        
    }


    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Exit")
            SceneManager.LoadScene("MainScene");


        if (!roundEnd) {
        	var dir = other.gameObject.GetComponent<IChangingDirection>().ChangePlayerDirection(direction, other.contacts[0].normal, ref availableRebounds);
            Debug.Log("Меняем направление");
            if (dir == Vector2.zero) {
                direction = dir;
                inMove = false;
                EndMove();
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
