using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private GameObject arm;
    [SerializeField] private GameObject teller;
    [SerializeField] private GameObject Prefab;

    private bool isPlaying;


    private Coroutine recordPlayCo;
    private Coroutine recordStopCo;


    private void OnEnable()
    {
        Debug.Log("ÇÁ¸®ÆÕ »ý¼º");
    }

    private void OnDisable()
    {
        SoundManager.Instance.MusicStop();
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(Camera.main.transform.position, ray.direction, Color.red);

        if (Input.GetMouseButtonDown(1))
        {
            GameObject instance = Instantiate(Prefab, ray.direction, Quaternion.identity);

            

        }

    }



}