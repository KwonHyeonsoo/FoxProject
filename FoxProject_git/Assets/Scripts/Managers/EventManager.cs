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
    // 이벤트가 발생할 때, 리스너에서 호출할 함수
    void OnEvent(EVENT_TYPE EventType, Component Sender, object Param = null);
}


public class EventManager
{

    private Dictionary<EVENT_TYPE, List<IListener>> Listeners = new Dictionary<EVENT_TYPE, List<IListener>>();



    //리스너 객체 추가
    public void AddListener(EVENT_TYPE eventType, IListener Listener)
    {
        List<IListener> ListenList = null;

        /* 이벤트 형식 키가 존재하는지 검사. 존재하면 리스트에 추가 */
        if (Listeners.TryGetValue(eventType, out ListenList))
        {
            ListenList.Add(Listener);
            return;
        }

        /* 없으면 새로운 리스트 생성 */
        ListenList = new List<IListener>();
        ListenList.Add(Listener);
        Listeners.Add(eventType, ListenList);    /* 리스너 리스트에 추가 */
    }

    /// <summary>
    /// 이벤트를 리스너에게 전달하기 위한 함수
    /// </summary>
    /// <param name="Event_Type">불려진 이벤트</param>
    /// <param name="Sender">이벤트를 부르는 오브젝트</param>
    /// <param name="Param">선택 가능한 파라미터</param>
    public void PostNotification(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        // 모든 리스너에게 이벤트에 대해 알린다.

        // 이 이벤트를 수신하는 리스너들의 리스트
        List<IListener> ListenList = null;

        // 이벤트 항목이 없으면, 알릴 리스너가 없으므로 끝낸다.
        if (!Listeners.TryGetValue(Event_Type, out ListenList))
            return;

        // 항목이 존재한다. 이제 적합한 리스너에게 알려준다.
        for (int i = 0; i < ListenList.Count; i++)
        {
            // 오브젝트가 null이 아니면 인터페이스를 통해 메세지를 보낸다.
            if (!ListenList[i].Equals(null))
                ListenList[i].OnEvent(Event_Type, Sender, Param);
        }
    }

    public void RemoveEvent(EVENT_TYPE Event_Type)
    {
        // 딕셔너리의 항목을 제거한다.
        Listeners.Remove(Event_Type);
    }

    public void RemoveRedundancies()
    {
        // 새 딕셔너리 생성
        Dictionary<EVENT_TYPE, List<IListener>> TmpListeners = new Dictionary<EVENT_TYPE, List<IListener>>();

        // 모든 딕셔너리 항목을 순회한다
        foreach (KeyValuePair<EVENT_TYPE, List<IListener>> Item in Listeners)
        {
            // 리스트의 모든 리스너 오브젝트를 순회하며 null 오브젝트를 제거한다.
            for (int i = Item.Value.Count - 1; i >= 0; i--)
            {
                // null이면 항목을 지운다.
                if (Item.Value[i].Equals(null))
                    Item.Value.RemoveAt(i);
            }

            // 알림을 받기 위한 항목들만 리스트에 남으면 이 항목들을 임시 딕셔너리에 담는다.
            if (Item.Value.Count > 0)
                TmpListeners.Add(Item.Key, Item.Value);
        }

        // 새로 최적화된 딕셔너리로 교체한다.
        Listeners = TmpListeners;
    }


    // 씬이 변경될 때 호출된다. 딕셔너리를 청소한다.
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RemoveRedundancies();
    }


}
