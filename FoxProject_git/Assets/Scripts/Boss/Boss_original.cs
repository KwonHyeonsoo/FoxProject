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

    //장구 원숭이 bool
    [HideInInspector]public bool isMonkey;

    //Components
    #region components
    private Animator animator;
    private NavMeshAgent agent;
    private AudioSource _audioSource;
    private Boss_FieldofView fieldOfview;
    private Boss_FieldofSound fieldOfsound;
    #endregion


    //sources
    private AudioClip _audio_growl; //일반?
    private AudioClip _audio_footstep;  //달릴때 피치 조정
    private AudioClip _audio_roar;  //추격시
    public enum BossStateEnum
    {
        idle,   //순찰 잠깐 정지
        patrol, //순찰 걷기
        chase,  //플레이어 추격
        fury,    //플레이어 더이상 추격 불가 땜빵용상태 아무의미 없음
        lookaround,  //플레이어가 근처에 있을때 두리번 거리기
        GameOver
    }
    [SerializeField] protected BossStateEnum _state;

    public void PlaySoundOneShot(string clip)
    {
        if(clip == "Boss_growl")
            _audioSource.PlayOneShot(_audio_growl);
        else if(clip == "Boss_roar")
            _audioSource.PlayOneShot(_audio_roar);

    }

    public bool IsPlaySoundStop()
    {
        return _audioSource.isPlaying;
    }
    public void PlaySoundLoop(float pitch) 
    {
        if(_audioSource.clip == null) _audioSource.clip = _audio_footstep;
        _audioSource.pitch = pitch;
        _audioSource.loop = true;
        _audioSource.Play();
    }
    public void PlaySoundStop() 
    {
        _audioSource.Pause();
    }
    void Start()
    {

        #region components
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        _audioSource = GetComponent<AudioSource>();
        fieldOfview = fieldofView_obj.GetComponent<Boss_FieldofView>();
        fieldOfsound = fieldofSound_obj.GetComponent<Boss_FieldofSound>();
        #endregion
        //agent.isStopped = true;

        _state = BossStateEnum.idle;
        _fsm = new BossFSM(new IdleState(this));
        setState(BossStateEnum.idle);

        isMonkey = false;

        //순찰 포인트 미지정
        if (wayPoints.Count == 0 || wayPoints[0] == null)
        {
            Debug.LogError("순찰 포인트 미지정!");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        agent.stoppingDistance = 1.0f;

        if (Managers.resourceManager.isLoaded)
        {
            _audio_growl = Managers.resourceManager._audioClips["Boss_growl"];
            _audio_footstep = Managers.resourceManager._audioClips["Boss_footstep"];
            _audio_roar = Managers.resourceManager._audioClips["Boss_roar"];  //
        }

        _audioSource.spatialBlend = 1.0f;
        SoundTargets = new List<Transform>();
        ViewTargets = new List<Transform>();
        StartCoroutine(updateTargetWithDelay(1f));    //시작하자마자 세팅하면 에러뜨더라고..
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("isOnNavMesh "+agent.isOnNavMesh);
        if(_audio_footstep == null)
        {
            if (Managers.resourceManager.isLoaded)
            {
                _audio_growl = Managers.resourceManager._audioClips["Boss_growl"];
                _audio_footstep = Managers.resourceManager._audioClips["Boss_footstep"];
                _audio_roar = Managers.resourceManager._audioClips["Boss_roar"];  //
                Managers.soundManager?.SetAudioMixer(_audioSource);
            }
        }
        if(Managers.soundManager != null) Managers.soundManager?.SetAudioMixer(_audioSource);
        switch (_state)
        {
            case BossStateEnum.idle:
                //타겟 발견
                if(ViewTargets?.Count > 0 || SoundTargets?.Count > 0 || isMonkey)
                {
                    setState(BossStateEnum.chase);
                    break;
                }

                //타임 아웃
                innerTimer += Time.deltaTime;
                if (innerTimer > innerRandomTime)
                {
                    setState(BossStateEnum.patrol);
                    break;
                }
                break;
            case BossStateEnum.patrol:
                //타겟 발견
                if (ViewTargets?.Count > 0 || SoundTargets?.Count > 0 || isMonkey)
                {
                    setState(BossStateEnum.chase);
                    break;
                }
                //Debug.Log("agent" + agent.remainingDistance + " hasPath "+ agent.hasPath);
                //waypoint 도착
                if (!agent.isOnNavMesh)
                {
                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(transform.position, out hit, 5.0f, NavMesh.AllAreas))
                    {
                        agent.Warp(hit.position);
                    }
                }
                else if (agent.remainingDistance < agent.stoppingDistance)
                {
                    setState(BossStateEnum.idle);
                    break;
                }
                break;
            case BossStateEnum.chase:
                //waypoint 도착
                //if (getDistanceTarget() < 0.1 && !agent.hasPath)
                //{
                //    setState(BossStateEnum.fury);
                //    break;
                //}
                //else if (ViewTargets.Count == 0 && SoundTargets.Count == 0)
                //{
                //    if (isMonkey) break;    //원숭이 쫓고 있을때는 chase유지
                //                            //setState(BossStateEnum.idle);//아니면 추격 중지
                //    setState(BossStateEnum.fury);
                //}
                break;
            case BossStateEnum.fury:
                //타임 아웃
                innerTimer += Time.deltaTime;
                if (innerTimer > innerRandomTime)
                {
                    setState(BossStateEnum.idle);
                    break;
                }
                break;
            case BossStateEnum.lookaround:
                //타임 아웃
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

        //Debug.Log("_audioSource.isPlaying "+ _audioSource.isPlaying);

        //==========현재 상태 실행==============
        _fsm.UpdateState(); //fsm내부의 UpdateState 실행 -> state의 OnStateUpdate 실행
        //Debug.Log("destination : " + agent.destination);

    }

    private void setState(BossStateEnum nextState)
    {
        _state = nextState;

        //Debug.Log(_state);

        switch (_state)
        {
            //setState 내부에 startOn이 포함되어 있음
            case BossStateEnum.idle:    //특정 시간 (5~10 초 동안 유지하고 patrol로 넘어가기)
                agent.isStopped = true;
                _fsm.setState(new IdleState(this));
                innerTimer = 0;
                innerRandomTime = 5;// Random.Range(1, 1);
                break;
            case BossStateEnum.patrol:  //이동 위치를 정하고 거기까지 전진
                agent.isStopped = false;
                _fsm.setState(new PatrolState(this));
                agent.speed = patrolSpeed;
                break;
            case BossStateEnum.chase:   //타겟을 추적
                _fsm.setState(new ChaseState(this));
                agent.speed = chaseSpeed;
                break;
            case BossStateEnum.fury:
                _fsm.setState(new FuryState(this));
                innerTimer = 0;
                innerRandomTime = 1;
                break;
            case BossStateEnum.lookaround:    //특정 시간 (5~10 초 동안 유지하고 patrol로 넘어가기)
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
        /*
        idle,   //순찰 잠깐 정지 <0>
        patrol, //순찰 걷기 정지<1>
        chase,  //플레이어 추격<2>
        fury,    //플레이어 더이상 추격 불가 땜빵용상태 아무의미 없음<3>
        lookaround  //플레이어가 근처에 있을때 두리번 거리기<4>

        */
        //state에 따라 애니메이션 적용
        //Debug.Log("setAnimator" + nextState);
        switch (nextState)
        {
            case BossStateEnum.GameOver:
                animator.SetTrigger("Trigger_GameOver");
                break;
            default:
                animator.SetInteger("State", (int)nextState);
                break;
        }
    }

    #region 길찾기
    public void setNewWaypoints()
    {
        int nextNumber;

        nextNumber = (currentWaypointNumber + 1) % wayPoints.Count;

        currentWaypointNumber = nextNumber;

        if (Vector3.Distance(wayPoints[currentWaypointNumber].transform.position, this.transform.position) < agent.stoppingDistance) return;
        agent.isStopped = false;
        agent.SetDestination(wayPoints[currentWaypointNumber].transform.position);


    }

    IEnumerator updateTargetWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            updateTarget();
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

    //플레이어 발견할 때 사용
    public void updateDestination(GameObject e)
    {
        agent.isStopped = false;
        target = e;
        agent.SetDestination(e.transform.position);
        if(_state != BossStateEnum.chase) setState(BossStateEnum.chase);

    }
    //원숭이 발견할 때 사용
    public void chaseToMonkey(Transform e)
    {
        //장구 소멸시
        if (!e)
        {
            isMonkey = false;
            return;
        }
        //장구 생성시
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
