using System.Collections;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    private PlayerStats stats;

    private float originalSpeed;
    private float originalAttack;
    private float originalMaxHealth;

    private Coroutine healthBuffCoroutine;
    private Coroutine speedBuffCoroutine;
    private Coroutine attackBuffCoroutine;

    private void Awake()
    {
        stats = GetComponent<PlayerStats>();
        if (stats == null)
        {
            Debug.LogError("BuffManager 找不到 PlayerStats！");
        }
        else
        {
            originalSpeed = stats.speed;
            originalAttack = stats.attack;
        }
    }

    public void ApplySpeedBuff(float amount, float duration)
    {
        if (speedBuffCoroutine != null)
            StopCoroutine(speedBuffCoroutine);

        speedBuffCoroutine = StartCoroutine(SpeedBuffRoutine(amount, duration));
    }

    public void ApplyAttackBuff(float amount, float duration)
    {
        if (attackBuffCoroutine != null)
            StopCoroutine(attackBuffCoroutine);

        attackBuffCoroutine = StartCoroutine(AttackBuffRoutine(amount, duration));
    }
    public void ApplyMaxHealthBuff(float amount, float duration)
    {
        if (healthBuffCoroutine != null)
            StopCoroutine(healthBuffCoroutine);

        healthBuffCoroutine = StartCoroutine(MaxHealthBuffRoutine(amount, duration));
    }
    private IEnumerator SpeedBuffRoutine(float amount, float duration)
    {
        stats.speed = originalSpeed + amount;
        yield return new WaitForSeconds(duration);
        stats.speed = originalSpeed;
        speedBuffCoroutine = null;
    }

    private IEnumerator AttackBuffRoutine(float amount, float duration)
    {
        stats.attack = originalAttack + amount;
        yield return new WaitForSeconds(duration);
        stats.attack = originalAttack;
        attackBuffCoroutine = null;
    }

    private IEnumerator MaxHealthBuffRoutine(float amount, float duration)
    {
        var chara = GetComponent<CharactorBase>();
        if (chara == null) yield break;

        originalMaxHealth = chara.MaxHealth;
        chara.MaxHealth += amount;
        chara.CurrentHealth += amount; // 同時補血

        yield return new WaitForSeconds(duration);

        chara.MaxHealth = originalMaxHealth;
        if (chara.CurrentHealth > originalMaxHealth)
            chara.CurrentHealth = originalMaxHealth;

        chara.OnHealthChange?.Invoke(chara);
        healthBuffCoroutine = null;
    }

    // 若你有需要刷新基礎值（如角色升級），可以加這個
    public void RefreshBaseStats()
    {
        originalSpeed = stats.speed;
        originalAttack = stats.attack;
    }
}
