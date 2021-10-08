using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;

public class UltimateController : MonoBehaviour
{
    [SerializeField] Slider _ultimateSlider;
    [SerializeField] Button _ultimateButton;
    float currentCharge = 0;

    public void AddCharge(float value)
    {
        currentCharge += value;
        _ultimateSlider.value = currentCharge;
        if (currentCharge >= 100)
        {
            _ultimateButton.gameObject.SetActive(true);
        }
    }

    public void StartUltimate()
    {
        StartCoroutine(Ultimate());
    }

    IEnumerator Ultimate()
    {
        foreach (var item in FindObjectsOfType<AIPath>())
        {
            item.canMove = false;
            item.GetComponent<Enemy>().canFire = false;
        }

        yield return new WaitForSeconds(3);

        foreach (var item in FindObjectsOfType<AIPath>())
        {
            item.canMove = true;
            item.GetComponent<Enemy>().canFire = true;
        }

        currentCharge = 0;
        _ultimateSlider.value = currentCharge;
        _ultimateButton.gameObject.SetActive(false);
    }
}
