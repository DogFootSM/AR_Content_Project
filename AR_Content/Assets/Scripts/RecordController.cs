using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class RecordController : MonoBehaviour
{
    [SerializeField] private GameObject arm;
    [SerializeField] private GameObject disk;
    [SerializeField] private TrackList trackList;

    private bool isPlaying;
 

    private Coroutine recordPlayCo;
    private Coroutine recordStopCo;

    private Music curMusic;
 

    private string albumName;
    public string AlbumName { get { return albumName; } set { albumName= value; } }


    private void OnDisable()
    {
        //비활성화 됐을 경우 arm 위치 초기화
        this.arm.transform.localEulerAngles = new Vector3(0, 0, 0);
        isPlaying = false;
    }

    private void Update()
    {
        
        if (isPlaying && recordStopCo != null)
        {
            StopCoroutine(recordStopCo);
        }
        else if (!isPlaying && recordPlayCo !=null)
        {
            StopCoroutine (recordPlayCo);
        }

        RecordTouch();

    }

    public void RecordTouch()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {

                if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, 500f))
                {

                    if (hit.collider.gameObject.tag == "Record")
                    { 
                        if (!isPlaying)
                        {
                            isPlaying = true;
                            GameManager.Instance.IsPlaying = isPlaying;
                            recordPlayCo = StartCoroutine(RecordPlayCoroutine());
                        }

                        else
                        {
                            isPlaying = false;
                            GameManager.Instance.IsPlaying = isPlaying;
                            recordStopCo = StartCoroutine(RecordStopCoroutine());
                        }
                    }
                }
            }
        }
    }

    //레코드 재생 코루틴
    public IEnumerator RecordPlayCoroutine()
    {
        float arm = this.arm.transform.localEulerAngles.y;
        float diskRot = disk.transform.eulerAngles.y;

        
        curMusic = trackList.GetMusic(albumName);

        //재생할 노래 클립
        SoundManager.Instance.MusicPlay(curMusic.TrackClip);

        //현재 재생할 노래 데이터
        UIManager.Instance.Info(curMusic);

        //UI 활성화
        UIManager.Instance.SetActiveGroup();


        while (isPlaying)
        {
            if(this.arm.transform.localEulerAngles.y < 45f)
            {
                arm += 20f * Time.deltaTime;
                this.arm.transform.localEulerAngles = new Vector3(0, arm, 0);
            }

            diskRot = 20f * Time.deltaTime;
            disk.transform.Rotate(0, diskRot, 0);

            yield return null;
        }

        yield break;
    }

    //레코드 정지 코루틴
    public IEnumerator RecordStopCoroutine()
    {
 
        float arm = this.arm.transform.localEulerAngles.y;

        //노래 재생 중지
        SoundManager.Instance.MusicStop();

        //UI 비활성화
        UIManager.Instance.SetActiveGroup();

        while (!isPlaying)
        {
            if(this.arm.transform.localEulerAngles.y > 1f)
            {
                arm -= 20f * Time.deltaTime;
                this.arm.transform.localEulerAngles = new Vector3(0, arm, 0);
            }
             
            yield return null;
        }

        yield break;
    }


}
