using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
using Pathfinding;

public class LvlGenerator : MonoBehaviour
{
    [SerializeField] GameObject[] _obstacles;   	    // Массив возможных препятствий в комнате
    [SerializeField] GameObject _outerWalls;    	    // Префаб внешних стен
    [SerializeField] GameObject _playerSpawner; 	    // Объект, где будет спавниться игрок
    [SerializeField] GameObject _playerPrefab;  	    // Префаб игрока
    [SerializeField] GameObject[] _enemiesPrefabs;      // Прафабы врагов
	[SerializeField] CinemachineVirtualCamera camera;
    [SerializeField] GameObject _exit;                  // Префаб выхода
    [SerializeField] EnemyControl _control;

    GameObject playerObj;

	[Header("Настройка количества препятствий")]
	public int maxObstaclesCount = 15;
	public int minObstaclesCount = 9;

	[Header("Диапазон возможных координат для спавна")]
	public float minX = -8.0f;
	public float maxX = 8.0f;
	public float minY = -8.0f;
	public float maxY = 8.0f;

	[Header("Диапазон количества врагов на уровне")]
	public int minEnemiesCount = 3;
	public int maxEnemiesCount = 7;


    [Header("Дистанция до выхода")]
    public float distanceToExit = 5f;


	void Awake() {
		GenerateRoom();
        AstarPath.active.Scan();
	}


    public void GenerateRoom() {
    	if (_outerWalls != null)
    		Instantiate(_outerWalls, this.transform);

    	int countOfObstacles = Random.Range(minObstaclesCount, maxObstaclesCount);  // Определяем количество препятствий комнате


    	// Создаём нужное количество препятствий
    	for (int i = 0; i < countOfObstacles; i++) {
    		var index = Random.Range(0, _obstacles.Length);
    		var objToSpawn = _obstacles[index];

    		// Делаем 10 попыток создать препятствие
    		for (int j = 0; j < 10; j++) {
    			Vector2 newPos = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));

    			
    			// Проверка доступности места
    			if (CheckPos(newPos, objToSpawn)) {
    				var obj = Instantiate(objToSpawn);
    				obj.transform.position = newPos;
    				break;
    			}
    		}
    	}
    }

    
    bool CheckPos(Vector2 pos, GameObject obj) {
    	Vector2 size = obj.GetComponent<BoxCollider2D>().size;

    	float xTopRight = pos.x + (size.x * obj.transform.localScale.x) / 2;
    	float yTopRight = pos.y + (size.y * obj.transform.localScale.y) / 2;

    	float xBottomLeft = pos.x - (size.x * obj.transform.localScale.x) / 2;
    	float yBottomLeft = pos.y - (size.y * obj.transform.localScale.y) / 2;

    	return Physics2D.OverlapArea(new Vector2(xTopRight, yTopRight), new Vector2(xBottomLeft, yBottomLeft)) == null;

    }


    public void SpawnPlayer() {
    	_playerSpawner.GetComponent<BoxCollider2D>().enabled = false;

    	playerObj = Instantiate(_playerPrefab);
    	playerObj.transform.localPosition = _playerSpawner.transform.position;

    	camera.Follow = playerObj.transform;
    }


    public void SpawnEnemies() {
    	var countOfEnemies = Random.Range(minEnemiesCount, maxEnemiesCount);

    	for (int i = 0; i < countOfEnemies; i++) {
    		var index = Random.Range(0, _enemiesPrefabs.Length);
    		for (int j = 0; j < 10; j++) {
    			Vector2 newPos = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));

    			if (Physics2D.OverlapCircle(newPos, _enemiesPrefabs[index].GetComponent<CircleCollider2D>().radius * 2) == null) {
    				var obj = Instantiate(_enemiesPrefabs[index]);
    				obj.transform.position = newPos;
                    obj.GetComponent<EnemyController>().SetTarget(GameObject.FindWithTag("Player").transform);
                    _control.countOfEnemy++;
    				break;
    			}
    		}
    	}
    }


    public void SpawnExit()
    {
        Vector2[] availableDir = { Vector2.up, Vector2.right, Vector2.down, Vector2.left };
        Vector2 pos = (Vector2)playerObj.transform.position + availableDir[Random.Range(0, availableDir.Length)] * distanceToExit;

        Vector2 size = _exit.GetComponent<BoxCollider2D>().size;

        float xTopRight = pos.x + (size.x * _exit.transform.localScale.x) / 2;
        float yTopRight = pos.y + (size.y * _exit.transform.localScale.y) / 2;

        float xBottomLeft = pos.x - (size.x * _exit.transform.localScale.x) / 2;
        float yBottomLeft = pos.y - (size.y * _exit.transform.localScale.y) / 2;

        Collider2D[] colliders = Physics2D.OverlapAreaAll(new Vector2(xTopRight, yTopRight), new Vector2(xBottomLeft, yBottomLeft));

        foreach (var item in colliders)
        {
            Destroy(item.gameObject);
        }

        Instantiate(_exit).transform.position = pos;
    }
}
