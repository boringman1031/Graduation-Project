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
            Debug.LogError("BuffManager �䤣�� PlayerStats�I");
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
        chara.CurrentHealth += amount; // �P�ɸɦ�

        yield return new WaitForSeconds(duration);

        chara.MaxHealth = originalMaxHealth;
        if (chara.CurrentHealth > originalMaxHealth)
            chara.CurrentHealth = originalMaxHealth;

        chara.OnHealthChange?.Invoke(chara);
        healthBuffCoroutine = null;
    }

    // �Y�A���ݭn��s��¦�ȡ]�p����ɯš^�A�i�H�[�o��
    public void RefreshBaseStats()
    {
        originalSpeed = stats.speed;
        originalAttack = stats.attack;
    }
}
