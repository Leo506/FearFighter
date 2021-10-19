using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LvlGenerator : MonoBehaviour
{
    [SerializeField] GameObject[] _obstacles;   // Массив возможных препятствий в комнате
    [SerializeField] GameObject _outerWalls;    // Префаб внешних стен
    [SerializeField] GameObject _playerSpawner; // Объект, где будет спавниться игрок
    [SerializeField] GameObject _playerPrefab;  // Префаб игрока
    [SerializeField] CinemachineVirtualCamera camera;

    public void GenerateRoom() {
    	if (_outerWalls != null)
    		Instantiate(_outerWalls);

    	int countOfObstacles = Random.Range(6, 9);  // Определяем количество препятствий комнате

    	for (int i = 0; i < countOfObstacles; i++) {
    		Instantiate(_obstacles[Random.Range(0, _obstacles.Length)]);
    	}

    	Invoke("StopWallMovement", 5);

    	Invoke("SpawnPlayer", 5);
    }



    void StopWallMovement() {
		foreach (var item in FindObjectsOfType<Wall>())
    		item.FixWall();
    }


    void SpawnPlayer() {
    	_playerSpawner.GetComponent<BoxCollider2D>().enabled = false;

    	var player = Instantiate(_playerPrefab);
    	player.transform.localPosition = _playerSpawner.transform.position;

    	camera.Follow = player.transform;
    }
}
