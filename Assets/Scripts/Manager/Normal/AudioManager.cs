using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 音频管理
/// </summary>
public class AudioManager {
    private AudioSource[] audioSources;//0, 循环播背景音乐，1，播特效音；
    private bool playEffectMusic = true;
    private bool playBGMusic = true;

    public AudioManager()
    {
        audioSources = GameManager.Instance.GetComponents<AudioSource>();
    }

    public void PlayBGMusic(AudioClip audioClip)
    {
        if (!audioSources[0].isPlaying||audioSources[0].clip!= audioClip)
        {
            audioSources[0].clip = audioClip;
            audioSources[0].Play();
        }
    }
    public void PlayEffectMusic(AudioClip audioClip)
    {
        if (playEffectMusic)
        {
            audioSources[1].PlayOneShot(audioClip);
        }
    }
    public void CloseBGMusic()
    {
        audioSources[0].Stop();
    }
    public void OpenBGMusic()
    {
        audioSources[0].Play(); 
    }

    public void CloseOrOpenBGMusic()
    {
        playBGMusic = !playBGMusic;

        if (playBGMusic)
        {
            OpenBGMusic();
        }
        else
        {
            CloseBGMusic();
        }
    }
    public void CloseOrOpenEffectMusic()
    {
        playEffectMusic = !playEffectMusic;

        if (playEffectMusic)
        {
            OpenBGMusic();
        }
        else
        {
            CloseBGMusic();
        }
    }
}
