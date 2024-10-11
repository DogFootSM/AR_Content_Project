using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioSource musicSource;

    [SerializeField] private List<Music> trackList = new List<Music> ();

    private int soundCount;
    public int SoundCount { get { return soundCount; } }

    private Music curMusic;
    public Music CurMusic { get { return curMusic; } }

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

        soundCount = trackList.Count;
    }

 

    public void RandomPlay(int index)
    {
 
        SetTrack(trackList[index].AlbumName);

        musicSource.Play();
    }

    public void MusicPlay(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }


    public void MusicStop()
    {
        musicSource.Stop();
    }


    public void SetTrack(string albumName)
    {
        foreach(var track in trackList)
        {
            if(albumName == track.AlbumName)
            {
                //musicSource.clip = track.TrackClip;
                curMusic = track;
            }

        }

    }


}
