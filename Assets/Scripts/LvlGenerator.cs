using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class LvlGenerator : MonoBehaviour
{
    [SerializeField] GameObject[] _obstacles;   	// Массив возможных препятствий в комнате
    [SerializeField] GameObject _outerWalls;    	// Префаб внешних стен
    [SerializeField] GameObject _playerSpawner; 	// Объект, где будет спавниться игрок
    [SerializeField] GameObject _playerPrefab;  	// Префаб игрока
    [SerializeField] GameObject[] _enemiesPrefabs;  // Прафабы врагов
	CinemachineVirtualCamera camera;


	void Awake() {
		DontDestroyOnLoad(this.gameObject);
	}


	void Start() {
		GenerateRoom();
	}


    public void GenerateRoom() {
    	if (_outerWalls != null)
    		Instantiate(_outerWalls, this.transform);

    	int countOfObstacles = Random.Range(6, 9);  // Определяем количество препятствий комнате

    	for (int i = 0; i < countOfObstacles; i++) {
    		Instantiate(_obstacles[Random.Range(0, _obstacles.Length)], this.transform);
    	}

    	Invoke("StopWallMovement", 5);
    }



    void StopWallMovement() {
		foreach (var item in FindObjectsOfType<Wall>())
    		item.FixWall();

    	SceneManager.LoadScene("MainScene");
    }


    public void SpawnPlayer() {
    	_playerSpawner.GetComponent<BoxCollider2D>().enabled = false;

    	var player = Instantiate(_playerPrefab);
    	player.transform.localPosition = _playerSpawner.transform.position;

    	camera = FindObjectOfType<CinemachineVirtualCamera>();

    	camera.Follow = player.transform;
    }


    public void SpawnEnemies() {
    	var countOfEnemies = Random.Range(5, 10);

    	for (int i = 0; i < countOfEnemies; i++) {
    		var index = Random.Range(0, _enemiesPrefabs.Length);
    		for (int j = 0; j < 10; j++) {
    			Vector2 newPos = new Vector2(Random.Range(-8, 8), Random.Range(-8, 8));

    			if (Physics2D.OverlapCircle(newPos, _enemiesPrefabs[index].GetComponent<CircleCollider2D>().radius * 2) == null) {
    				var obj = Instantiate(_enemiesPrefabs[index]);
    				obj.transform.position = newPos;
    				break;
    			}
    		}
    	}
    }
}
