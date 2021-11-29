using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioControl : MonoBehaviour
{
    public static AudioControl current;//当前音量控制脚本
    [SerializeField]
    private AudioMixer m_audioMixer;
    [SerializeField]
    public AudioSource fallBall, throwBall;
    public float soundVolume, bgmVolume;
    private void Awake()
    {
        current = this;
    }
    public void SetSoundVolume(float volume)
    {
        soundVolume = volume;
        m_audioMixer.SetFloat("SoundVolume", volume);
    }
    public void SetBGMVolume(float volume)
    {
        bgmVolume = volume;
        m_audioMixer.SetFloat("BGMVolume", volume);
    }
}
