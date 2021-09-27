using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    bool isMobile;
    Vector2 startPos;
    Vector2 swipeDelta;

    public float deadZone = 80;

    private void Start()
    {
        isMobile = Application.isMobilePlatform;
    }

    // Update is called once per frame
    void Update()
    {


    }

    public Vector2 GetSwipe()
    {
        Vector2 swipe = Vector2.zero;

        if (isMobile)
        {
            // TODO добавить определение свайпа для сенсорных экранов
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                startPos = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                swipe = CheckSwip();
            }
        }
        Debug.Log(swipe);
        return swipe;
    }


    Vector2 CheckSwip()
    {
        swipeDelta = Vector2.zero;
        Vector2 swipe = Vector2.zero;

        if (isMobile)
        {

        }
        else
        {
            swipeDelta = (Vector2)Input.mousePosition - startPos;

            if (swipeDelta.magnitude >= deadZone)
            {
                if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
                {
                    if (swipeDelta.x > 0)
                        swipe = Vector2.right;
                    else
                        swipe = Vector2.left;
                }
                else
                {
                    if (swipeDelta.y > 0)
                        swipe = Vector2.up;
                    else
                        swipe = Vector2.down;
                }
            }
        }

        ResetSwip();
        return swipe;
    }

    void ResetSwip()
    {

        startPos = Vector2.zero;
        swipeDelta = Vector2.zero;
    }

}
