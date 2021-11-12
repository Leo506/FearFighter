using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashBlock : MonoBehaviour, IChangingDirection
{
    FightController controller;
    BossController boss;
    PlayeLogic logic;

    public Vector2 ChangePlayerDirection(Vector2 dir, Vector3 normal, ref int rebounds)
    {
        Debug.Log("Collison with dash block");
        controller.ActiveNextBlock();
        StartCoroutine(DeactivateObj());
        logic.invulnerability = true;
        rebounds--;
        return (boss.transform.position - this.transform.position).normalized;
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = FindObjectOfType<FightController>();
        boss = FindObjectOfType<BossController>();
        logic = FindObjectOfType<PlayeLogic>();
    }

    IEnumerator DeactivateObj()
    {
        yield return new WaitForSeconds(1);
        this.gameObject.SetActive(false);
    }
}
