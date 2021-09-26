using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] RoomGenerator _creator;
    [SerializeField] Vector3 _startPos;
    [SerializeField] Canvas _chooseCanvas;
    [SerializeField] Joystick _joystick;
    [SerializeField] Transform firePoint;
    [SerializeField] float impulseBullet;
    public GameObject bulletPrefab;
    public GameObject enemy;
    private Rigidbody2D rb;
    GameObject obstacle;

    bool recharge = false;
    bool canMove = true;
   
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        impulseBullet = 20f;
    }

    private void Update()
    {
        rb.velocity = new Vector3(_speed * _joystick.Horizontal, _speed * _joystick.Vertical, 0);

        // transform.LookAt(enemy.transform.position);
        var direction = enemy.transform.position - this.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        rb.rotation = angle;

        if(!recharge)
        {
            Shoot(angle, enemy.transform.position);
        }
       // if (canMove)
       //     this.transform.Translate(0, _speed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _creator.GenerateRoom();
        StartCoroutine(ResetPos());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            Time.timeScale = 0;
            obstacle = collision.gameObject;
            _chooseCanvas.enabled = true;
        }
    }

    public void ContinueGame()
    {
        Destroy(obstacle.gameObject);
        Time.timeScale = 1;
        _chooseCanvas.enabled = false;
    }

    public void Shoot(float angle, Vector3 pos)
    {
        GameObject bullet = Instantiate(bulletPrefab, new Vector3(firePoint.transform.position.x, firePoint.transform.position.y, 0), firePoint.rotation);
        bullet.transform.rotation.Set(0,0,angle,0);
        Rigidbody2D rbBul = bullet.GetComponent<Rigidbody2D>();
        rbBul.AddForce(firePoint.up * impulseBullet, ForceMode2D.Impulse);

        StartCoroutine(TimerRecharge(0.5f));
    }

    IEnumerator ResetPos()
    {
        canMove = false;
        var deltaY = (this.transform.position - _startPos).magnitude / 100;
        for (int i = 0; i < 100; i++)
        {
            this.transform.Translate(0, -deltaY, 0);
            yield return new WaitForSeconds(_creator.timeToChange / 100);
        }
        canMove = true;
    }
    IEnumerator TimerRecharge(float secRecharge)
    {
        recharge = true;

        yield return new WaitForSeconds(secRecharge);
        recharge = false;
    }
}
