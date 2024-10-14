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

            //�̹��� Ʈ��ŷ���� ����
            if (temp.trImage.trackingState == TrackingState.Tracking)
            {
                //�뷡�� ��������� ���� �����̸� Ʈ��ŷ�� ��� ���ڵ� ������ Ȱ��ȭ
                if (!SoundManager.Instance.IsPlay)
                { 
                    temp.timer = lostTimer;
                    temp.trImage.transform.GetChild(0).gameObject.SetActive(true);
                }

                //�뷡�� ������� ���¿����� ���� ������� �뷡�� �ƴ� ���ڵ� ������ ��Ȱ��ȭ
                else
                {
                    //���� ������� �뷡 Ŀ���̹��� ���� ������
                    string text = UIManager.Instance.CoverImage();

                    //������� �뷡�� ���ڵ尡 �ƴϸ� ��Ȱ��ȭ
                    if (temp.trImage.referenceImage.name != text)
                    {
                        temp.trImage.transform.GetChild(0).gameObject.SetActive(false);
                    }
                }
            }
            
            //�̹����� ��ģ ����
            else if (temp.trImage.trackingState == TrackingState.Limited)
            {
                //�뷡�� ������� �ʴ� ���ڵ常 ��Ȱ��ȭ
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

            //������� �ٹ��� �̸��� ����
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
