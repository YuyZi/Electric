using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AudioController : SingletonMono<AudioController>
{   
    public AudioClip[] audioClips;
    private Dictionary<string, AudioSource> audioSources;
    public override void OnInit()
    {
        base.OnInit();
        // 加载AudioClip
        audioClips = Resources.LoadAll<AudioClip>("Audio");
        //自动生成子物体并添加AudioSource组件
        audioSources = new Dictionary<string, AudioSource>();
        for (int i = 0; i < audioClips.Length; i++)
        {
            GameObject child = new GameObject(audioClips[i].name);
            child.transform.SetParent(transform);
            AudioSource audioSource = child.AddComponent<AudioSource>();
            audioSource.clip = audioClips[i];
            audioSource.playOnAwake = false;
            audioSources.Add(audioClips[i].name, audioSource);
        }

    }
    //通过传入的名称来调用对应的音频
    public void AudioPlay(string name)
    {
        if (audioSources.ContainsKey(name))
        {
            audioSources[name].loop = false;
            audioSources[name].Play();
        }
    }
    public void AudioPlayLoop(string name)
    {
        if (audioSources.ContainsKey(name))
        {
            audioSources[name].loop = true;
            audioSources[name].Play();
        }
    }
    public void AudioStop(string name)
    {
        if (audioSources.ContainsKey(name))
        {
            audioSources[name].Stop();
        }
    }
}
