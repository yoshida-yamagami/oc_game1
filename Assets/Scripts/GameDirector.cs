using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDirector : MonoBehaviour
{
    [SerializeField] GameObject gameClearObject;        // ゲームクリア表示
    [SerializeField] GameObject gameOverObject;         // ゲームオーバー表示
    [SerializeField] GameObject restartButton;          // リスタートボタン
    [SerializeField] UnityEngine.UI.Image errorMessage; // ゲーム開始不可表示

    private int resetCount = -1; // 非稼働時は-1。数値がセットされるとカウントダウン、0になるとシーンリセット実行

    private void Start()
    {
        // テキストやボタンを隠す
        gameClearObject.SetActive(false);
        gameOverObject.SetActive(false);
        restartButton.SetActive(false);
        errorMessage.gameObject.SetActive(false);

        // ゲーム開始要件をチェック
        Validate();
    }

    void FixedUpdate()
    {
        if (resetCount > 0)
        {
            resetCount--;
            if (resetCount <= 0)
            {
                // ここでシーンリセット
                Restart();
            }
        }
    }

    /// <summary>
    /// ゲーム開始要件をチェック
    /// </summary>
    private bool Validate()
    {
        // 一つ以上ゴールタグのついたオブジェクトが存在することをチェック
        GameObject goal = GameObject.FindWithTag("Goal");
        if (goal == null)
        {
            // ゴールできる状態でない(=ゴールタグのついたオブジェクトが存在しない)
            // 場合はエラーメッセージを出す
            errorMessage.gameObject.SetActive(true);
            return false;
        }

        // どこにも引っかからなければゲーム開始OK
        return true;
    }

    /// <summary>
    /// ゲームオーバー時の処理
    /// </summary>
    public void GameOver()
    {
        gameOverObject.SetActive(true); // ゲームオーバーの文字表示
        resetCount = 50;                // ゲームリセットまでのカウンターセット
    }

    /// <summary>
    /// ゲームクリア時の処理
    /// </summary>
    public void GameClear()
    {
        gameClearObject.SetActive(true);    // ゲームクリアの文字表示
        restartButton.SetActive(true);      // リセットボタンを表示
    }

    /// <summary>
    /// リスタート処理
    /// </summary>
    public void Restart()
    {
        // 同じステージを最初から始める
        Initiate.Fade(SceneManager.GetActiveScene().name, Color.black, 1.0f);
    }
}
