using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(menuName = "Music/MusicData")]

public class Music : ScriptableObject
{
    [Header("�ٹ� ǥ��")]
    [SerializeField] private Sprite albumCoverImage;
    public Sprite AlbumCover { get { return albumCoverImage; } }

    [Header("�뷡 ����")]
    [SerializeField] private AudioClip trackClip;
    public AudioClip TrackClip { get { return trackClip; } }

    [Header("����")]
    [SerializeField] private string singer;
    public string Singer { get { return singer; } }

    [Header("�뷡 ����")]
    [SerializeField] private string trackName;
    public string TrackName { get { return trackName; } }


    [Header("�ٹ���")]
    [SerializeField] private string albumName;
    public string AlbumName { get { return albumName; } }

    [Header("�߸���")]
    [SerializeField] private string releaseDate;
    public string ReleaseDate { get { return releaseDate; } }

}
