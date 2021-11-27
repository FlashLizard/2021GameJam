using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour//设置界面UI
{
    [SerializeField]
    private Slider m_bgmSlider, m_soundSlider;//音量条
    private void OnEnable()
    {
        m_bgmSlider.value = AudioControl.current.bgmVolume;
        m_soundSlider.value = AudioControl.current.soundVolume;
    }
    public void SetSoundVolume(float volume)
    {
        AudioControl.current.SetSoundVolume(volume);
    }
    public void SetBGMVolume(float volume)
    {
        AudioControl.current.SetBGMVolume(volume);
    }
}
