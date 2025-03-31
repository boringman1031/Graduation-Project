using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public VoidEventSO dialogEndEvent;
    public Transform player;
    public float followDistance = 2.0f;
    public float moveSpeed = 2.0f;

    private Animator animator;
    private bool isFollowing = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        dialogEndEvent.OnEventRaised += OnDialogEnd;
    }

    private void OnDisable()
    {
        dialogEndEvent.OnEventRaised -= OnDialogEnd;
    }

    void OnDialogEnd()
    {
        if (!isFollowing)
        {
            if (player == null)
            {
                GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");
                if (foundPlayer != null)
                {
                    player = foundPlayer.transform;
                }
                else
                {
                    Debug.LogWarning("Player not found in current scene.");
                    return;
                }
            }

            isFollowing = true;
            StartCoroutine(FollowPlayerCoroutine());
        }
    }

    IEnumerator FollowPlayerCoroutine()
    {
        while (true)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            Vector3 direction = player.position - transform.position;

            // 正確翻轉角色
            if (direction.x != 0)
            {
                float originalX = Mathf.Abs(transform.localScale.x);
                float faceX = direction.x > 0 ? originalX : -originalX;
                transform.localScale = new Vector3(-faceX, transform.localScale.y, transform.localScale.z);
            }

            // 動畫與移動同步
            bool shouldMove = distance > followDistance;
            animator?.SetBool("Walk", shouldMove);

            if (shouldMove)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
            }

            yield return null;
        }
    }
}
