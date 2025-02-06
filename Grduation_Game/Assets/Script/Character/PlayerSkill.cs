using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSkill : MonoBehaviour
{
    //------請支援輸贏--------
    public GameObject minionPrefab; // 指定小弟Prefab
    public Transform summonPoint; // 召喚位置
    public float cooldown = 3; // 冷卻時間
    float lastTime; // 用來計算冷卻時間用
    public void WinOrLoseSkill()
    {
        if (Time.time - lastTime < cooldown)
        {
            summonPoint = this.GetComponent<Transform>();
            Instantiate(minionPrefab, summonPoint.position, Quaternion.identity); // 召喚出小弟
            lastTime = Time.time;
        }
    }
}
