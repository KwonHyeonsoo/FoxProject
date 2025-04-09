using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTimer : MonoBehaviour
{
    public int TimerCount;
    void Start()
    {
        //Ÿ�̸� ���� ���

        //Ÿ�̸� �ڷ�ƾ ����
        StartCoroutine(TimerStart(TimerCount));
    }

    IEnumerator TimerStart(int timer)
    {
        yield return new WaitForSeconds(timer);
        //Ÿ�̸� ���� ��� ����
        //���� ���� ȣ��
        Managers.gameManager.GameOver();
        Destroy(this);
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}
