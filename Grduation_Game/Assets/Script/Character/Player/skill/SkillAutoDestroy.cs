using UnityEngine;
using UnityEngine.SceneManagement;

// ���W���}��������b���������ɷ|�۰� Destroy
// �A�Ω�ޯ�S�ġB�ޯॻ��B���򫬧�����
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
