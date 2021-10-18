using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	LvlGenerator generator;

    // Start is called before the first frame update
    void Start()
    {
        generator = FindObjectOfType<LvlGenerator>();
        generator.GenerateRoom();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
