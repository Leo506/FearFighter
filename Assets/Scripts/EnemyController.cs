using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyController : MonoBehaviour, IChangingDirection, IChangingTime
{
    public Pathfinding.AIDestinationSetter AI;
    [SerializeField] float health;

    // Start is called before the first frame update
    void Start()
    {
        AI = GetComponent<AIDestinationSetter>();
    }


    public void SetTarget(Transform target)
    {
        AI.target = target;
    }


    public Vector2 ChangePlayerDirection(Vector2 dir, Vector3 normal, ref int rebounds) {
        health--;

        if (health > 0) {
            return (Vector2)Vector3.Reflect(dir, normal).normalized;
        }

        return dir;
    }


    public void SlowDownTime() {
        GetComponent<AIPath>().maxSpeed *= 0.25f;
    }


    public void AccelerateTime() {
        GetComponent<AIPath>().maxSpeed *= 4;
    }
}
