using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float _speed;
    public GameObject player;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.transform.position = Vector2.MoveTowards(rb.transform.position, player.transform.position, _speed * Time.deltaTime);
    }
}
