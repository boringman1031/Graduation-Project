using System.Collections;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header("基本傷害")]
    public float baseDamage = 20f;

    private Transform attacker;
    private bool usePlayerStats = false;

    /// <summary>
    /// 初始化攻擊資訊
    /// </summary>
    /// <param name="origin">攻擊者</param>
    /// <param name="damage">基礎傷害</param>
    /// <param name="useStats">是否加上 PlayerStats.attack</param>
    public void Init(Transform origin, float damage, bool useStats)
    {
        attacker = origin;
        baseDamage = damage;
        usePlayerStats = useStats;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CharactorBase target = other.GetComponent<CharactorBase>();
        if (target == null || other.transform == attacker) return;

        float finalDamage = baseDamage;

        if (usePlayerStats && attacker != null)
        {
            PlayerStats stats = attacker.GetComponent<PlayerStats>();
            if (stats != null)
            {
                finalDamage += stats.attack;
            }
        }
        Debug.Log("造成傷害:" + finalDamage + "baseDamage" + baseDamage);
        target.TakeDamage(finalDamage, transform);
    }
}
