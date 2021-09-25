using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvlMovement : MonoBehaviour
{
    [SerializeField] float _speed;

    public bool canMove = true;

    private void Update()
    {
        if (canMove)
        {
            this.transform.Translate(0, -_speed * Time.deltaTime, 0);
        }
    }
}
