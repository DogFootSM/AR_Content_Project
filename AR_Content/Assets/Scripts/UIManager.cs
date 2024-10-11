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

    private Music music;
    public Image Cover { get { return cover; } }

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

}
