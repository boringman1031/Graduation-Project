using UnityEngine;

public class BossHealthUIManager : MonoBehaviour
{
    public CharactorBase bossCharacter;
    public BossHealthUI bossUI;

    private void OnEnable()
    {
        if (bossCharacter != null)
        {
            bossCharacter.OnHealthChange.AddListener(UpdateUI);
        }
    }

    private void OnDisable()
    {
        if (bossCharacter != null)
        {
            bossCharacter.OnHealthChange.RemoveListener(UpdateUI);
        }
    }

    private void UpdateUI(CharactorBase boss)
    {
        if (bossUI != null)
        {
            bossUI.UpdateHealth(boss.CurrentHealth, boss.MaxHealth);
        }
    }
}
