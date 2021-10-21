using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyController : MonoBehaviour
{
    public Pathfinding.AIDestinationSetter AI;
    [SerializeField] float health;

    // Start is called before the first frame update
    void Start()
    {
        AI = GetComponent<AIDestinationSetter>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void SetTarget(Transform target)
    {
        AI.target = target;
    }
}
