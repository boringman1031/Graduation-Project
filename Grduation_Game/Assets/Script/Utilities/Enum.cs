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
    //��������:�a�I,�S��,���n���d,BOSS,�D����
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
    None,           // �L�о�
    Move,           // ���ʱо�
    Jump,           // ���D�о�
    Attack,         // �����о�
    MusicGame       // ���ֹC���о�
}