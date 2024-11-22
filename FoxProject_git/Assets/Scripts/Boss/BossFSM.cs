using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFSM
{
    private BossBaseStates _currentState;

    public BossFSM(BossBaseStates initState)
    {
        _currentState = initState;
        setState(_currentState);
    }

    public void setState(BossBaseStates nextState)
    {
        if (nextState == _currentState) return;

        if(_currentState != null)
        {
            _currentState.OnStateEnd();
        }
        _currentState = nextState;
        _currentState.OnStateStart();
    }

    public void UpdateState()
    {
        _currentState?.OnStateUpdate(); //_current가 null이 아니면 onstateupdate 실행
    }
}
