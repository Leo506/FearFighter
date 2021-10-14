using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] PlayerInput _input;
    [SerializeField] UnityEngine.UI.Slider _hpSlider;
    public float speed;
    public static bool canMove = false;

    Rigidbody2D rb2D;

    Vector3 direction;

    int multiplier = 1;

    int availableTouches = 1;

    float hp = 100;


    void Start() {
    	rb2D = GetComponent<Rigidbody2D>();
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
    	
    	if (other.gameObject.tag == "Enemy" && canMove) {
    		Debug.Log("Враг!!");
    		Destroy(other.gameObject);
    	}
    	else {

	    	float magnitude = direction.magnitude;
	    	direction = Vector3.Reflect(direction, other.contacts[0].normal) * magnitude;
	    	
	    	if (availableTouches > 0)
	    		availableTouches--;
	    	else {
	    		canMove = false;
	    		availableTouches = 1;
	    	}
    	}

    	Debug.DrawRay((Vector2)this.transform.position, direction, Color.green);
    }

    
    public void SetDirection(Vector2 dir) {
    	direction = dir;
    }

    public void GetDamage(float value) {
    	hp -= value;
    	_hpSlider.value = hp;
    	if (hp <= 0)
    		SceneManager.LoadScene("Prototype");
    }
}
