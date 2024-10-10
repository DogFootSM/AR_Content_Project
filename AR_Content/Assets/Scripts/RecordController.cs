using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class RecordController : MonoBehaviour
{
    [SerializeField] private GameObject arm;
    [SerializeField] private GameObject teller;

    private bool isPlaying;
 

    private Coroutine recordPlayCo;
    private Coroutine recordStopCo;


    private void OnEnable()
    {
        Debug.Log("프리팹 생성");
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
         
        Debug.DrawRay(Camera.main.transform.position, ray.direction, Color.red);


        if (isPlaying && recordStopCo != null)
        {
            StopCoroutine(recordStopCo);
        }
        else if (!isPlaying && recordPlayCo !=null)
        {
            StopCoroutine (recordPlayCo);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, 500f))
            {

                string tag = hit.collider.gameObject.tag;

                if (tag == "Record" && !isPlaying)
                {

                    Debug.Log("음악 재생");
                    isPlaying = true;
                    recordPlayCo = StartCoroutine(RecordPlayCoroutine());
                     
                }
                else
                {
                    Debug.Log("음악 종료");
                    isPlaying = false;
                    recordStopCo = StartCoroutine(RecordStopCoroutine());
                }

            }
        }

        //if (Input.touchCount > 0)
        //{
        //    Touch touch = Input.GetTouch(0);
           
        //    if (touch.phase == TouchPhase.Began)
        //    {

        //        if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, 5f))
        //        {
        //            string tag = hit.collider.gameObject.tag;

        //            if (tag == "Record" && !isPlaying)
        //            {
        //                Debug.Log("음악 재생");
        //                isPlaying = true;
        //                StartCoroutine(PlayCoroutine());
                         
        //            }
        //            else
        //            {
        //                StopCoroutine(PlayCoroutine());
        //                Debug.Log("음악 종료");
        //                isPlaying = false;
        //                StartCoroutine(StopCoroutine());
        //            }
        //        }
        //    }
        //}

    }



    public IEnumerator RecordPlayCoroutine()
    {
        float arm = this.arm.transform.localEulerAngles.y;
        float discRot = teller.transform.eulerAngles.y;

        //랜덤 재생 > 스캔한 이미지에 맞는 노래 재생 변경하기
        int random = Random.Range(0, SoundManager.Instance.SoundCount);
        SoundManager.Instance.RandomPlay(random);


        while (isPlaying)
        {
            if(this.arm.transform.localEulerAngles.y < 45f)
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

        while (!isPlaying)
        {
  
            if(this.arm.transform.localEulerAngles.y > 0.1f)
            {
                arm -= 20f * Time.deltaTime;
                this.arm.transform.localEulerAngles = new Vector3(0, arm, 0);
            }

             
            yield return null;
        }

        yield break;
    }


}
