using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] RoomGenerator _creator;
    [SerializeField] Vector3 _startPos;
    [SerializeField] Canvas _chooseCanvas;
    GameObject obstacle;

    public bool canMove = true;

    private void Update()
    {
        if (canMove)
            this.transform.Translate(0, _speed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Turn")
        {
            canMove = false;
            _creator.ChooseRoom();
            Destroy(collision.gameObject);
        }
        else
        {
            _creator.GenerateRoom();
            StartCoroutine(ResetPos());
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Turn")
        {
            canMove = false;
            _creator.ChooseRoom();
        }
        else
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
}
