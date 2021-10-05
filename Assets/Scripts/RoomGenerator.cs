using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Pathfinding;

struct Cell
{
    public int x;
    public int y;

    public Cell(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public override string ToString()
    {
        return $"X = {this.x} Y = {this.y}";
    }
}


public class RoomGenerator : MonoBehaviour
{
    [SerializeField] GameObject _floorPrefab, _wallPrefab, _exitPrefab, _enemyPrefab;
    [SerializeField] GameObject[] _items;
    [SerializeField] Player _player;
    public float xOffset;
    public float yOffset;

    GameObject currentRoom;

    List<Cell> cells = new List<Cell>();

    Cell startCell;

    void Start()
    {
        GenerateRoom(-1);
    }

    public void GenerateRoom(int indexOfRoom)
    {
        if (currentRoom != null)
            Destroy(currentRoom.gameObject);
        cells.Clear();

        currentRoom = new GameObject("Room");

        if (indexOfRoom == 2)
            GenerateBossArena();
        else
        {
            CreateEnter();
            GenerateMaze();
            CreateExit();
            GenerateWalls();
            CreateItems();

        }

        CreatePlayer();


    }

    void GenerateBossArena()
    {
        for (int i = 0; i <= 4; i++)
        {
            for (int j = 0; j <= 12; j++)
            {
                CreateCell(new Cell(i, j));
            }
        }

        CreateEnter();
        GenerateWalls();
        var enemy = Instantiate(_enemyPrefab);
        enemy.GetComponent<AIDestinationSetter>().target = _player.transform;

    }

    void GenerateMaze()
    {
        // Доступные x: 0...4
        // Доступные y: 0...12
        // TODO сделать настраиваемыми диапазоны x и y

        for (int x = startCell.x; x <= 4; x++)
        {
            int y = 0;
            Cell next = RandomDir(x, y);
            if (!next.Equals(new Cell(-1, -1)))
                CreateCell(new Cell(x, y), next);
        }

        for (int x = 0; x <= 4; x+=2)
        {
            for (int y = 2; y <= 12; y+=2)
            {
                Cell next = RandomDir(x, y);
                if (!next.Equals(new Cell(-1, -1)))
                    CreateCell(new Cell(x, y), next);
                else
                    CreateCell(new Cell(x, y));
            }
        }
    }

    void GenerateWalls()
    {
        for (int x = -1; x <= 5; x++)
        {
            for (int y = -1; y <= 13; y++)
            {
                Cell cell = new Cell(x, y);
                Debug.Log($"x: {cell.x} y = {cell.y} contains? {cells.Contains(cell)}");
                if (!cells.Contains(cell))
                {
                    var wall = Instantiate(_wallPrefab, currentRoom.transform);
                    wall.transform.position = new Vector2((x - 2) * xOffset, (y - 6) * yOffset);
                }
            }
        }
    }

    void CreateItems()
    {
        int countOfItems = Random.Range(1, 5);

        for (int i = 0; i < countOfItems; i++)
        {
            GameObject itemPrefab = _items[Random.Range(1, _items.Length)];
            Cell itemCell = cells[Random.Range(0, cells.Count)];
            var item = Instantiate(itemPrefab, currentRoom.transform);
            item.transform.position = new Vector2((itemCell.x - 2) * xOffset, (itemCell.y - 6) * yOffset);
        }
    }

    void CreatePlayer()
    {
        _player.gameObject.transform.position = new Vector2((startCell.x - 2) * xOffset, (startCell.y - 6) * yOffset);
        _player.SetAllEnemiesInArray();
    }

    void CreateEnter()
    {
        int x = Random.Range(0, 5);
        int y = 0;

        CreateCell(new Cell(x, y - 1));

        startCell = new Cell(x, 0);
        Debug.Log($"Start cell: x={startCell.x} y={startCell.y}");
    }

    void CreateExit()
    {
        int countOfExit = Random.Range(1, 4);
        List<Cell> cellToCreate = new List<Cell>();
        var aheadExits = from c in cells where c.y == 12 select c;
        var rightExists = from c in cells where c.x == 0 select c;
        var leftExits = from c in cells where c.x == 4 select c;

        Cell cellToAdd = new Cell(-1, -1);
        switch (countOfExit)
        {
            case 1:
                cellToAdd = aheadExits.ElementAt(Random.Range(0, aheadExits.Count()));
                cellToAdd.y += 1;
                cellToCreate.Add(cellToAdd);
                break;

            case 2:
                cellToAdd = rightExists.ElementAt(Random.Range(0, rightExists.Count()));
                cellToAdd.x -= 1;
                cellToCreate.Add(cellToAdd);

                cellToAdd = leftExits.ElementAt(Random.Range(0, leftExits.Count()));
                cellToAdd.x += 1;
                cellToCreate.Add(cellToAdd);

                break;

            case 3:
                cellToAdd = aheadExits.ElementAt(Random.Range(0, aheadExits.Count()));
                cellToAdd.y += 1;
                cellToCreate.Add(cellToAdd);

                cellToAdd = rightExists.ElementAt(Random.Range(0, rightExists.Count()));
                cellToAdd.x -= 1;
                cellToCreate.Add(cellToAdd);

                cellToAdd = leftExits.ElementAt(Random.Range(0, leftExits.Count()));
                cellToAdd.x += 1;
                cellToCreate.Add(cellToAdd);
                break;

            default:
                break;
        }

        foreach (var item in cellToCreate)
        {
            CreateCell(_exitPrefab, item);
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

        for (int i = 0; i < dirs.Length; i++)
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
        Vector2[] dirs = { Vector2.up * 2, Vector2.right * 2};

        for (int i = 0; i < 2; i++)
        {
            int j = Random.Range(0, 2);
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
            
            return true;
                
        }



        return false;
    }


    void CreateCell(Cell current, Cell next)
    {

        if (current.x != next.x)
        {
            if (current.x < next.x)
            {
                for (int i = current.x; i < next.x; i++)
                {
                    var obj = Instantiate(_floorPrefab, currentRoom.transform);
                    obj.transform.position = new Vector2((i - 2) * xOffset, (current.y - 6) * yOffset);
                    cells.Add(new Cell(i, current.y));
                }
            } else
            {
                for (int i = current.x; i > next.x; i--)
                {
                    var obj = Instantiate(_floorPrefab, currentRoom.transform);
                    obj.transform.position = new Vector2((i - 2) * xOffset, (current.y - 6) * yOffset);
                    cells.Add(new Cell(i, current.y));
                }
            }
        } else
        {
            if (current.y < next.y)
            {
                for (int i = current.y; i < next.y; i++)
                {
                    var obj = Instantiate(_floorPrefab, currentRoom.transform);
                    obj.transform.position = new Vector2((current.x - 2) * xOffset, (i - 6) * yOffset);
                    cells.Add(new Cell(current.x, i));
                }
            } else
            {
                for (int i = current.y; i > next.y; i--)
                {
                    var obj = Instantiate(_floorPrefab, currentRoom.transform);
                    obj.transform.position = new Vector2((current.x - 2) * xOffset, (i - 6) * yOffset);
                    cells.Add(new Cell(current.x, i));
                }
            }
        }
        
    }

    void CreateCell(Cell cell)
    {
        var obj = Instantiate(_floorPrefab, currentRoom.transform);
        obj.transform.position = new Vector2((cell.x - 2) * xOffset, (cell.y - 6) * yOffset);
        cells.Add(cell);
    }

    void CreateCell(GameObject prefab, Cell cell)
    {
        var obj = Instantiate(prefab, currentRoom.transform);
        obj.transform.position = new Vector2((cell.x - 2) * xOffset, (cell.y - 6) * yOffset);
        cells.Add(cell);
    }
}
