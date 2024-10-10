using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(menuName = "Music/MusicData")]

public class Music : ScriptableObject
{
    [Header("앨범 표지")]
    [SerializeField] private Sprite albumCoverImage;
    public Sprite AlbumCover { get { return albumCoverImage; } }

    [Header("노래 파일")]
    [SerializeField] private AudioClip trackClip;
    public AudioClip TrackClip { get { return trackClip; } }

    [Header("가수")]
    [SerializeField] private string singer;
    public string Singer { get { return singer; } }

    [Header("노래 제목")]
    [SerializeField] private string trackName;
    public string TrackName { get { return trackName; } }


    [Header("앨범명")]
    [SerializeField] private string albumName;
    public string AlbumName { get { return albumName; } }

    [Header("발매일")]
    [SerializeField] private string releaseDate;
    public string ReleaseDate { get { return releaseDate; } }

}
