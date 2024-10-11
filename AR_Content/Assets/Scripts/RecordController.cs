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



    public IEnumerator RecordPlayCoroutine()
    {
        float arm = this.arm.transform.localEulerAngles.y;
        float diskRot = disk.transform.eulerAngles.y;


        curMusic = trackList.GetMusic(albumName);

        SoundManager.Instance.MusicPlay(curMusic.TrackClip);
        UIManager.Instance.Info(curMusic);
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

     
    public IEnumerator RecordStopCoroutine()
    {
 
        float arm = this.arm.transform.localEulerAngles.y;
        SoundManager.Instance.MusicStop();
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
