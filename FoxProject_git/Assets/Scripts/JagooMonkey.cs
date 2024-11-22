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

        /*�屸 ������ ���� �ǹ���
         * 
         * �ϴ� onEnable�ϸ� ������ target�� �����̷� ���� ->>>�̰� ���ϴ´�� �����Ȱ� ����
         * �ٵ� taget�� �������� ���¿��� �����̸� disable�ص� �������� monkey�� ������
         * �ǵ��ϸ� �����̰����� �̷����� ���� �� �ִ� ��������
         * �屸 �������� ���ӽð�(���� ��� 10�ʶ�� ���� ��)���� �Ÿ��� �ְ�쿡�� 
         * �ǵ��� ���ӽð����� ������ �� �ָ�(�� 20�ʱ���) ��׷θ� �� �� ����
         * ==�뷱�� �ı� ����
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
