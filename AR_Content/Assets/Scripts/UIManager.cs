using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.XR.ARFoundation;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject group;
    [SerializeField] private Image cover;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI singer;
    [SerializeField] private TextMeshProUGUI albumName;
    [SerializeField] private TextMeshProUGUI release;

    [SerializeField] private Sprite[] muteImages = new Sprite[2];
    [SerializeField] private Image muteImg;

    [SerializeField] private Sprite[] pauseImages = new Sprite[2];
    [SerializeField] private Image pauseImg;


    private Music music;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        group.SetActive(false);
    }
    
    /// <summary>
    /// �뷡 ��� ���ο� ���� UI Group Ȱ��ȭ ���� ����
    /// </summary>
    public void SetActiveGroup()
    {
         
        if (GameManager.Instance.IsPlaying)
        {
            group.SetActive(true); 
        }
        else
        {
            group.SetActive(false); 
        }
    }

    /// <summary>
    /// ������� �뷡 ������ �޾ƿ� UI ����
    /// </summary>
    /// <param name="music">�뷡 ������ ����ִ� Data</param>
    public void Info(Music music)
    {
        this.music = music;

        if (this.music != null)
        { 
            cover.sprite = music.AlbumCover;
            title.text = music.TrackName;
            singer.text = music.Singer;
            albumName.text = music.AlbumName;
            release.text = music.ReleaseDate;
        }     
    }

    public string CoverImage()
    { 
        return albumName.text;
    }

    public void MuteImageChange()
    {
        if (SoundManager.Instance.IsMute)
        {
            muteImg.sprite = muteImages[1];
        }
        else
        {
            muteImg.sprite = muteImages[0];
        }

    }

    public void PauseImageChange()
    {
        if (SoundManager.Instance.IsPlay)
        {
            pauseImg.sprite = pauseImages[1];
        }
        else
        {
            pauseImg.sprite = pauseImages[0];
        }
    }


}
