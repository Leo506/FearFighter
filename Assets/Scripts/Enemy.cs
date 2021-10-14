using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    bool isFighting = false;

    void OnCollisionEnter2D(Collision2D other) {
    	if (other.gameObject.tag == "Player" && !isFighting)
    		StartCoroutine(Fighting(other.gameObject.GetComponent<Player>()));
    }

    void OnCollisionStay2D(Collision2D other) {
		if (other.gameObject.tag == "Player" && !isFighting)
    		StartCoroutine(Fighting(other.gameObject.GetComponent<Player>()));
    }

    IEnumerator Fighting(Player player) {
    	isFighting = true;
    	player.GetDamage(10);
    	yield return new WaitForSeconds(1);
    	isFighting = false;
    }
}
