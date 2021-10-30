using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

enum EnemyStates {
    GO_TO_PLAYER,
    DASH_INTO_PLAYER,
    RETURN_TO_START_POINT
}

public class EnemyController : MonoBehaviour, IChangingDirection, IChangingTime
{
    public AIDestinationSetter AI;
    [SerializeField] float health;
    EnemyControl control;

    AIPath path;
    EnemyStates state = EnemyStates.GO_TO_PLAYER;
    Rigidbody2D rb2d;

    Vector2 startDashPoint;  // Точка, с которой враг начинает свой "дэш"
    Vector2 direction;

    public DroppingItem[] items;  // Выпадающие предметы

    // Start is called before the first frame update
    void Start()
    {
        AI = GetComponent<AIDestinationSetter>();
        control = FindObjectOfType<EnemyControl>();
        path = GetComponent<AIPath>();
        rb2d = GetComponent<Rigidbody2D>();
    }


    void FixedUpdate() {

        if (state == EnemyStates.GO_TO_PLAYER) {
            if (Vector2.Distance(transform.position, AI.target.transform.position) <= path.endReachedDistance)
                Dash();
        }

        // Если враг "дэшиться"
        if (state == EnemyStates.DASH_INTO_PLAYER || state == EnemyStates.RETURN_TO_START_POINT) {

            rb2d.velocity = direction * path.maxSpeed * 100 * Time.fixedDeltaTime;
        }

        if (state == EnemyStates.RETURN_TO_START_POINT) {
            if (Vector2.Distance(transform.position, startDashPoint) < 0.1f) {
                path.canMove = true;
                state = EnemyStates.GO_TO_PLAYER;
            }
        }
    }


    public void Dash() {
        state = EnemyStates.DASH_INTO_PLAYER;
        path.canMove = false;
        direction = (AI.target.transform.position - transform.position).normalized;
        startDashPoint = transform.position;
    }


    public void SetTarget(Transform target)
    {
        AI.target = target;
    }


    public Vector2 ChangePlayerDirection(Vector2 dir, Vector3 normal, ref int rebounds) {
        health -= 10;

        return Vector2.Reflect(dir, normal).normalized;

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (health <= 0)
                control.DestroyEnemy(this);
        }


        if (state == EnemyStates.DASH_INTO_PLAYER && collision.gameObject.tag == "Player") {
            direction = (startDashPoint - (Vector2)transform.position).normalized;
            state = EnemyStates.RETURN_TO_START_POINT;
        } else {
            state = EnemyStates.GO_TO_PLAYER;
            path.canMove = true;
        }
    }


    public void SlowDownTime() {
        path.maxSpeed *= 0.25f;
    }


    public void AccelerateTime() {
        path.maxSpeed *= 4;
    }

    public void Init()
    {
        EnemyControl.countOfEnemy++;
    }
}
