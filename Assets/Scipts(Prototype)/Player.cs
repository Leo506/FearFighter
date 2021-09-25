using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] LvlMovement _movement;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _movement.canMove = false;
    }
}
