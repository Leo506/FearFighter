using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
	public static int countOfEnemy = 0;

    public void DestroyEnemy(EnemyController enemy) {
    	countOfEnemy--;

        // При смерти врага спавним выпадающие предметы
        foreach (var item in enemy.items)
        {
            var drop = Instantiate(item);
            drop.transform.position = enemy.transform.position;
            drop.Init();
        }
        Destroy(enemy.gameObject);

        if (countOfEnemy <= 0)
        {
            FindObjectOfType<GameController>().ChangeState(GameState.WAITING_DROP);
        }
        Debug.Log("Count of enemies: " + countOfEnemy);
    }
}
