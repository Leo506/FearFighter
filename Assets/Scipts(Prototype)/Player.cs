using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] PlayerInput _input;
    Rigidbody2D rb2D;
    BoxCollider2D bx;
    RoomGenerator generator;

//Для стрельбы
    public GameObject bulletPrefab;
    public CircleCollider2D fireArea;
    public Transform firePoint;
    private GameObject[] arrayAllEnemies;
    private GameObject[] arrayFreeEnemies;
    private bool roomIsGenerated;
    [SerializeField] GameObject _nearestEnemy;
    private float directionToEnemy;
    public float force;
    [SerializeField] float _timeDelay;
    private bool timeToShoot;
    private bool enemyIs;
    private RaycastHit2D checkFree;
//Для стрельбы

    // Start is called before the first frame update
    void Start()
    {
        //Для стрельбы
        roomIsGenerated = false;
        timeToShoot = true;
        enemyIs = false;
        //Для стрельбы

        rb2D = GetComponent<Rigidbody2D>();
        bx = GetComponent<BoxCollider2D>();
        generator = FindObjectOfType<RoomGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        var dir = _input.GetSwipe();
        if (AttemptMove(dir * generator.xOffset))
            rb2D.MovePosition(rb2D.position + dir * generator.xOffset);
        
        //Для стрельбы Определение ближайшего врага в зоне видимости
        if (roomIsGenerated)
        {
            foreach (GameObject enemy in arrayAllEnemies)
            {
                checkFree = Physics2D.Raycast(firePoint.transform.position, enemy.transform.position - transform.position, Vector2.Distance(transform.position, enemy.transform.position), LayerMask.GetMask("Walls"));
                if (fireArea.IsTouching(enemy.GetComponent<Collider2D>()) && checkFree.collider == null)
                {
                    enemyIs = true;
                    if (Vector3.Distance(transform.position, enemy.transform.position) < Vector3.Distance(transform.position, _nearestEnemy.transform.position))
                    {
                        _nearestEnemy = enemy;
                    }
                    else
                    {
                        _nearestEnemy = enemy;
                    }
                } else
                {
                    enemyIs = false;
                }
            }
        }
        if (enemyIs)
        {
            Shoot(FindAngle(gameObject, _nearestEnemy));
        }
        //Для стрельбы Определение ближайшего врага в зоне видимости
    }

    bool AttemptMove(Vector2 dir)
    {
        Vector2 newPos = rb2D.position + dir;
        bx.enabled = false;
        RaycastHit2D hit = Physics2D.Linecast(rb2D.position, newPos);
        bx.enabled = true;

        if (hit.transform != null)
            return false;

        return true;
    }

    //Для стрельбы Заполняем массив всеми врагами в комнате
    public void SetAllEnemiesInArray()
    {
        arrayAllEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        roomIsGenerated = true;
    }
    //Заполняем массив всеми врагами в комнате

    //Стрельба
    float FindAngle(GameObject go1, GameObject go2)
    {
        Vector3 difference = go1.transform.position - go2.transform.position;
        directionToEnemy = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        return directionToEnemy;
    }
    private void Shoot(float angle)
    {
        if (timeToShoot)
        {
            StartCoroutine(shootDelay(angle, _timeDelay));
            timeToShoot = false;
        }
    }

    IEnumerator shootDelay(float angle, float timeDelay)
    {
        yield return new WaitForSeconds(timeDelay);
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0f, 0f, angle-90));
        firePoint.transform.rotation = Quaternion.Euler(0f, 0f, angle - 270);
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * force, ForceMode2D.Impulse);
        timeToShoot = true;
    }
    //Для стрельбы Стрельба
}
