/*-----by017--------*/
public enum EenemyState
{
    Idle, Patrol, Chase,Attack
}
public enum BossState
{
    Idle,Attack, Summon, SummonHeart
}

public enum Chapter { Chap1, Chap2, Chap3 }


public enum SceneType
{
    Menu, Boss, Special, Necessary, Location
}


public enum PersistentType
{
    ReadWrite, DontPersistence
}
public enum TutorialType
{
    None,           // 無教學
    Move,           // 移動教學
    Jump,           // 跳躍教學
    Attack,         // 攻擊教學
    MusicGame,      // 音樂遊戲教學
    TriviaGame,     // 對話遊戲
    CleanEnemy,     // 清除敵人教學
    DefeatEnemy,     // 打敗一定數量敵人教學
    Boss,           // boss戰教學
    UnlockSkill,    // 解鎖技能提醒
    SkillAndClass,   //更換職業教學
    Boss3
}
public enum SkillType
{
    Q_Skill,
    W_Skill,
    E_Skill,
    R_Ultimate, // 大招绑定职业
}