/*---------BY 017---------
 ------------Bossª¬ºA¾÷--------*/
public abstract class BossBaseState
{
    protected EnemyBase currentEnemy;
    public abstract void OnEnter(BossBase boss);
    public abstract void LogicUpdate();
    public abstract void PhysicsUpdate();
    public abstract void OnExit();

}
