using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


// Возможные состояния врага
enum EnemyStates {
    GO_TO_PLAYER,
    DASH_INTO_PLAYER,
    RETURN_TO_START_POINT
}

public class EnemyController : MonoBehaviour, IChangingDirection, IChangingTime
{
    public AIDestinationSetter AI;                                      // Компонент для установки цели, за которой нужно двигаться
    public float health = 10;                                           // Количество здоровья врага
    public float damage = 10;                                           // Сколько урона наносит враг по персонажу
    EnemyControl control;                                               // Объкт, следящий за количеством врагов на сцене

    AIPath path;

    EnemyStates state = EnemyStates.GO_TO_PLAYER;                       // Текущее состояние врага
    
    Rigidbody2D rb2d;

    Vector2 startDashPoint;                                             // Точка, с которой враг начинает свой "дэш"
    Vector2 direction;                                                  // Направление на игрока (используется во время "дэша")

    public DroppingItem[] items;                                        // Выпадающие предметы


    // Start is called before the first frame update
    void Start()
    {
        AI = GetComponent<AIDestinationSetter>();
        control = FindObjectOfType<EnemyControl>();
        path = GetComponent<AIPath>();
        rb2d = GetComponent<Rigidbody2D>();
    }


    void FixedUpdate()
    {
        // Если находимся в состоянии следования за игроком
        if (state == EnemyStates.GO_TO_PLAYER) {

            // И дистанция до него меньше определённого значения - "дэш"
            if (Vector2.Distance(transform.position, AI.target.transform.position) <= path.endReachedDistance)
                Dash();
        }

        // Если враг "дэшиться"
        if (state == EnemyStates.DASH_INTO_PLAYER || state == EnemyStates.RETURN_TO_START_POINT) {

            rb2d.velocity = direction * path.maxSpeed * 100 * Time.fixedDeltaTime;
        }

        // Если возвращаемся в стартовую точку
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
        
        return Vector2.Reflect(dir, normal).normalized;

    }

    public void GetDamage(float value)
    {
        health -= value;

        if (health <= 0)
            control.DestroyEnemy(this);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {


        if (state == EnemyStates.DASH_INTO_PLAYER && collision.gameObject.tag == "Player") {
            collision.gameObject.GetComponent<PlayeLogic>().GetDamage(damage);
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
