using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvlGenerator : MonoBehaviour
{
    [SerializeField] GameObject[] _obstacles;  // Массив возможных препятствий в комнате
    [SerializeField] GameObject _outerWalls;   // Префаб внешних стен

    public void GenerateRoom() {
    	if (_outerWalls != null)
    		Instantiate(_outerWalls);

    	int countOfObstacles = Random.Range(6, 9);  // Определяем количество препятствий комнате

    	for (int i = 0; i < countOfObstacles; i++) {
    		Instantiate(_obstacles[Random.Range(0, _obstacles.Length)]);
    	}

    	Invoke("StopWallMovement", 5);
    }



    void StopWallMovement() {
		foreach (var item in FindObjectsOfType<Wall>())
    		item.FixWall();
    }
}
