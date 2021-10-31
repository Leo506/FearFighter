using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    public Transform target; // объект за которым надо следить

    public SpriteRenderer sprite;
    Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        Vector3 targetOnScreen = cam.WorldToViewportPoint(target.position);

        if (targetOnScreen.x >= 0 && targetOnScreen.x <= 1 && targetOnScreen.y >= 0 && targetOnScreen.y <= 1)
            sprite.enabled = false;
        else
        {
            targetOnScreen.x = Mathf.Clamp01(targetOnScreen.x);
            targetOnScreen.y = Mathf.Clamp01(targetOnScreen.y);

            sprite.enabled = true;

            Vector2 dir = target.position - transform.position;
            transform.position = cam.ViewportToWorldPoint(targetOnScreen);
            var zRot = Mathf.Acos(Vector2.Dot(Vector2.up, dir) / dir.magnitude) * Mathf.Rad2Deg;

            if (Vector3.Cross(Vector2.up, dir).z < 0)
                zRot *= -1;
            
            transform.rotation = Quaternion.Euler(0, 0, zRot);
        }
    }
}
