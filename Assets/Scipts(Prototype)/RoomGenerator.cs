using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    [SerializeField] GameObject[] _floorPrefabs;
    [SerializeField] GameObject[] _wallPrefabs;
    [SerializeField] GameObject[] _obstacles;
    [SerializeField] GameObject _wallWithTurns;
    [SerializeField] GameObject _trigger;
    [SerializeField] float _roomOffset = 10.15f;
    public float timeToChange;
    GameObject currentRoom, nextRoom, lastRoom;

    private void Start()
    {
        currentRoom = CreateRoom();
        /*nextRoom = CreateRoom();
        lastRoom = CreateRoom();*/
        
        currentRoom.transform.position = Vector3.zero;

        /*nextRoom.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90.0f));
        nextRoom.transform.position = new Vector3(7.64f, 0, 0);

        lastRoom.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90.0f));
        lastRoom.transform.position = new Vector3(-7.64f, 0, 0);

        StartCoroutine(RotateRooms());*/

    }

    IEnumerator ChangeRooms(bool hasObstacle = true)
    {
        var deltaY = (nextRoom.transform.position - currentRoom.transform.position).magnitude / 100;
        for (int i = 0; i < 100; i++)
        {
            nextRoom.transform.Translate(0, -deltaY, 0);
            currentRoom.transform.Translate(0, -deltaY, 0);
            yield return new WaitForSeconds(timeToChange / 100);
        }

        Destroy(currentRoom.gameObject);
        currentRoom = nextRoom;

        if (hasObstacle)
            GenerateObstacles();
    }


    public void GenerateRoom()
    {
        bool hasTurn = Random.Range(0, 2) == 1; // Будет ли в комнате развилка?
        if (hasTurn)
            nextRoom = CreateRoom(true);
        else
            nextRoom = CreateRoom();

        nextRoom.transform.position = new Vector3(0, _roomOffset, 0);
        StartCoroutine(ChangeRooms(!hasTurn));
    }

    GameObject CreateRoom(bool hasTurn = false)
    {
        GameObject room = new GameObject("Room");

        GameObject floor = _floorPrefabs[Random.Range(0, _floorPrefabs.Length)];

        GameObject wall;
        if (hasTurn)
            wall = _wallWithTurns;  // Есть развилка
        else
            wall = _wallPrefabs[Random.Range(0, _wallPrefabs.Length)];  // Нет развилки

        Instantiate(floor, room.transform);
        Instantiate(wall, room.transform);

        if (hasTurn)
            Instantiate(_trigger, room.transform);  // Триггер для развилки

        return room;
    }

    void GenerateObstacles()
    {
        Instantiate(_obstacles[Random.Range(0, _obstacles.Length)]);
    }

    public void ChooseRoom()
    {
        nextRoom = CreateRoom();
        lastRoom = CreateRoom();

        nextRoom.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90.0f));
        nextRoom.transform.position = new Vector3(7.64f, 0, 0);

        lastRoom.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90.0f));
        lastRoom.transform.position = new Vector3(-7.64f, 0, 0);

        StartCoroutine(RotateRooms());
    }


    IEnumerator RotateRooms()
    {
        var deltaZ = 0.9f;
        for (int i = 0; i <= 100; i++)
        {
            currentRoom.transform.rotation = Quaternion.Euler(new Vector3(0, 0, i * deltaZ));
            nextRoom.transform.RotateAround(currentRoom.transform.position, Vector3.forward, deltaZ);
            lastRoom.transform.RotateAround(currentRoom.transform.position, Vector3.forward, deltaZ);
            yield return new WaitForSeconds(timeToChange / 100);
        }

        var deltaY = (nextRoom.transform.position - currentRoom.transform.position).magnitude / 100;
        for (int i = 0; i < 100; i++)
        {
            nextRoom.transform.Translate(0, -deltaY, 0);
            currentRoom.transform.Translate(-deltaY, 0, 0);
            lastRoom.transform.Translate(0, -deltaY, 0);
            yield return new WaitForSeconds(timeToChange / 100);
        }
        nextRoom.transform.position = Vector3.zero;

        Destroy(currentRoom.gameObject);
        Destroy(lastRoom.gameObject);
        currentRoom = nextRoom;

        FindObjectOfType<Player>().canMove = true;
    }
}
