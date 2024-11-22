using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondBoss : Boss_original
{
    //BossStateEnum ���
    //_state
    private BossFSM _fsm;

    private GameObject target;


    // Start is called before the first frame update
    void Start()
    {
        _state = BossStateEnum.idle;
        _fsm = new BossFSM(new IdleState(this));
    }

    // Update is called once per frame
    void Update()
    {
        
        switch (_state)
        {
            //SetState�� ���� ����
            case BossStateEnum.idle:
                break;
            case BossStateEnum.patrol:
                break;
            case BossStateEnum.chase:
                break;
            default:
                break;

        }


        //==========���� ���� ����==============
        _fsm.UpdateState();
    }

    private void SetState(BossStateEnum nextState)
    {
        _state = nextState;

        switch (_state)
        {
            case BossStateEnum.idle:
                _fsm.setState(new IdleState(this));
                break;
            case BossStateEnum.patrol:
                _fsm.setState(new PatrolState(this));
                break;
            case BossStateEnum.chase:
                _fsm.setState(new ChaseState(this));
                break;
            default:
                break;
        }
    }
}
