using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

//상태에 따른 행동만 적을 것!!! 상태변화는 여기서 하는게 아님!!!!!!!111
public abstract class BossBaseStates
{
    private Boss_original _boss;

    protected BossBaseStates(Boss_original boss)
    {
        _boss = boss;
    }

    public abstract void OnStateStart();
    public abstract void OnStateUpdate();
    public abstract void OnStateEnd();

}

public class IdleState : BossBaseStates
{
    private Boss_original _boss;

    public IdleState(Boss_original boss) : base(boss)
    {
        _boss = boss;
        
    }
    public override void OnStateStart()
    {

        _boss.setAnimator(Boss_original.BossStateEnum.idle);
        if (UnityEngine.Random.Range(0.0f, 1.0f) < 0.2)
            _boss.PlaySoundOneShot("Boss_growl");
    }

    public override void OnStateUpdate()
    {

        //Debug.Log(innerTimer);
    }

    public override void OnStateEnd()
    {

    }
}

public class PatrolState : BossBaseStates
{
    private Boss_original _boss;

    public PatrolState(Boss_original boss) : base(boss)
    {
        _boss = boss;
    }
    public override void OnStateStart()
    {
        _boss.PlaySoundLoop(2.0f);

        _boss.setAnimator(Boss_original.BossStateEnum.patrol);
        _boss.setNewWaypoints();
    }

    public override void OnStateUpdate()
    {

        

    }

    public override void OnStateEnd()
    {
        _boss.stopWaypoint();
        _boss.PlaySoundStop();
    }
}

public class ChaseState : BossBaseStates
{
    private Boss_original _boss;

    public ChaseState(Boss_original boss) : base(boss)
    {
        _boss = boss;
    }
    public override void OnStateStart()
    {
        
        //후에 추가되는 원숭이 고려
        //[count-1]이 원숭이이면 target에 넣지 안ㄹ기
        _boss.setAnimator(Boss_original.BossStateEnum.chase);
        if(_boss.isMonkey)
            _boss.updateDestination(_boss.getTarget());

        else if (_boss.getTargetsList().Count != 0 )
            _boss.updateDestination(_boss.getTargetsList()[_boss.getTargetsList().Count-1].gameObject);

        _boss.PlaySoundLoop(3.0f);
        //_boss.PlaySoundOneShot("Boss_roar");

    }

    public override void OnStateUpdate()
    {

        if (_boss.getTarget())
            _boss.updateDestination(_boss.getTarget());
    }

    public override void OnStateEnd()
    {
        _boss.PlaySoundStop();

    }
}


public class FuryState : BossBaseStates
{
    private Boss_original _boss;

    public FuryState(Boss_original boss) : base(boss)
    {
        _boss = boss;
    }
    public override void OnStateStart()
    {
        _boss.setAnimator(Boss_original.BossStateEnum.fury);
    }

    public override void OnStateUpdate()
    {
      
    }

    public override void OnStateEnd()
    {

    }
}

public class LookaroundState : BossBaseStates
{
    private Boss_original _boss;

    public LookaroundState(Boss_original boss) : base(boss)
    {
        _boss = boss;

    }
    public override void OnStateStart()
    {

        _boss.setAnimator(Boss_original.BossStateEnum.lookaround);
    }

    public override void OnStateUpdate()
    {
        //좌회전
        _boss.RotateView(Quaternion.Euler(Vector3.left), 3f);
        //원위치
        //우회전
        //원위치
    }

    public override void OnStateEnd()
    {

    }
}

