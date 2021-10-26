using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
	public int countOfEnemy = 0;
    LvlGenerator generator;
    PlayerMovement move;

    void Start() {
    	generator = FindObjectOfType<LvlGenerator>();
    }

    public void DestroyEnemy(GameObject enemy) {
    	countOfEnemy--;
    	if (countOfEnemy <= 0) {
    		move = FindObjectOfType<PlayerMovement>();
    		generator.SpawnExit();
    		move.SetDir(Vector2.zero);
    	}

    	Destroy(enemy);
    }
}
