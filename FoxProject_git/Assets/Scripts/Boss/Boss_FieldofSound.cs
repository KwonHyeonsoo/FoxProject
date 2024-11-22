using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_FieldofSound : MonoBehaviour
{
    //���簢�� ��� ����
    [SerializeField] [Range(1, 20)] float sides;
    [SerializeField] float target_height;

    private Vector3 collider_size;
    private Vector3 collider_position;

    // ����ũ 2��
    public LayerMask targetMask, obstacleMask;

    // Target mask�� ray hit�� transform�� �����ϴ� ����Ʈ
    public List<Transform> visibleTargets = new List<Transform>();


    // Start is called before the first frame update
    void Start()
    {
        collider_size.x = collider_size.z = sides;
        collider_size.y = 0;
        collider_position = transform.position;
        collider_position.y = collider_position.y-transform.parent.position.y + target_height;
        // 0.2�� �������� �ڷ�ƾ ȣ��
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
        // collider_size�� ���� �ڽ� ���� �� targetMask ���̾��� �ݶ��̴��� ��� ������
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

    //�þ� ���ü�
    private void OnDrawGizmos()
    {
        Gizmos.color  = Color.magenta;
        Gizmos.DrawCube(collider_position, 2*collider_size);
    }
}
