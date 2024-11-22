using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_FieldofSound : MonoBehaviour
{
    //정사각형 모양 범위
    [SerializeField] [Range(1, 20)] float sides;
    [SerializeField] float target_height;

    private Vector3 collider_size;
    private Vector3 collider_position;

    // 마스크 2종
    public LayerMask targetMask, obstacleMask;

    // Target mask에 ray hit된 transform을 보관하는 리스트
    public List<Transform> visibleTargets = new List<Transform>();


    // Start is called before the first frame update
    void Start()
    {
        collider_size.x = collider_size.z = sides;
        collider_size.y = 0;
        collider_position = transform.position;
        collider_position.y = collider_position.y-transform.parent.position.y + target_height;
        // 0.2초 간격으로 코루틴 호출
        StartCoroutine(FindTargetsWithDelay(0.2f));


    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            FindVisibleTargets();
            yield return new WaitForSeconds(delay);
        }
    }

    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        collider_position = transform.position;
        collider_position.y = collider_position.y - transform.parent.position.y + target_height;
        // collider_size를 가진 박스 영역 내 targetMask 레이어인 콜라이더를 모두 가져옴
        Collider[] targetsInViewRadius = Physics.OverlapBox(collider_position, collider_size, Quaternion.identity, targetMask);

        foreach (Collider e in targetsInViewRadius)
        {
            visibleTargets.Add(e.transform);
        }
    }

    public List<Transform> getVisibleTarget()
    {
        return visibleTargets;
    }

    //시야 가시성
    private void OnDrawGizmos()
    {
        Gizmos.color  = Color.magenta;
        Gizmos.DrawCube(collider_position, 2*collider_size);
    }
}
