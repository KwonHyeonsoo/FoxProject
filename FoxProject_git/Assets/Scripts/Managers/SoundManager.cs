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
        MaxCount    //���ڸ� �������� enum ��
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
        _audioSources[(int)Sound.Bgm].loop = true; // bgm ������ ���� �ݺ� ���
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
        // ����� ���� ��� ��ž, ���� ����
        foreach (AudioSource audioSource in _audioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }
        // ȿ���� Dictionary ����
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
