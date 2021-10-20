using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class LvlOverview : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera camera;
    public float startCamSize;
    public float endCamSize;
    public float overviewTime;


    void Start() {
    	StartOverview();
    }


    public void StartOverview() {
    	StartCoroutine(Overview());
    }


    IEnumerator Overview() {
    	var step = (endCamSize - startCamSize) / (overviewTime * 100);

    	camera.m_Lens.OrthographicSize = startCamSize;
    	for (int i = 0; i < overviewTime * 100; i++) {
    		camera.m_Lens.OrthographicSize += step;
    		yield return new WaitForSeconds(0.01f);
    	}

    	for (int i = 0; i < overviewTime * 100; i++) {
    		camera.m_Lens.OrthographicSize -= step;
    		yield return new WaitForSeconds(0.01f);
    	}

    	FindObjectOfType<LvlGenerator>().SpawnPlayer();
    }
}
