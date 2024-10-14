using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] public Slider volumeSlider;
    [SerializeField] public Button muteButton;
    [SerializeField] public Button pauseButton;

    [SerializeField] private TextMeshProUGUI startText;
    [SerializeField] private TextMeshProUGUI endText;


    [SerializeField] private Slider progressBar;

    [SerializeField] private AudioSource musicAudio;

    [SerializeField] private AudioMixer audioMixer;
 
    private bool isPlay;
    public bool IsPlay { get { return isPlay; } }

    private bool isMute;
    public bool IsMute { get { return isMute; } }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        }
        else
        {
            Destroy(gameObject);
        }

        //사운드 조절 이벤트 리스너
        volumeSlider.onValueChanged.AddListener(SetVolume);

        //버튼 클릭 이벤트 리스너
        muteButton.onClick.AddListener(MuteOnOff);
        pauseButton.onClick.AddListener(Pause);
    }

    private void Start()
    {
        //버튼 클릭 이벤트 리스너
        //muteButton.onClick.AddListener(UIManager.Instance.MuteImageChange);
    }
 
    private void Update()
    {

        //재생중 상태값 변경
        if (isPlay != musicAudio.isPlaying)
        {
            isPlay = musicAudio.isPlaying;
        }


        if (isPlay)
        { 
            ProgressBar();
            TotalPlayTime();
            StartPlayTime();
        }
 

    }

    /// <summary>
    /// 재생중인 시간
    /// </summary>
    public void StartPlayTime()
    {
        
        float minute = (int)(musicAudio.time / 60);
        float second = (int)(musicAudio.time % 60);

        if(second < 10)
        {
            startText.text = minute.ToString() + ":0" + second.ToString();
        }
        else
        {
            startText.text = minute.ToString() + ":" + second.ToString();
        }
         
    }

    /// <summary>
    /// 노래의 총 시간
    /// </summary>
    public void TotalPlayTime()
    {
       float endTime = musicAudio.clip.length;
       float minute = Mathf.Round(endTime / 60);
       float second = Mathf.Round(endTime % 60);

        endText.text = minute.ToString() + ":" + second.ToString();


    }

    /// <summary>
    /// Slider의 max 값을 노래의 총 시간 길이로 변경
    /// 현재 audioSource가 재생중인 시간을 Slider의 값에 대입
    /// </summary>
    public void ProgressBar()
    {
        progressBar.maxValue = musicAudio.clip.length;

        progressBar.value = musicAudio.time;

    }

    public void Pause()
    {
        if (musicAudio.isPlaying)
        {
            musicAudio.Pause(); 
        }
        else
        {
            musicAudio.UnPause();
        }
        UIManager.Instance.PauseImageChange();
    }

    public void MuteOnOff()
    {
        if (!musicAudio.mute)
        {
            musicAudio.mute = true;
        }
        else
        {
            musicAudio.mute = false;
        }
         
        isMute = musicAudio.mute;
        UIManager.Instance.MuteImageChange();
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("MUSIC", volume * 20); 
    }
 

    public void MusicPlay(AudioClip clip)
    {
        musicAudio.clip = clip;
        musicAudio.Play();
    }


    public void MusicStop()
    {
        musicAudio.Stop();
    }
     
}
