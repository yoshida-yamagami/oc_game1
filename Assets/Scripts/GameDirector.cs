using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    [SerializeField] GameObject gameClearText;
    [SerializeField] GameObject gameOverText;
    [SerializeField] GameObject restartButton;

    private bool isClear = false;
    private int resetCount = -1; // 非稼働時は-1。数値がセットされるとカウントダウン、0になるとシーンリセット実行

    void Start()
    {
        // テキストやボタンを隠す
        gameClearText.SetActive(false);
        gameOverText.SetActive(false);
        restartButton.SetActive(false);
    }

    void Update()
    {
        if (resetCount > 0)
        {
            resetCount--;
            if (resetCount <= 0)
            {
                // ここでシーンリセット
                //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                Restart();
            }
        }
    }

    public void GameOver()
    {
        // ゲームオーバーの文字表示
        gameOverText.SetActive(true);
        restartButton.SetActive(true);

        // ゲームリセットまでのカウンターセット
        resetCount = 180; // 1秒60想定。3秒でゲームリセット
    }

    public void GameClear()
    {
        // ゲームクリアの文字表示
        gameClearText.SetActive(true);
        restartButton.SetActive(true);

        // ゲームリセットまでのカウンターセット
        resetCount = 180; // 1秒60想定。3秒でゲームリセット
    }

    public void Restart()
    {
        // 同じステージを最初から始める
        Initiate.Fade(SceneManager.GetActiveScene().name, Color.black, 1.0f);
    }
}
