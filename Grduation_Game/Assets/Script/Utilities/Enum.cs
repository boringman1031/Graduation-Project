/*-----by017--------*/
public enum NPCState
{
    Patrol, Chase, Skill
}

public enum SceneType
{
    //場景類型:地點,特殊,必要關卡,BOSS,主頁面
    Chap1_Classroom, Chap1_School, Chap1_Cafe, Chap1_Cinema, Chap1_NightMarket,
    Chap1_Necessary, Chap1_Boss, Identity_89, Identity_Sport, Identity_Music,
    Identity_Rapper, Identity_Dancer, Menu
}

public enum PersistentType
{
    ReadWrite, DontPersistence
}