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
        // 次のステージシーンへの遷移ボタンを表示
        // 次のステージがない場合は遷移ボタンを無効化する（表示はしておく
        // その場合、ボタンのテキストを「全クリ！」みたいにしようかな
    }

    /// <summary>
    /// リスタート処理
    /// </summary>
    public void Restart()
    {
        // 同じステージを最初から始める
        Initiate.Fade(SceneManager.GetActiveScene().name, Color.black, 1.0f);
    }

    /// <summary>
    /// 命名規則に従い、次のステージのシーン名を取得
    /// </summary>
    /// <returns></returns>
    private string GetNextSceneName()
    {
        // 1. 現在のシーン名を取得 (例: "Stage1")
        string currentSceneName = SceneManager.GetActiveScene().name;

        // 2. 接頭辞 "Stage" を取り除いて数値部分だけに変換する (例: "1")
        string numberString = currentSceneName.Replace("Stage", "");

        // 3. 数値に変換して +1 する
        if (int.TryParse(numberString, out int currentStageNumber))
        {
            int nextStageNumber = currentStageNumber + 1;
            string nextSceneName = "Stage" + nextStageNumber; // 例: "Stage2"

            // 4. 次のシーンが存在するかチェックして読み込む
            if (Application.CanStreamedLevelBeLoaded(nextSceneName))
            {
                return nextSceneName;
            }
            else
            {
                // Stage10 をクリアした時など、次のシーンが無い場合の処理
                return null;
            }
        }

        Debug.LogError("シーン名からステージ番号を取得できませんでした: " + currentSceneName);
        return null;
    }

    /// <summary>
    /// 次のシーンへの遷移処理
    /// </summary>
    private void LoadNextStage()
    {
        string nextSceneName = GetNextSceneName();

        if (nextSceneName != null)
        {
            SceneManager.LoadScene(nextSceneName);
        }

        // 次のシーンがない場合は何もしない
    }
}
