using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VollyBallController : MonoBehaviour
{
    [Header("球速設定")]
    public float speed = 400f;
    public float speedIncrease = 1.05f;
    public float angleVariation = 0.5f;

    [Header("UI 參考")]
    public RectTransform canvasRect;
    public RectTransform playerPaddle;
    public RectTransform aiPaddle;
    public RectTransform leftGoal;
    public RectTransform rightGoal;

    [Header("UI相關")]  
    public Text playerScoreText;
    public Text scoreText;
    public int winningScore = 20;
    public GameObject startPromptText; // 提示 "按任意鍵開始"
    public GameObject winTextPanel; // 顯示勝利 UI

    [Header("特效")]
    public GameObject playerScoreEffect;
    public GameObject aiScoreEffect;

    private int playerScore = 0;
    private RectTransform rectTransform;
    private Vector2 direction;
    private bool gameOver = false;
    private bool gameStarted = false;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.localPosition = Vector3.zero;
        startPromptText.SetActive(true); // 顯示提示
    }

    void Update()
    {
        if (!gameStarted)
        {
            if (Input.GetKey(KeyCode.L))
            {
                StartGame();
            }
            return; // 沒開始前不做球邏輯
        }
        if (gameOver) return; // 若遊戲結束，不再更新
        // 球移動
        rectTransform.localPosition += (Vector3)(direction * speed * Time.deltaTime);

        // 上下邊界反彈
        float minY = -canvasRect.rect.height / 2 + rectTransform.rect.height / 2;
        float maxY = canvasRect.rect.height / 2 - rectTransform.rect.height / 2;

        if (rectTransform.localPosition.y <= minY || rectTransform.localPosition.y >= maxY)
        {
            BounceOffWall();
        }

        // Paddle 碰撞
        if (BallHitsPaddle(playerPaddle))
        {
            BounceOffPaddle(playerPaddle);
        }
        else if (BallHitsPaddle(aiPaddle))
        {
            BounceOffPaddle(aiPaddle);
        }

        // Goal 判定
        if (IsOverlapping(rectTransform, rightGoal))
        {
            AIScores();
            ResetBall();
        }
        else if (IsOverlapping(rectTransform, leftGoal))
        {
            PlayerScores();
            ResetBall();
        }
    }

    private void StartGame()
    {
        gameStarted = true;
        startPromptText.gameObject.SetActive(false);
        ServeBall();// 開始發球
    }
    public void ServeBall()
    {
        direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    private void BounceOffWall()// 球碰到上下邊界時的反彈
    {
        float xVariation = Random.Range(-angleVariation, angleVariation);
        direction = new Vector2(direction.x + xVariation, -direction.y).normalized;
    }

    private void BounceOffPaddle(RectTransform paddle)// 球碰到球拍時的反彈
    {
        float offset = (rectTransform.localPosition.y - paddle.localPosition.y) / (paddle.rect.height / 2);
        direction = new Vector2(-direction.x, offset).normalized;
        speed *= speedIncrease;
        if(paddle == playerPaddle)
        {
            Instantiate(playerScoreEffect, rectTransform.position, Quaternion.identity);
            PlayerScores();
        }     
    }

    private void ResetBall()// 重置球的位置
    {
        rectTransform.localPosition = Vector3.zero;
        speed = 400f;
        StartCoroutine(DelayServe());
    }

    private IEnumerator DelayServe()
    {
        yield return new WaitForSeconds(1f);
        ServeBall();
    }

    // ================== 分數系統 ==================
    private void PlayerScores()
    {
        playerScore++;
        playerScoreText.text = "你很有實力嘛!!";
        scoreText.text =$"目前分數:{playerScore}";
        if (playerScore >= winningScore)
        {
            EndGame(true);
        }
    }

    private void AIScores()
    {
        playerScore--;
        playerScoreText.text = "笑死就這??";
        scoreText.text = $"目前分數:{playerScore}";
    }

    private void EndGame(bool playerWon)
    {
        gameOver = true;
        rectTransform.localPosition = Vector3.zero;

        winTextPanel.SetActive(true);   
    }
    // ================== 改良碰撞判斷 ==================
    private bool BallHitsPaddle(RectTransform paddle)
    {
        float paddleMinY = paddle.localPosition.y - paddle.rect.height / 2;
        float paddleMaxY = paddle.localPosition.y + paddle.rect.height / 2;

        float ballY = rectTransform.localPosition.y;
        float ballX = rectTransform.localPosition.x;
        float paddleX = paddle.localPosition.x;

        float overlapThreshold = (rectTransform.rect.width / 2 + paddle.rect.width / 2);

        return ballY >= paddleMinY && ballY <= paddleMaxY &&
               Mathf.Abs(ballX - paddleX) <= overlapThreshold;
    }

    private bool IsOverlapping(RectTransform a, RectTransform b)
    {
        Rect rectA = GetWorldRect(a);
        Rect rectB = GetWorldRect(b);
        return rectA.Overlaps(rectB);
    }

    private Rect GetWorldRect(RectTransform rt)
    {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);
        return new Rect(corners[0], corners[2] - corners[0]);
    }

    // ================== 可視化碰撞框 ==================
    private void OnDrawGizmos()
    {
        if (playerPaddle != null)
        {
            Gizmos.color = Color.green;
            DrawRectGizmo(playerPaddle);
        }
        if (aiPaddle != null)
        {
            Gizmos.color = Color.red;
            DrawRectGizmo(aiPaddle);
        }
        if (rectTransform != null)
        {
            Gizmos.color = Color.yellow;
            DrawRectGizmo(rectTransform);
        }
    }

    private void DrawRectGizmo(RectTransform rt)
    {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);
        for (int i = 0; i < 4; i++)
        {
            Gizmos.DrawLine(corners[i], corners[(i + 1) % 4]);
        }
    }
}
