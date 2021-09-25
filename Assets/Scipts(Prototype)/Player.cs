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
        this.transform.position = _startPos;
    }
}
