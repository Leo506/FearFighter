using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] PlayerInput _input;
    [SerializeField] Canvas _chooseCanvas, _endGameCanvas;
    [SerializeField] UnityEngine.UI.Slider _braveSlider;
    GameObject item;
    Rigidbody2D rb2D;
    BoxCollider2D bx;
    RoomGenerator generator;
    public float brave = 100;

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

        _braveSlider.maxValue = brave;
        _braveSlider.value = _braveSlider.maxValue;

        Time.timeScale = 1;
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
            _nearestEnemy = CloseToEnemy(arrayAllEnemies);

            if (_nearestEnemy != null)
            {
                checkFree = Physics2D.Raycast(firePoint.transform.position, _nearestEnemy.transform.position - transform.position, Vector2.Distance(transform.position, _nearestEnemy.transform.position), LayerMask.GetMask("Walls"));
                enemyIs = fireArea.IsTouching(_nearestEnemy.GetComponent<Collider2D>()) && checkFree.collider == null;
            } else
            {
                enemyIs = false;
                SetAllEnemiesInArray();
            }

            
        }

        if (enemyIs)
        {
            Shoot(FindAngle(gameObject, _nearestEnemy));
        }

        if (countOfRoom == 3 && generator.boss == null)
        {
            Time.timeScale = 0;
            _endGameCanvas.enabled = true;
        }
    }

    GameObject CloseToEnemy(GameObject[] enemies)
    {
        try
        {
            GameObject near = enemies[0];
            float dist = Vector2.Distance(enemies[0].transform.position, transform.position);

            for (int i = 1; i < enemies.Length; i++)
            {
                var tmp = Vector2.Distance(enemies[i].transform.position, transform.position);
                if (tmp < dist)
                {
                    near = enemies[i];
                    dist = tmp;
                }
            }
            return near;
        } catch
        {
            return null;
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
                countOfRoom++;
                generator.GenerateRoom(countOfRoom);
                
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

    public void GetDamage(float value)
    {
        brave -= value;
        _braveSlider.value = brave;
        
        if (brave <= 0)
        {
            Time.timeScale = 0;
            _endGameCanvas.enabled = true;
        }
    }

    public void Replay()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Prototype");
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
        if (go1 != null && go2 != null)
        {
            Vector3 difference = go1.transform.position - go2.transform.position;
            directionToEnemy = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            return directionToEnemy;
        }
        return 0;
        
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
