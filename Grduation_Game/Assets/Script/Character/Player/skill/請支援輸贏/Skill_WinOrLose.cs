using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_WinOrLose : MonoBehaviour, ISkillEffect
{
    [Header("召喚設定")]
    public GameObject minion1Prefab;
    public GameObject minion2Prefab;
    public float summonOffsetX = 1.5f; // 左右各偏移
    public AudioClip summonSound;
    public AudioDefination audioPlayer;

    [Header("能量消耗")]
    public float energyCost = 50f;
    public CharacterEventSO powerChangeEvent;

    private Transform origin;

    public void SetOrigin(Transform origin)
    {
        this.origin = origin;

        CharactorBase character = origin.GetComponent<CharactorBase>();
        if (character == null)
        {
            Debug.LogWarning("找不到 CharactorBase");
            Destroy(gameObject);
            return;
        }

        if (character.CurrentPower < energyCost)
        {
            Debug.Log("能量不足，無法施放技能");
            Destroy(gameObject);
            return;
        }

        // 扣能量
        character.AddPower(-energyCost);

        // 播音效
        if (audioPlayer && summonSound)
        {
            audioPlayer.audioClip = summonSound;
            audioPlayer.PlayAudioClip();
        }

        // 召喚左右兩邊小弟
        Vector3 left = origin.position + new Vector3(-summonOffsetX, 0.3f, 0);
        Vector3 right = origin.position + new Vector3(summonOffsetX, 0.3f, 0);

        if (minion1Prefab != null)
        {
            GameObject m1 = Instantiate(minion1Prefab, left, Quaternion.identity);
            m1.AddComponent<SkillAutoDestroy>();

            var follower = m1.GetComponent<MinionFollower>();
            if (follower != null)
                follower.SetOrigin(origin); // ✅ 傳入玩家資訊
        }

        if (minion2Prefab != null)
        {
            GameObject m2 = Instantiate(minion2Prefab, right, Quaternion.identity);
            m2.AddComponent<SkillAutoDestroy>();

            var follower = m2.GetComponent<MinionFollower>();
            if (follower != null)
                follower.SetOrigin(origin);
        }


        Destroy(gameObject);
    }

    public void SetPlayerAnimator(Animator animator) { }
}
