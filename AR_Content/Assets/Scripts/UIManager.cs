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
    /// 노래 재생 여부에 따라 UI Group 활성화 상태 변경
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
    /// 재생중인 노래 정보를 받아와 UI 변경
    /// </summary>
    /// <param name="music">노래 정보가 들어있는 Data</param>
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
