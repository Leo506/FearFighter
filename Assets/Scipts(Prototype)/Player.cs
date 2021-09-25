using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] RoomGenerator _creator;
    [SerializeField] Vector3 _startPos;

    bool canMove = true;

    private void Update()
    {
        if (canMove)
            this.transform.Translate(0, _speed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _creator.GenerateRoom();
        StartCoroutine(ResetPos());
    }

    IEnumerator ResetPos()
    {
        var deltaY = (this.transform.position - _startPos).magnitude / 100;
        for (int i = 0; i < 100; i++)
        {
            this.transform.Translate(0, -deltaY, 0);
            yield return new WaitForSeconds(_creator.timeToChange / 100);
        }
    }
}
