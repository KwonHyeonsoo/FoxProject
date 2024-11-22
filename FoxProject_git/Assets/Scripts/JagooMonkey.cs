using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JagooMonkey : MonoBehaviour
{
    [SerializeField] float time = 2;
    private Boss_original bossComponent;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Die(time));
    }

    private void OnEnable()
    {
        GameObject _boss = GameObject.FindGameObjectsWithTag("Boss")[0];
        bossComponent = 
        _boss.GetComponent<Boss_original>();
        bossComponent.chaseToMonkey(transform);

        /*장구 원숭이 구현 의문점
         * 
         * 일단 onEnable하면 무조건 target이 원숭이로 집중 ->>>이건 원하는대로 구현된게 맞음
         * 근데 taget이 원숭이인 상태에서 원숭이를 disable해도 목적지가 monkey로 설정됨
         * 의도하면 다행이겠지만 이로인해 나올 수 있는 문제점은
         * 장구 원숭이의 지속시간(예를 들면 10초라고 했을 때)보다 거리가 멀경우에는 
         * 의도한 지속시간보다 보스를 더 멀리(막 20초까지) 어그로를 끌 수 있음
         * ==밸런스 파괴 문제
         * 
         */

    }
    private void OnDisable()
    {
        bossComponent.chaseToMonkey(null);

    }
    private IEnumerator Die(float time)
    {
        yield return new WaitForSeconds(time);
        bossComponent.chaseToMonkey(null);
    }
}
