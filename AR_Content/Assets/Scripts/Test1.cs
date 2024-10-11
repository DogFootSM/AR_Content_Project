using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test1 : MonoBehaviour
{
    [SerializeField] private GameObject arm;
    [SerializeField] private GameObject teller;
    [SerializeField] private GameObject Prefab;

    private bool isPlaying;


    private Coroutine recordPlayCo;
    private Coroutine recordStopCo;


    private void OnEnable()
    {
        Debug.Log("프리팹 생성");
    }

    private void OnDisable()
    {
        SoundManager.Instance.MusicStop();
    }

    Test1 tc;
    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(Camera.main.transform.position, ray.direction, Color.red);


        if (isPlaying && recordStopCo != null)
        {
            StopCoroutine(recordStopCo);
        }
        else if (!isPlaying && recordPlayCo != null)
        {
            StopCoroutine(recordPlayCo);
        }
 

        //테스트 코드
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, 500f))
            {
                 

                if (hit.collider.gameObject.tag == "Record")
                {
                    tc = hit.collider.gameObject.GetComponent<Test1>();

                    if (!tc.isPlaying)
                    {
                        tc.recordPlayCo = tc.StartCoroutine(tc.RecordPlayCoroutine());
                        tc.isPlaying = true;
                        Debug.Log("음악 재생");
                    }
                    else
                    {
                        Debug.Log("음악 종료");
                        tc.isPlaying = false;
                        tc.recordStopCo = tc.StartCoroutine(tc.RecordStopCoroutine());
                    }

                     
                    //isPlaying = true;
                    //recordPlayCo = StartCoroutine(RecordPlayCoroutine());

                }
                //else
                //{
                     
                //    Debug.Log("음악 종료");
                //    isPlaying = false;
                //    recordStopCo = StartCoroutine(RecordStopCoroutine());
                //}

            }
        }

 
    }



    public IEnumerator RecordPlayCoroutine()
    {
        float arm = this.arm.transform.localEulerAngles.y;
        float discRot = teller.transform.eulerAngles.y;

        //테스트 코드
        SoundManager.Instance.RandomPlay(Random.Range(0, 7));
        //UIManager.Instance.SetActiveGroup();
 
        while (isPlaying)
        {
            if (this.arm.transform.localEulerAngles.y < 45f)
            {
                arm += 20f * Time.deltaTime;
                this.arm.transform.localEulerAngles = new Vector3(0, arm, 0);
            }

            discRot = 20f * Time.deltaTime;
            teller.transform.Rotate(0, discRot, 0);

            yield return null;
        }

        yield break;
    }


    public IEnumerator RecordStopCoroutine()
    {

        float arm = this.arm.transform.localEulerAngles.y;
        SoundManager.Instance.MusicStop();
        //UIManager.Instance.SetActiveGroup();

        while (!isPlaying)
        {

            if (this.arm.transform.localEulerAngles.y > 1f)
            {
                arm -= 20f * Time.deltaTime;
                this.arm.transform.localEulerAngles = new Vector3(0, arm, 0);
            }


            yield return null;
        }

        yield break;
    }


}