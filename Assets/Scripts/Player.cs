using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] PlayerInput _input;
    [SerializeField] Canvas _chooseCanvas;
    GameObject item;
    Rigidbody2D rb2D;
    BoxCollider2D bx;
    RoomGenerator generator;

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

    public int countOfRoom;

    // Start is called before the first frame update
    void Start()
    {
        roomIsGenerated = false;
        timeToShoot = true;
        enemyIs = false;

        rb2D = GetComponent<Rigidbody2D>();
        bx = GetComponent<BoxCollider2D>();
        generator = FindObjectOfType<RoomGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        var dir = _input.GetSwipe() * generator.xOffset;
        if (dir != Vector2.zero)
        {
            if (AttemptMove(dir))
                rb2D.MovePosition(rb2D.position + dir);
        }

        if (roomIsGenerated)
        {
            Dictionary<float, GameObject> distances = new Dictionary<float, GameObject>();
            foreach (var item in arrayAllEnemies)
            {
                distances.Add((item.transform.position - this.transform.position).magnitude, item);
            }

            List<float> keys = new List<float>();
            foreach (var item in distances.Keys)
            {
                keys.Add(item);
            }
            keys.Sort();


            foreach (GameObject enemy in arrayAllEnemies)
            {
                checkFree = Physics2D.Raycast(firePoint.transform.position, enemy.transform.position - transform.position, Vector2.Distance(transform.position, enemy.transform.position), LayerMask.GetMask("Walls"));
                if (fireArea.IsTouching(enemy.GetComponent<Collider2D>()) && checkFree.collider == null)
                {
                    enemyIs = true;
                    /*if (Vector3.Distance(transform.position, enemy.transform.position) < Vector3.Distance(transform.position, _nearestEnemy.transform.position))
                    {
                        _nearestEnemy = enemy;
                    }
                    else
                    {
                        _nearestEnemy = enemy;
                    }*/

                    _nearestEnemy = distances[keys[0]];
                }
                else
                {
                    enemyIs = false;
                }
            }
        }
        if (enemyIs)
        {
            Shoot(FindAngle(gameObject, _nearestEnemy));
        }
    }


    bool AttemptMove(Vector2 dir)
    {
        Vector2 newPos = rb2D.position + dir;
        bx.enabled = false;
        RaycastHit2D hit = Physics2D.Linecast(rb2D.position, newPos, LayerMask.GetMask("Walls", "Enemies", "Items"));
        bx.enabled = true;

        if (hit.transform != null)
        {
            if (hit.collider.gameObject.tag == "Item")
            {
                Time.timeScale = 0;
                _chooseCanvas.enabled = true;
                item = hit.collider.gameObject;
            }
            else if (hit.collider.gameObject.tag == "Exit")
            {
                generator.GenerateRoom(countOfRoom);
                countOfRoom++;
            }
            return false;
        }

        return true;
    }

    public void ContinueGame()
    {
        Time.timeScale = 1;
        _chooseCanvas.enabled = false;
        Destroy(item);
    }

    //Для стрельбы Заполняем массив всеми врагами в комнате
    public void SetAllEnemiesInArray()
    {
        arrayAllEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        roomIsGenerated = true;
        Debug.Log("qwerty");
    }

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
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0f, 0f, angle - 90));
        firePoint.transform.rotation = Quaternion.Euler(0f, 0f, angle - 270);
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * force, ForceMode2D.Impulse);
        timeToShoot = true;
    }
}
