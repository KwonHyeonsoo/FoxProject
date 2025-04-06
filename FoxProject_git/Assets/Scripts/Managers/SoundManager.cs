using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundManager
{
    public enum Sound
    {
        UI, //_ui
        BGM,    //background
        BackgroundSFX,
        Player,    //�ȱ� �Ҹ�loop
        Boss,      //����oneshot, �޸��� loo[
        Vehicle,
        Others,   //_reflector, ���� ���� �۵���onesh,å �ѱ�� �Ҹ�oneshot,���� ���� Ŭ�����, ��¦ �����̴� �Ҹ�oneshot
        StorySound,     //only oneshot except death timer
        MaxCount    //���ڸ� �������� enum ��
    }
    public enum OneShotSound
    {
        _UI, DM, _reflctor
    }

    public enum LoopSound
    {
        _BackgroundSFX, _Player, _Timer

    }

    AudioSource[] _audioSources = new AudioSource[(int)Sound.MaxCount];
    public Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();
    AudioMixer AudioMixer;
    // Start is called before the first frame update
    public void Start()
    {
        Debug.Log("SoundManager");

        //AudioMixer�� ���ҽ�.�ε�
        if (AudioMixer == null)  AudioMixer = Resources.Load<AudioMixer>("Sound/DefaultAudioMixer");
        _audioClips = Managers.resourceManager._audioClips;
        string[] soundNames = System.Enum.GetNames(typeof(Sound)); // "Bgm", "Effect"
        for (int i = 0; i < soundNames.Length - 1; i++)
        {
            GameObject go = new GameObject { name = soundNames[i] };
            _audioSources[i] = go.AddComponent<AudioSource>();
            _audioSources[i].volume = 0.1f;
            if (i == 0)
            {
                _audioSources[i].outputAudioMixerGroup = AudioMixer.FindMatchingGroups("UI")[0];
            }
            else
            {
                _audioSources[i].outputAudioMixerGroup = AudioMixer.FindMatchingGroups("InGame")[0];

            }
            go.transform.parent = Managers.Instance.transform;
        }
        _audioClips = Managers.resourceManager._audioClips;

        _audioSources[(int)Sound.BGM].clip = _audioClips["_BackgroundSFX"];
        _audioSources[(int)Sound.BGM].loop = true; // bgm ������ ���� �ݺ� ���
        _audioSources[(int)Sound.BGM].Play();

        _audioSources[(int)Sound.UI].clip = _audioClips["_UI"];
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
    }
    void OnSceneUnLoaded(Scene scene)
    {
    }
    public AudioMixerGroup ReturnMatchingGroups(string str)
    {
        if (AudioMixer != null) { }  
        else
        {
            AudioMixer = Resources.Load<AudioMixer>("Sound/DefaultAudioMixer");
        }
        return AudioMixer.FindMatchingGroups(str)[0];
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

    public void PlaySoundStart(LoopSound type)
    {
        switch (type)
        {
            case LoopSound._BackgroundSFX:
                break;
            case LoopSound._Player:
                _audioSources[(int)Sound.Player].clip = _audioClips[LoopSound._Player.ToString()];
                _audioSources[(int)Sound.Player].loop = true;
                _audioSources[(int)Sound.Player].Play();
                break;
        }
    }
    public void PlaySoundEnd(LoopSound type)
    {
        switch (type)
        {
            case LoopSound._BackgroundSFX:
                break;
            case LoopSound._Player:
                _audioSources[(int)Sound.Player].Stop();
                break;
        }

    } 
    public void PlaySoundOneShot(OneShotSound type)
    {
        switch (type)
        {
            case OneShotSound._reflctor:
                _audioSources[(int)Sound.Others].PlayOneShot(_audioClips[OneShotSound._reflctor.ToString()]);
                break;
        }

    }


    public void PlayUIEffectOneShot() {
        _audioSources[(int)Sound.UI].PlayOneShot(_audioClips[OneShotSound._UI.ToString()]);
    }

    public void PlayStorySoudnOneShot(int ID) 
    {
        _audioSources[(int)Sound.StorySound].PlayOneShot(Managers.resourceManager.GetGameAudio(ID));
        _audioSources[(int)Sound.StorySound].Play();
    }
}
