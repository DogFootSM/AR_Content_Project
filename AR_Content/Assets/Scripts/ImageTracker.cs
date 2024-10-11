using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        //추적할 이미지 개수를 사운드 트랙 개수로 변경
        arManager.requestedMaxNumberOfMovingImages = SoundManager.Instance.SoundCount;
    }

    private void Update()
    {
 
        for(int i = 0; i < trackImages.Count; i++)
        {
            TrackInfo temp = trackImages[i];

            if(temp.trImage.trackingState == TrackingState.Tracking)
            {
                temp.timer = lostTimer;
                temp.trImage.transform.GetChild(0).gameObject.SetActive(true);
            }
            else if(temp.trImage.trackingState == TrackingState.Limited)
            { 
 
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

            RecordController recordController = instance.GetComponent<RecordController>();
            recordController.AlbumName = v.referenceImage.name;
        }

        foreach (var v in args.updated)
        {
            v.transform.GetChild(0).gameObject.transform.SetPositionAndRotation(v.transform.position, v.transform.rotation);
        }

        foreach(var v in args.removed)
        {
            Destroy(v.transform.GetChild(0).gameObject);
        }

    }

}
