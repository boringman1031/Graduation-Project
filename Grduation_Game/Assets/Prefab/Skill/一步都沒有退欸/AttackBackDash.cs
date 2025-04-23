using System.Collections.Generic;
using UnityEngine;

public class AttackBackDash : MonoBehaviour
{
    private Transform origin;
    private float damage;
    private bool useStats;
    private HashSet<CharactorBase> hitTargets = new HashSet<CharactorBase>();

    public void Init(Transform origin, float baseDamage, bool usePlayerStats)
    {
        this.origin = origin;
        this.damage = baseDamage;
        this.useStats = usePlayerStats;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CharactorBase target = other.GetComponent<CharactorBase>();
        if (target == null || hitTargets.Contains(target) || target.CompareTag("Player")) return;

        hitTargets.Add(target);

        float finalDamage = damage;

        if (useStats && origin != null)
        {
            var stats = origin.GetComponent<PlayerStats>();
            if (stats != null)
                finalDamage += stats.attack;
        }

        target.TakeDamage(finalDamage, transform);
    }
}
