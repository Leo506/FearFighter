using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct Cell
{
    public int x;
    public int y;

    public Cell(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}


public class RoomGenerator : MonoBehaviour
{
    [SerializeField] GameObject _floorPrefab, _wallPrefab;
    public float xOffset;
    public float yOffset;

    GameObject currentRoom;

    List<Cell> cells = new List<Cell>();

    //Для стрельбы
    public Player player;
    //Для стрельбы

    public void GenerateRoom()
    {
        if (currentRoom != null)
            Destroy(currentRoom.gameObject);
        cells.Clear();

        currentRoom = new GameObject("Room");
        GenerateMaze();
        for (int x = -1; x <= 5; x++)
        {
            for (int y = -1; y <= 13; y++)
            {
                Cell cell = new Cell(x, y);
                if (!cells.Contains(cell))
                {
                    var wall = Instantiate(_wallPrefab, currentRoom.transform);
                    wall.transform.position = new Vector2((x - 2) * xOffset, (y - 6) * yOffset);
                }
            }
        }
        //Для стрельбы
        player.SetAllEnemiesInArray();
        //Для стрельбы
    }

void GenerateMaze()
    {
        // Доступные x: 0...4
        // Доступные y: 0...12
        // TODO сделать настраиваемыми диапазоны x и y

        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 12; y++)
            {
                Cell cell = RandomDir(x, y);
                CreateCell(cell);
                cells.Add(cell);
            }
        }
    }


    /// <summary>
    /// Выбирает случайное направление для лабиринта
    /// </summary>
    /// <returns>Возвращает клетку в случайном направлении</returns>
    /// <param name="xPos">Позиция x текущей клетки (условный)</param>
    /// <param name="yPos">Позиция y текущей клетки (условный)</param>
    Cell RandomDir(int xPos, int yPos)
    {
        Vector2 dir = CreateRandomDir();
        int counter = 0;

        while (!ValidDir(xPos, yPos, dir) && counter < 10)
        {
            dir = CreateRandomDir();
            counter++;
        }




        return new Cell(xPos + (int)dir.x, yPos + (int)dir.y);

    }


    /// <summary>
    /// Создание случайного направления (в Vector2)
    /// </summary>
    /// <returns>Возвращает направление в виде Vector2</returns>
    Vector2 CreateRandomDir()
    {
        Vector2 dir;

        var rnd = Random.Range(0, 4);

        switch (rnd)
        {
            case 0:
                dir = Vector2.right;
                break;

            case 1:
                dir = Vector2.left;
                break;

            case 2:
                dir = Vector2.down;
                break;

            case 3:
                dir = Vector2.up;
                break;

            default:
                dir = Vector2.zero;
                break;
        }

        return dir;
    }


    /// <summary>
    /// Проверяем клетку в определённом направлении на доступность
    /// </summary>
    /// <returns><c>true</c>, if dir was valided, <c>false</c> otherwise.</returns>
    /// <param name="xPos">Условная позиция текущей клетки по X</param>
    /// <param name="yPos">Условная позиция текущей клетки по Y</param>
    /// <param name="dir">Направление к соседней клетке</param>
    bool ValidDir(int xPos, int yPos, Vector2 dir)
    {
        int newXPos = xPos + (int)dir.x;
        int newYPos = yPos + (int)dir.y;
        if ((newXPos >= 0) && (newXPos <= 4) && (newYPos >= 0) && (newYPos <= 12))
        {
            if (!cells.Contains(new Cell(newXPos, newYPos)))
                return true;
        }



        return false;
    }


    void CreateCell(Cell cell)
    {
        Debug.Log("Cell x: " + cell.x + " Cell y: " + cell.y);
        var obj = Instantiate(_floorPrefab, currentRoom.transform);
        obj.transform.position = new Vector2((cell.x-2) * xOffset, (cell.y-6) * yOffset);
        Debug.Log(new Vector2((cell.x - 2) * xOffset, (cell.y - 6) * yOffset));
    }
}
