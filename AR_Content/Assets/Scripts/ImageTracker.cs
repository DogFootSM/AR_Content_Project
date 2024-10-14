using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;



public class TrackInfo
{
    public ARTrackedImage trImage;
    public float timer;

}

public class ImageTracker : MonoBehaviour
{
    [SerializeField] private GameObject recordPrefab;
    [SerializeField] private float lostTimer;


    private ARTrackedImageManager arManager;
    private List<TrackInfo> trackImages = new List<TrackInfo>();



    private void Awake()
    {
        arManager = GetComponent<ARTrackedImageManager>();

    }

    private void OnEnable()
    {
        arManager.trackedImagesChanged += OnChanged;
    }

    private void OnDisable()
    {
        arManager.trackedImagesChanged -= OnChanged;
    }
 

    private void Update()
    {
        RecordHide();
    }

    public void RecordHide()
    {
        for (int i = 0; i < trackImages.Count; i++)
        {
            TrackInfo temp = trackImages[i];

            //이미지 트래킹중인 상태
            if (temp.trImage.trackingState == TrackingState.Tracking)
            {
                //노래가 재생중이지 않은 상태이면 트래킹된 모든 레코드 프리팹 활성화
                if (!SoundManager.Instance.IsPlay)
                { 
                    temp.timer = lostTimer;
                    temp.trImage.transform.GetChild(0).gameObject.SetActive(true);
                }

                //노래가 재생중인 상태에서는 현재 재생중인 노래가 아닌 레코드 프리팹 비활성화
                else
                {
                    //현재 재생중인 노래 커버이미지 제목 가져옴
                    string text = UIManager.Instance.CoverImage();

                    //재생중인 노래의 레코드가 아니면 비활성화
                    if (temp.trImage.referenceImage.name != text)
                    {
                        temp.trImage.transform.GetChild(0).gameObject.SetActive(false);
                    }
                }
            }
            
            //이미지를 놓친 상태
            else if (temp.trImage.trackingState == TrackingState.Limited)
            {
                //노래를 재생하지 않는 레코드만 비활성화
                if (!GameManager.Instance.IsPlaying)
                {
                     
                    temp.timer -= Time.deltaTime;

                    if (temp.timer < 0f)
                    {
                        temp.trImage.transform.GetChild(0).gameObject.SetActive(false); 
                    }
                }
            }
        }
    }
     

    private void OnChanged(ARTrackedImagesChangedEventArgs args)
    {

        foreach (var v in args.added)
        { 
            TrackInfo info = new TrackInfo();
            info.trImage = v;
            info.timer = lostTimer;
            trackImages.Add(info);
             
            GameObject instance = Instantiate(recordPrefab, v.transform.position, v.transform.rotation);
            instance.transform.SetParent(v.transform);
            instance.SetActive(false);

            //재생중인 앨범의 이름을 전달
            RecordController recordController = instance.GetComponent<RecordController>();
            recordController.AlbumName = v.referenceImage.name;
             
        }

        foreach (var v in args.updated)
        {
            v.transform.GetChild(0).gameObject.transform.SetPositionAndRotation(v.transform.position, v.transform.rotation);
        }

        foreach (var v in args.removed)
        {
            Destroy(v.transform.GetChild(0).gameObject);
        }

    }

}
