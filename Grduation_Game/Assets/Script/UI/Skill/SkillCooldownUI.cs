using UnityEngine;
using UnityEngine.UI;

public class SkillCooldownUI : MonoBehaviour
{
    public Image cooldownOverlay;

    private float cooldownTime;
    private float lastUsedTime;
    private bool isCooling;

    public void StartCooldown(float duration)
    {
        cooldownTime = duration;
        lastUsedTime = Time.time;
        isCooling = true;
    }

    private void Update()
    {
        if (isCooling)
        {
            float elapsed = Time.time - lastUsedTime;
            float remaining = cooldownTime - elapsed;
            if (remaining <= 0)
            {
                cooldownOverlay.fillAmount = 0;
                isCooling = false;
            }
            else
            {
                cooldownOverlay.fillAmount = remaining / cooldownTime;
            }
        }
    }

    public bool IsCooling()
    {
        return isCooling;
    }

    public void ResetCooldown()
    {
        cooldownOverlay.fillAmount = 0f;
        isCooling = false;
    }
}
