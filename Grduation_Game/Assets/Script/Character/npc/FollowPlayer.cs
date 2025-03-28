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
        player = GameObject.FindWithTag("Player").transform;
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
            isFollowing = true;
            StartCoroutine(FollowPlayerCoroutine());
        }
    }

    IEnumerator FollowPlayerCoroutine()
    {
        animator?.SetBool("Walk", true);

        while (Vector3.Distance(transform.position, player.position) > followDistance)
        {
            Vector3 direction = player.position - transform.position;

            if (direction.x != 0)
                transform.localScale = new Vector3(Mathf.Sign(direction.x), 1, 1);

            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
            yield return null;
        }

        animator?.SetBool("Walk", false);
        isFollowing = false;
    }
}
