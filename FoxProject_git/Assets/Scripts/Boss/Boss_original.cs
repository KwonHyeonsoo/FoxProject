using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss_original : MonoBehaviour
{
    private float innerTimer;
    private float innerRandomTime;

    private BossFSM _fsm;

    [SerializeField] private GameObject target;


    [SerializeField] private List<GameObject> wayPoints;
    [SerializeField] private int currentWaypointNumber = 0;
    [SerializeField] private GameObject fieldofView_obj;
    [SerializeField] private GameObject fieldofSound_obj;

    [SerializeField] [Range(0, 10)] private float patrolSpeed = 3.5f; 
    [SerializeField] [Range(0, 20)] private float chaseSpeed = 5f;

    private List<Transform> ViewTargets;
    private List<Transform> SoundTargets;

    //�屸 ������ bool
    [HideInInspector]public bool isMonkey;

    //Components
    #region components
    private Animator animator;
    private NavMeshAgent agent;
    private Boss_FieldofView fieldOfview;
    private Boss_FieldofSound fieldOfsound;
    #endregion

    public enum BossStateEnum
    {
        patrol, //���� �ȱ�
        idle,   //���� ��� ����
        chase,  //�÷��̾� �߰�
        fury,    //�÷��̾� ���̻� �߰� �Ұ� ��������� �ƹ��ǹ� ����
        lookaround  //�÷��̾ ��ó�� ������ �θ��� �Ÿ���
    }
    [SerializeField] protected BossStateEnum _state;

    void Start()
    {
        _state = BossStateEnum.idle;
        _fsm = new BossFSM(new IdleState(this));
        setState(BossStateEnum.idle);

        isMonkey = false;

        //���� ����Ʈ ������
        if (wayPoints.Count == 0 || wayPoints[0] == null)
        {
            Debug.LogError("���� ����Ʈ ������!");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }



        #region components
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        fieldOfview = fieldofView_obj.GetComponent<Boss_FieldofView>();
        fieldOfsound = fieldofSound_obj.GetComponent<Boss_FieldofSound>();
        #endregion 
        //agent.isStopped = true;

        StartCoroutine(updateTargetWithDelay(0.2f));    //�������ڸ��� �����ϸ� �����ߴ����..
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case BossStateEnum.idle:
                //Ÿ�� �߰�
                if(ViewTargets.Count > 0 || SoundTargets.Count > 0 || isMonkey)
                {
                    setState(BossStateEnum.chase);
                    break;
                }

                //Ÿ�� �ƿ�
                innerTimer += Time.deltaTime;
                if (innerTimer > innerRandomTime)
                {
                    setState(BossStateEnum.patrol);
                    break;
                }
                break;
            case BossStateEnum.patrol:
                //Ÿ�� �߰�
                if (ViewTargets.Count > 0 || SoundTargets.Count > 0 || isMonkey)
                {
                    setState(BossStateEnum.chase);
                    break;
                }


                //waypoint ����
                if (getDistanceTarget() < 0.1)
                {
                    setState(BossStateEnum.idle);
                    break;
                }
                break;
            case BossStateEnum.chase:
                //waypoint ����
                //if (getDistanceTarget() < 0.1 && !agent.hasPath)
                //{
                //    setState(BossStateEnum.fury);
                //    break;
                //}
                //else if (ViewTargets.Count == 0 && SoundTargets.Count == 0)
                //{
                //    if (isMonkey) break;    //������ �Ѱ� �������� chase����
                //                            //setState(BossStateEnum.idle);//�ƴϸ� �߰� ����
                //    setState(BossStateEnum.fury);
                //}
                break;
            case BossStateEnum.fury:
                //Ÿ�� �ƿ�
                innerTimer += Time.deltaTime;
                if (innerTimer > innerRandomTime)
                {
                    setState(BossStateEnum.idle);
                    break;
                }
                break;
            case BossStateEnum.lookaround:
                //Ÿ�� �ƿ�
                innerTimer += Time.deltaTime;
                if (innerTimer > innerRandomTime)
                {
                    setState(BossStateEnum.idle);
                    break;
                }
                break;
            default:
                break;        
        }


        //==========���� ���� ����==============
        _fsm.UpdateState(); //fsm������ UpdateState ���� -> state�� OnStateUpdate ����
        //Debug.Log("destination : " + agent.destination);

    }

    private void setState(BossStateEnum nextState)
    {
        _state = nextState;

        Debug.Log(_state);

        switch (_state)
        {
            //setState ���ο� startOn�� ���ԵǾ� ����
            case BossStateEnum.idle:    //Ư�� �ð� (5~10 �� ���� �����ϰ� patrol�� �Ѿ��)
                _fsm.setState(new IdleState(this));
                innerTimer = 0;
                innerRandomTime = 1;// Random.Range(1, 1);
                break;
            case BossStateEnum.patrol:  //�̵� ��ġ�� ���ϰ� �ű���� ����
                _fsm.setState(new PatrolState(this));
                agent.speed = patrolSpeed;
                break;
            case BossStateEnum.chase:   //Ÿ���� ����
                _fsm.setState(new ChaseState(this));
                agent.speed = chaseSpeed;
                break;
            case BossStateEnum.fury:
                _fsm.setState(new FuryState(this));
                innerTimer = 0;
                innerRandomTime = 1;
                break;
            case BossStateEnum.lookaround:    //Ư�� �ð� (5~10 �� ���� �����ϰ� patrol�� �Ѿ��)
                _fsm.setState(new IdleState(this));
                innerTimer = 0;
                innerRandomTime = 5;// Random.Range(1, 1);
                break;
            default:
                break;
        }
    }

    public void setAnimator(BossStateEnum nextState)
    {
        //state�� ���� �ִϸ��̼� ����
    }

    #region ��ã��
    public void setNewWaypoints()
    {
        int nextNumber;

        nextNumber = (currentWaypointNumber + 1) % wayPoints.Count;

        currentWaypointNumber = nextNumber;

        agent.isStopped = false;
        agent.SetDestination(wayPoints[currentWaypointNumber].transform.position);

    }

    IEnumerator updateTargetWithDelay(float delay)
    {
        while (true)
        {
            updateTarget();
            yield return new WaitForSeconds(delay);
        }
    }

    private void updateTarget() 
    {

        ViewTargets = fieldOfview.getVisibleTarget();
        SoundTargets =  fieldOfsound.getVisibleTarget();
    }


    public float getDistanceTarget()
    {
        return agent.remainingDistance;
    }

    //�÷��̾� �߰��� �� ���
    public void updateDestination(GameObject e)
    {
        agent.isStopped = false;
        target = e;
        agent.SetDestination(e.transform.position);
        if(_state != BossStateEnum.chase) setState(BossStateEnum.chase);

    }
    //������ �߰��� �� ���
    public void chaseToMonkey(Transform e)
    {
        //�屸 �Ҹ��
        if (!e)
        {
            isMonkey = false;
            return;
        }
        //�屸 ������
        isMonkey = true;
        agent.isStopped = false;
        target = e.gameObject;
        agent.SetDestination(e.position);
        if (_state != BossStateEnum.chase) setState(BossStateEnum.chase);

    }

    public void stopWaypoint()
    {
        agent.isStopped = true;
    }
    #endregion

    public List<Transform> getTargetsList()
    {
        List<Transform> tmpList = new List<Transform>();
        tmpList.AddRange(ViewTargets);
        tmpList.AddRange(SoundTargets);
        return tmpList;
    }
    public GameObject getTarget()
    {
        return target;
    }
    
    public void StartLookaround() { 
        setState(BossStateEnum.lookaround);
    }
    public void RotateView(Quaternion angle, float rotationTime)
    {
        Quaternion newRotation = angle;

        fieldofView_obj.transform.rotation = Quaternion.Slerp(fieldofView_obj.transform.rotation, newRotation , 2);
    }
}
