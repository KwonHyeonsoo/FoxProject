using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager
{
    public enum Sound
    {
        Bgm,
        InGameEffect,
        UI_Effet,
        StorySound,
        MaxCount    //숫자를 세기위한 enum 값
    }

    AudioSource[] _audioSources = new AudioSource[(int)Sound.MaxCount];
    public Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

    // Start is called before the first frame update
    public void Start()
    {
        Debug.Log("SoundManager");

        _audioClips = Managers.resourceManager._audioClips;
        string[] soundNames = System.Enum.GetNames(typeof(Sound)); // "Bgm", "Effect"
        for (int i = 0; i < soundNames.Length - 1; i++)
        {
            GameObject go = new GameObject { name = soundNames[i] };
            _audioSources[i] = go.AddComponent<AudioSource>();
            go.transform.parent = Managers.Instance.transform;
        }
        _audioClips = Managers.resourceManager._audioClips;

        _audioSources[(int)Sound.Bgm].clip = _audioClips["BGM01"];
        _audioSources[(int)Sound.Bgm].loop = true; // bgm 재생기는 무한 반복 재생
        _audioSources[(int)Sound.Bgm].Play();

        _audioSources[(int)Sound.UI_Effet].clip = _audioClips["SFX01"];
    }

    // Update is called once per frame
    public void Update()
    {

    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
    }
    void OnSceneUnLoaded(Scene scene)
    {
    }
    public void Clear()
    {
        // 재생기 전부 재생 스탑, 음반 빼기
        foreach (AudioSource audioSource in _audioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }
        // 효과음 Dictionary 비우기
        _audioClips.Clear();
    }

    public void PlayEffectStart()
    {

    }
    public void PlayEffectEnd()
    {

    } 
    public void PlayEffectOneShot()
    {

    }
    public void PlayUIEffectOneShot() {
        _audioSources[(int)Sound.UI_Effet].PlayOneShot(_audioClips["SFX01"]);
    }

    public void PlayStorySoudnOneShot(int ID) 
    {
        _audioSources[(int)Sound.StorySound].clip = _audioClips["SFX02"];
        _audioSources[(int)Sound.StorySound].loop = false; 
        _audioSources[(int)Sound.StorySound].Play();
    }
}
