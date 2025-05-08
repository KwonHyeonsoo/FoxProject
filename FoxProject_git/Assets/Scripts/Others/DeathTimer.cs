using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DeathTimer : MonoBehaviour
{
    public int TimerCount;
    void Start()
    {
        //타이머 사운드 재생
        Managers.soundManager.PlaySoundStart(SoundManager.LoopSound._Timer);

        //타이머 코루틴 시작
        StartCoroutine(TimerStart(TimerCount));
    }

    IEnumerator TimerStart(int timer)
    {
        yield return new WaitForSeconds(timer);

        //게임 오버 호출
        Managers.gameManager.CarPlayer.GetComponent<PlayableDirector>().Play();
        Destroy(this);
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }

    public void OnDestroy()
    {
        //타이머 사운드 재생 중지
        Managers.soundManager.PlaySoundEnd(SoundManager.LoopSound._Timer);
    }
}
