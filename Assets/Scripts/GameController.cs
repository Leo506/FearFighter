using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    OVERVIEW,
    SPAWNING,
    PLAY,
    WAITING_DROP,
    GO_TO_EXIT
}

public class GameController : MonoBehaviour
{
    GameState currentState = GameState.OVERVIEW;
    LvlGenerator generator;
    PlayeLogic player;
    LvlOverview overview;


    public void ChangeState(GameState state)
    {
        currentState = state;

        // При переходе в состояние спавнинга
        if (state == GameState.SPAWNING)
        {
            player = generator.SpawnPlayer().GetComponent<PlayeLogic>();  // Спавн персонажа
            generator.SpawnEnemies();                                     // Спавн врагов
            generator.SpawnBonusItems();                                  // Спавн бонусных предметов
            currentState = GameState.PLAY;                                // Переход в состояние игры
        }


        // При переходе в состояние ожидания получения дропа
        if (state == GameState.WAITING_DROP)
        {
            player.EndRound();                                       // Запрещаем персонажу двигаться и выбирать направление
            foreach (var item in FindObjectsOfType<DroppingItem>())  
            {
                item.StartMoveToPlayer(player.transform.position);   // Начинаем двигать весь дроп к персонажу
            }
            
        }


        // При переходе в состояние похода к выходу
        if (state == GameState.GO_TO_EXIT)
        { 
            player.GetComponent<PlayerMovement>().GoToExit(generator.SpawnExit());    // Движение персонажа к выходу
        }
    }

    private void Start()
    {
        generator = FindObjectOfType<LvlGenerator>();
        overview = FindObjectOfType<LvlOverview>();

        generator.GenerateRoom();
        overview.StartOverview(this);
    }
}
