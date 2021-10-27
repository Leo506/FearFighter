using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyController : MonoBehaviour, IChangingDirection, IChangingTime
{
    public AIDestinationSetter AI;
    [SerializeField] float health;
    EnemyControl control;

    // Start is called before the first frame update
    void Start()
    {
        AI = GetComponent<AIDestinationSetter>();
        control = FindObjectOfType<EnemyControl>();
    }


    public void SetTarget(Transform target)
    {
        AI.target = target;
    }


    public Vector2 ChangePlayerDirection(Vector2 dir, Vector3 normal, ref int rebounds) {
        health --;

        if (health > 0) {
            return (Vector2)Vector3.Reflect(dir, normal).normalized;
        }

        return dir;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (health <= 0) {
                control.DestroyEnemy(this.gameObject);
            }
        }
    }


    public void SlowDownTime() {
        GetComponent<AIPath>().maxSpeed *= 0.25f;
    }


    public void AccelerateTime() {
        GetComponent<AIPath>().maxSpeed *= 4;
    }
}
