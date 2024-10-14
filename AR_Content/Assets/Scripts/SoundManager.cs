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

        //���� ���� �̺�Ʈ ������
        volumeSlider.onValueChanged.AddListener(SetVolume);

        //��ư Ŭ�� �̺�Ʈ ������
        muteButton.onClick.AddListener(MuteOnOff);
        pauseButton.onClick.AddListener(Pause);
    }

    private void Start()
    {
        //��ư Ŭ�� �̺�Ʈ ������
        //muteButton.onClick.AddListener(UIManager.Instance.MuteImageChange);
    }
 
    private void Update()
    {

        //����� ���°� ����
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
    /// ������� �ð�
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
    /// �뷡�� �� �ð�
    /// </summary>
    public void TotalPlayTime()
    {
       float endTime = musicAudio.clip.length;
       float minute = Mathf.Round(endTime / 60);
       float second = Mathf.Round(endTime % 60);

        endText.text = minute.ToString() + ":" + second.ToString();


    }

    /// <summary>
    /// Slider�� max ���� �뷡�� �� �ð� ���̷� ����
    /// ���� audioSource�� ������� �ð��� Slider�� ���� ����
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
