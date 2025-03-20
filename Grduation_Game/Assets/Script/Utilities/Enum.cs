/*-----by017--------*/
public enum EenemyState
{
    Idle, Patrol, Chase,Attack
}
public enum BossState
{
    Attack,Summon, SummonHeart
}
public enum SceneName
{
    //場景類型:地點,特殊,必要關卡,BOSS,主頁面
    Chap1_Classroom, Chap1_School, Chap1_Cafe, Chap1_Cinema, Chap1_NightMarket,
    Chap1_Necessary, Chap1_Boss, Identity_89, Identity_Sport, Identity_Music,
    Identity_Rapper, Identity_Dancer, Menu
}

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
    MusicGame       // 音樂遊戲教學
}