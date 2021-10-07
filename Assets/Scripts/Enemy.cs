using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    Player target;
    float hp = 100;
   
    public void InitEnemy()
    {
        target = GetComponent<AIDestinationSetter>().target.GetComponent<Player>();
        StartCoroutine(CheckDistance());
    }

    public void GetDamage(float value)
    {
        hp -= value;
        if (hp <= 0)
        {
            target.SetAllEnemiesInArray();
            Destroy(this.gameObject);
        }
    }

    IEnumerator CheckDistance()
    {
        while (target.brave > 0)
        {
            if ((Vector2.Distance(this.transform.position, target.gameObject.transform.position) <= 1f))
                target.GetDamage(10);

            yield return new WaitForSeconds(1);
        }
    }

   
}
