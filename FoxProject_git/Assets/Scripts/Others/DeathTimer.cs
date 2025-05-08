using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DeathTimer : MonoBehaviour
{
    public int TimerCount;
    void Start()
    {
        //Ÿ�̸� ���� ���
        Managers.soundManager.PlaySoundStart(SoundManager.LoopSound._Timer);

        //Ÿ�̸� �ڷ�ƾ ����
        StartCoroutine(TimerStart(TimerCount));
    }

    IEnumerator TimerStart(int timer)
    {
        yield return new WaitForSeconds(timer);

        //���� ���� ȣ��
        Managers.gameManager.CarPlayer.GetComponent<PlayableDirector>().Play();
        Destroy(this);
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }

    public void OnDestroy()
    {
        //Ÿ�̸� ���� ��� ����
        Managers.soundManager.PlaySoundEnd(SoundManager.LoopSound._Timer);
    }
}
