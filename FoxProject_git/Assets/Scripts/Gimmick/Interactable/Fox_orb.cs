using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Fox_orb : InteractableObject
{
    public GameObject black_clyliner;
    public override void Invoke(GameObject playerObject)
    {
        //������ ����� Ȱ��ȭ & ������� �÷��̾�� ������ outline off
        Instantiate(black_clyliner, transform);
        //�ó׸ӽ�, ���� �� �ִϸ��̼�, ����ũ����
        GetComponent<PlayableDirector>().Play();

        //�ϴû��� �ٲٰ�

        //��ڿ� ����ũ������ �ø���
    }
}
