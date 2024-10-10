using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioSource musicSource;

    [SerializeField] private List<Music> trackList = new List<Music> ();

    private int soundCount;
    public int SoundCount { get { return soundCount; } }

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
    }

    private void Start()
    {
        soundCount = trackList.Count;
    }


    public void RandomPlay(int index)
    {
        musicSource.clip = trackList[index].TrackClip;
        musicSource.Play();
    }

    public void MusicStop()
    {
        musicSource.Stop();
    }


}
