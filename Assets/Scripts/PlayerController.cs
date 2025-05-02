using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
public class PlayerController : MonoBehaviour
{
    public float speed; // 動く速さ
    public TMP_Text scoreText; // スコアの UI
    public TMP_Text winText; // リザルトの UI
    private Rigidbody rb; // Rididbody
    private int score; // スコア

    // 修正：時間
    public float timeLimit = 120f; //　時間
    public TMP_Text timerText; // 時間UI
    private float timeRemaining;
    private bool gameEnded = false;

    // 修正：一時停止
    private bool stopTrapped = false;

    void Start()
    {
        // Rigidbody を取得
        rb = GetComponent<Rigidbody>();

        // UI を初期化
        score = 0;
        SetCountText();
        winText.text = "";

        //　修正：時間, 時間を初期化
        timeRemaining = timeLimit;
    }

    void Update()
    {
        // 修正：時間、時間反映
        if (!gameEnded)
        {
            timeRemaining -= Time.deltaTime;
            timerText.text = "Time: " + Mathf.CeilToInt(timeRemaining).ToString();

            if (timeRemaining <= 0 && score < 12)
            {
                timeRemaining = 0;
                GameOver();
            }

            // 修正：一時停止
            if (!stopTrapped)
            {
                // カーソルキーの入力を取得
                var moveHorizontal = Input.GetAxis("Horizontal");
                var moveVertical = Input.GetAxis("Vertical");

                // カーソルキーの入力に合わせて移動方向を設定
                var movement = new Vector3(moveHorizontal, 0, moveVertical);

                // Ridigbody に力を与えて玉を動かす
                rb.AddForce(movement * speed);
            }
        }
    }

    // 玉が他のオブジェクトにぶつかった時に呼び出される
    void OnTriggerEnter(Collider other)
    {
        // ぶつかったオブジェクトが収集アイテムだった場合
        if (other.gameObject.CompareTag("Pick Up"))
        {
            // その収集アイテムを非表示にします
            other.gameObject.SetActive(false);

            // スコアを加算します
            score = score + 1;

            // UI の表示を更新します
            SetCountText();
        }

        // 修正：Score trap
        else if (other.gameObject.CompareTag("Score trap"))
        {
            other.gameObject.SetActive(false);
            score = Mathf.Max(score - 1, 0);
            SetCountText();
        }

        // 修正：Stop trap
        else if (other.gameObject.CompareTag("Stop trap"))
        {
            other.gameObject.SetActive(false);
            StartCoroutine(StopTrapEffect());
        }
    }

    // UI の表示を更新する
    void SetCountText()
    {
        // スコアの表示を更新
        scoreText.text = "Count: " + score.ToString();

        // すべての収集アイテムを獲得した場合
        if (score >= 12)
        {
            // リザルトの表示を更新
            winText.text = "You Win!";
            gameEnded = true;

            // ボールを完全に止める
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    void GameOver()
    {
        gameEnded = true;
        winText.text = "Game Over!";

        // ボールを完全に止める
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    IEnumerator StopTrapEffect()
    {
        stopTrapped = true;

        // ボールを一時止める
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        yield return new WaitForSeconds(5f);

        stopTrapped = false;
    }
}