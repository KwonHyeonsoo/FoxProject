using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using static UnityEditor.Searcher.Searcher.AnalyticsEvent;

public enum EVENT_TYPE
{
    InitResourceLoaded,
    GameOver
};

public interface IListener
{
    // �̺�Ʈ�� �߻��� ��, �����ʿ��� ȣ���� �Լ�
    void OnEvent(EVENT_TYPE EventType, Component Sender, object Param = null);
}


public class EventManager
{

    private Dictionary<EVENT_TYPE, List<IListener>> Listeners = new Dictionary<EVENT_TYPE, List<IListener>>();



    //������ ��ü �߰�
    public void AddListener(EVENT_TYPE eventType, IListener Listener)
    {
        List<IListener> ListenList = null;

        /* �̺�Ʈ ���� Ű�� �����ϴ��� �˻�. �����ϸ� ����Ʈ�� �߰� */
        if (Listeners.TryGetValue(eventType, out ListenList))
        {
            ListenList.Add(Listener);
            return;
        }

        /* ������ ���ο� ����Ʈ ���� */
        ListenList = new List<IListener>();
        ListenList.Add(Listener);
        Listeners.Add(eventType, ListenList);    /* ������ ����Ʈ�� �߰� */
    }

    /// <summary>
    /// �̺�Ʈ�� �����ʿ��� �����ϱ� ���� �Լ�
    /// </summary>
    /// <param name="Event_Type">�ҷ��� �̺�Ʈ</param>
    /// <param name="Sender">�̺�Ʈ�� �θ��� ������Ʈ</param>
    /// <param name="Param">���� ������ �Ķ����</param>
    public void PostNotification(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        // ��� �����ʿ��� �̺�Ʈ�� ���� �˸���.

        // �� �̺�Ʈ�� �����ϴ� �����ʵ��� ����Ʈ
        List<IListener> ListenList = null;

        // �̺�Ʈ �׸��� ������, �˸� �����ʰ� �����Ƿ� ������.
        if (!Listeners.TryGetValue(Event_Type, out ListenList))
            return;

        // �׸��� �����Ѵ�. ���� ������ �����ʿ��� �˷��ش�.
        for (int i = 0; i < ListenList.Count; i++)
        {
            // ������Ʈ�� null�� �ƴϸ� �������̽��� ���� �޼����� ������.
            if (!ListenList[i].Equals(null))
                ListenList[i].OnEvent(Event_Type, Sender, Param);
        }
    }

    public void RemoveEvent(EVENT_TYPE Event_Type)
    {
        // ��ųʸ��� �׸��� �����Ѵ�.
        Listeners.Remove(Event_Type);
    }

    public void RemoveRedundancies()
    {
        // �� ��ųʸ� ����
        Dictionary<EVENT_TYPE, List<IListener>> TmpListeners = new Dictionary<EVENT_TYPE, List<IListener>>();

        // ��� ��ųʸ� �׸��� ��ȸ�Ѵ�
        foreach (KeyValuePair<EVENT_TYPE, List<IListener>> Item in Listeners)
        {
            // ����Ʈ�� ��� ������ ������Ʈ�� ��ȸ�ϸ� null ������Ʈ�� �����Ѵ�.
            for (int i = Item.Value.Count - 1; i >= 0; i--)
            {
                // null�̸� �׸��� �����.
                if (Item.Value[i].Equals(null))
                    Item.Value.RemoveAt(i);
            }

            // �˸��� �ޱ� ���� �׸�鸸 ����Ʈ�� ������ �� �׸���� �ӽ� ��ųʸ��� ��´�.
            if (Item.Value.Count > 0)
                TmpListeners.Add(Item.Key, Item.Value);
        }

        // ���� ����ȭ�� ��ųʸ��� ��ü�Ѵ�.
        Listeners = TmpListeners;
    }


    // ���� ����� �� ȣ��ȴ�. ��ųʸ��� û���Ѵ�.
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RemoveRedundancies();
    }


}
