using UnityEngine;
using UnityEngine.SceneManagement;

// 掛上此腳本的物件在場景切換時會自動 Destroy
// 適用於技能特效、技能本體、持續型攻擊等
public class SkillAutoDestroy : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    private void OnSceneUnloaded(Scene scene)
    {
        Destroy(gameObject);
    }
}
