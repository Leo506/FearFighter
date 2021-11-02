using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppingItem : MonoBehaviour
{
    public static int countOfDrop = 0;                                       // Количество дропа
    public float speed = 1;                                                  // Скорость движения к персонажу

    bool moveToPlayer = false;                                               // Можем ли начать двигаться к персонажу
    Vector2 dir;                                                             // Направление движения
    Vector2 destPos;                                                         // Точка назначения (положение персонажа)


    /// <summary>
    /// Начинает движение предмета к персонажу
    /// </summary>
    /// <param name="playerPos">Положение персонажа</param>
    public void StartMoveToPlayer(Vector2 playerPos)
    {
        moveToPlayer = true;
        GetComponent<Collider2D>().enabled = false;
        dir = (playerPos - (Vector2)transform.position).normalized;
        destPos = playerPos;
    }

    private void Update()
    {
        if (moveToPlayer)
        {
            if (Vector2.Distance(destPos, transform.position) >= 0.1f)       // Пока игрок недостаточно близко
                transform.Translate(dir * speed * Time.deltaTime);           // Двигаемся к игроку
            else                                                             // Иначе
            {
                countOfDrop--;

                if (countOfDrop <= 0)
                    FindObjectOfType<GameController>().ChangeState(GameState.GO_TO_EXIT);
                
                Destroy(this.gameObject);                                    // Уничтожаем объект
            }
            
        }
    }

    public void Init()
    {
        countOfDrop++;
    }
}
