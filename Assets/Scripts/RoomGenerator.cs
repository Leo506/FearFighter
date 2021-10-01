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

    Cell startCell;

    public void GenerateRoom()
    {
        if (currentRoom != null)
            Destroy(currentRoom.gameObject);
        cells.Clear();

        currentRoom = new GameObject("Room");
        CreateEnter();
        GenerateMaze();
        CreateExit();
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
        
    }

    void GenerateMaze()
    {
        // Доступные x: 0...4
        // Доступные y: 0...12
        // TODO сделать настраиваемыми диапазоны x и y

        for (int x = startCell.x; x <= 4; x++)
        {
            int y = startCell.y;
            CreateCell(RandomDir(x, y));
        }

        for (int x = 0; x < startCell.x; x++)
        {
            int y = startCell.y;
            CreateCell(RandomDir(x, y));
        }

        for (int x = 0; x < 4; x++)
        {
            for (int y = 1; y < 12; y++)
            {
                CreateCell(RandomDir(x, y));
            }
        }
    }

    void CreateEnter()
    {
        int x = Random.Range(0, 5);
        int y = -1;
        Cell cell = new Cell(x, y);
        CreateCell(cell);

        startCell = new Cell(cell.x, cell.y + 1);
        CreateCell(startCell);
    }

    void CreateExit()
    {
        var lastCell = cells[cells.Count - 1];

        for (int i = 1; i <= 13 - lastCell.y; i++)
        {
            CreateCell(new Cell(lastCell.x, lastCell.y + i));
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
        Vector2[] dirs = CreateRandomDir();

        for (int i = 0; i < 4; i++)
        {
            if (ValidDir(xPos, yPos, dirs[i]))
                return new Cell(xPos + (int)dirs[i].x, yPos + (int)dirs[i].y);
        }

        return new Cell(-1, -1);

    }


    /// <summary>
    /// Создание случайного направления (в Vector2)
    /// </summary>
    /// <returns>Возвращает направление в виде Vector2</returns>
    Vector2[] CreateRandomDir()
    {
        Vector2[] dirs = { Vector2.up, Vector2.down, Vector2.right, Vector2.left };

        for (int i = 0; i < 4; i++)
        {
            int j = Random.Range(0, 4);
            var tmp = dirs[j];
            dirs[j] = dirs[i];
            dirs[i] = tmp;
        }

        return dirs;
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
        if (((newXPos >= 0) && (newXPos <= 4)) && ((newYPos >= 0) && (newYPos <= 12)))
        {
            if (!cells.Contains(new Cell(newXPos, newYPos)))
            {
                return true;
            }
                
        }



        return false;
    }


    void CreateCell(Cell cell)
    {
        if (!cell.Equals(new Cell(-1, -1)))
        {
            var obj = Instantiate(_floorPrefab, currentRoom.transform);
            obj.transform.position = new Vector2((cell.x-2) * xOffset, (cell.y-6) * yOffset);
            cells.Add(cell);
        }
        
    }
}
