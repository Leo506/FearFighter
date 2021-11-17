using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum BossState
{
    FIRST_STATE,
    SECOND_STATE
}

public class BossController : MonoBehaviour
{
    BossState currentState = BossState.FIRST_STATE;
    public float delay = 3;  // Время между атаками
    public float HP = 100;
    public int countOfFire = 10;
    public float distanceMultiplier = 1;

    [SerializeField] FireBallController _firePrefab;  // Префаб снаряда, который будет пускать босс
    [SerializeField] UnityEngine.UI.Slider _hpSlider;
    [SerializeField] EnemyController _helper;

    CircleCollider2D cc2D;
    PlayeLogic playe;

    int countOfHelpers = 0;

    // Start is called before the first frame update
    void Start()
    {
        cc2D = GetComponent<CircleCollider2D>();
        playe = FindObjectOfType<PlayeLogic>();
        StartCoroutine(Delay());
    }

    public void GetDamage(float value)
    {
        HP -= value;
        _hpSlider.value = HP;

        if (HP <= 50)
            currentState = BossState.SECOND_STATE;
    }

    void Fire()
    {
        float radius = cc2D.radius * this.transform.localScale.x * distanceMultiplier;

        for (int j = 1; j <= 3; j++)
        {
            for (int i = 0; i < countOfFire; i++)
            {
                float angle = i * 2 * Mathf.PI / countOfFire;
                float x = Mathf.Cos(angle) * radius * (float)j/3 + this.transform.position.x;
                float y = Mathf.Sin(angle) * radius * (float)j / 3 + this.transform.position.y;

                var fireBall = Instantiate(_firePrefab);
                fireBall.transform.position = new Vector2(x, y);
                fireBall.SetDir(fireBall.transform.position - this.transform.position);
            }

            /*if (currentState == BossState.SECOND_STATE && countOfHelpers + 1 <= 6)
            {
                countOfHelpers++;
                Instantiate(_helper).SetTarget(playe.transform);
            }*/
        }

        

        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(delay);
        Fire();
    }

    
}
