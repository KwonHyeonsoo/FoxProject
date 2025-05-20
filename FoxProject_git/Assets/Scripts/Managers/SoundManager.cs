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
        Player,    //걷기 소리loop
        Boss,      //괴성oneshot, 달리기 loo[
        Vehicle,
        Deathtimer,
        Others,   //_reflector, 깡통 차량 작동음onesh,책 넘기는 소리oneshot,주차 퍼즐 클리어시, 문짝 움직이는 소리oneshot
        StorySound,     //only oneshot except death timer
        MaxCount    //숫자를 세기위한 enum 값
    }
    public enum OneShotSound
    {
        _UI, DM, _reflctor, ClearSound
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

        //AudioMixer를 리소스.로드
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
        _audioSources[(int)Sound.BGM].loop = true; // bgm 재생기는 무한 반복 재생
        _audioSources[(int)Sound.BGM].Play();

        _audioSources[(int)Sound.UI].clip = _audioClips["_UI"];
    }

    public void SetAudioMixer(AudioSource audioSource)
    {
        audioSource.outputAudioMixerGroup = AudioMixer.FindMatchingGroups("InGame")[0];
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _audioSources[(int)Sound.BGM]?.Play();
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
        // 재생기 전부 재생 스탑, 음반 빼기
        foreach (AudioSource audioSource in _audioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }
        // 효과음 Dictionary 비우기
        _audioClips.Clear();
    }

    public void Stop()
    {

        foreach(AudioSource audioSource in _audioSources) { audioSource.Stop(); }
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
            case LoopSound._Timer:
                _audioSources[(int)Sound.Deathtimer].clip = _audioClips[LoopSound._Timer.ToString()];
                _audioSources[(int)Sound.Deathtimer].loop = true;
                _audioSources[(int)Sound.Deathtimer].Play();
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
            case LoopSound._Timer:
                _audioSources[(int)Sound.Deathtimer].Stop();
                _audioSources[(int)Sound.Others].Stop();
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
            case OneShotSound.ClearSound:
                _audioSources[(int)Sound.Others].PlayOneShot(_audioClips[OneShotSound.ClearSound.ToString()]);
                break;


        }

    }


    public void PlayUIEffectOneShot() {
        _audioSources[(int)Sound.UI].PlayOneShot(_audioClips[OneShotSound._UI.ToString()]);
    }

    public void PlayStorySoudnOneShot(int ID) 
    {
        _audioSources[(int)Sound.StorySound].Stop();
        //_audioSources[(int)Sound.StorySound].PlayOneShot(Managers.resourceManager.GetGameAudio(ID));
        _audioSources[(int)Sound.StorySound].clip = Managers.resourceManager.GetGameAudio(ID);
        _audioSources[(int)Sound.StorySound].Play();
    }
}
