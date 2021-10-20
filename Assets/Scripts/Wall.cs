using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
	// Данная функция нужна для того, чтобы установить edge radius в 0
    public void FixWall() {
    	foreach (var item in this.gameObject.GetComponents<BoxCollider2D>()) {
    		item.edgeRadius = 0;
    		item.enabled = false;
    	}

    	Rigidbody2D rb2d = this.gameObject.GetComponent<Rigidbody2D>();

    	if (rb2d != null)
    		this.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static; 
    }


    public virtual Vector2 ChangePlayerDirection(Vector2 dir, Vector3 normal, ref int rebounds) {
    	if (rebounds > 0) {
    		rebounds--;
    		return (Vector2)Vector3.Reflect(dir, normal).normalized;
    	} else {
    		rebounds = 1;
    		return Vector2.zero;
    	}
    }
}
