using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    [SerializeField] GameObject gameClearObject;        // ゲームクリア表示
    [SerializeField] GameObject gameOverObject;         // ゲームオーバー表示
    [SerializeField] GameObject restartButton;          // リスタートボタン
    [SerializeField] GameObject nextButton;             // 次のステージへの遷移ボタン
    [SerializeField] UnityEngine.UI.Image errorMessage; // ゲーム開始不可表示

    private int resetCount = -1; // 非稼働時は-1。数値がセットされるとカウントダウン、0になるとシーンリセット実行

    private void Start()
    {
        // テキストやボタンを隠す
        gameClearObject.SetActive(false);
        gameOverObject.SetActive(false);
        restartButton.SetActive(false);
        nextButton.SetActive(false);
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
        gameClearObject.SetActive(true); // ゲームクリアの文字表示
        restartButton.SetActive(true);   // リセットボタンを表示
        nextButton.SetActive(true);      // 次のステージへの遷移ボタンを表示

        // 次のステージがない場合は遷移ボタンを無効化する（表示はしておく
        if (GetNextSceneName() == null)
        {
            nextButton.GetComponent<Button>().interactable = false;
            nextButton.transform.Find("NextButtonText").GetComponent<Text>().text = "全クリ！";
        }
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
    /// <returns>次ステージがあればscene名を、なければnullを返す</returns>
    private string GetNextSceneName()
    {
        // 次シーンの情報を得るため現シーンの情報を取得
        int currentStageNumber;
        string currentSceneName = SceneManager.GetActiveScene().name;
        string numberString = currentSceneName.Replace("Stage", "");
        if (!int.TryParse(numberString, out currentStageNumber))
        {
            // ステージ番号を得られない = 次シーンがない
            Debug.LogError("シーン名からステージ番号を取得できませんでした: " + currentSceneName);
            return null;
        }

        // 次シーン名を特定
        int nextStageNumber = currentStageNumber + 1;
        string nextSceneName = "Stage" + nextStageNumber;

        // 次のシーンが存在するかチェックして読み込む
        if (Application.CanStreamedLevelBeLoaded(nextSceneName))
        {
            // 次シーンが存在する場合はシーン名を返す
            return nextSceneName;
        }

        // 次シーンが無い場合はnullを返す
        return null;
    }

    /// <summary>
    /// 次のシーンへの遷移処理
    /// </summary>
    public void LoadNextStage()
    {
        string nextSceneName = GetNextSceneName();

        if (nextSceneName != null)
        {
            SceneManager.LoadScene(nextSceneName);
        }

        // 次のシーンがない場合は何もしない
    }
}
