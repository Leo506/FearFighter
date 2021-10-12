using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] PlayerInput _input;
    public float speed;
    public static bool canMove = false;

    // Update is called once per frame
    void Update()
    {
        if (_input.GetDir() != Vector3.zero && canMove) {
        	this.transform.Translate(_input.GetDir() * speed * Time.deltaTime);
        }
    }
}
