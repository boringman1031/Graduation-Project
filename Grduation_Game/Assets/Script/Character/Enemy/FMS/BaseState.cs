public abstract class BaseState 
{
    protected EnemyBase currentEnemy;
    public abstract void OnEnter(EnemyBase enemy);
    public abstract void LogicUpdate();
    public abstract void PhysicsUpdate();
    public abstract void OnExit();

}
