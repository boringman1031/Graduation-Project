/*---------BY 017---------
 ------------Boss���A��--------*/
public abstract class BossBaseState
{
    protected BossBase currentBoss;
    public abstract void OnEnter(BossBase boss);
    public abstract void LogicUpdate();
    public abstract void PhysicsUpdate();
    public abstract void OnExit();

}
