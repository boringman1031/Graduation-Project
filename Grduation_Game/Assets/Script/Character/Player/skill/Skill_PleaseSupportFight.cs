using System.Collections;
using UnityEngine;

public class Skill_PleaseSupportFight : MonoBehaviour, ISkillEffect
{
    public GameObject thugPrefab1; // �p��1
    public GameObject thugPrefab2; // �p��2
    public Vector3 spawnOffset1 = new Vector3(-1.5f, 0f, 0f);
    public Vector3 spawnOffset2 = new Vector3(1.5f, 0f, 0f);
    public float lifeTime = 5f;
    private Transform origin;

    public AudioClip summonSound;
    public CharacterEventSO powerChangeEvent;
    public float energyCost = 50f;
    void SetPlayerAnimator(Animator animator)
    {
        
    }
    public void SetOrigin(Transform _origin)
    {
        origin = _origin;

        // ������q
        CharactorBase character = origin.GetComponent<CharactorBase>();
        if (character != null)
        {
            character.AddPower(-energyCost);
            powerChangeEvent?.OnEventRaised(character);
        }

        // �ͦ����p��
        if (thugPrefab1 != null)
        {
            GameObject thug1 = Instantiate(thugPrefab1, origin.position + spawnOffset1, Quaternion.identity);
            Destroy(thug1, lifeTime);
        }
        if (thugPrefab2 != null)
        {
            GameObject thug2 = Instantiate(thugPrefab2, origin.position + spawnOffset2, Quaternion.identity);
            Destroy(thug2, lifeTime);
        }

        // ����
        if (summonSound)
        {
            FindObjectOfType<AudioManager>()?.FXSource.PlayOneShot(summonSound);
        }
    }

    void ISkillEffect.SetPlayerAnimator(Animator animator)
    {
        throw new System.NotImplementedException();
    }
}
