using UnityEngine;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    [SerializeField] GameObject gameClearText;
    [SerializeField] GameObject gameOverText;
    [SerializeField] GameObject restartButton;

    private bool isClear = false;

    void Start()
    {
        // テキストやボタンを隠す
        gameClearText.SetActive(false);
        gameOverText.SetActive(false);
        restartButton.SetActive(false);
    }

    void Update()
    {
        
    }

    public void GameOver()
    {
        // ゲームオーバーの文字表示
        gameOverText.SetActive(true);
        restartButton.SetActive(true);
    }

    public void GameClear()
    {
        // ゲームクリアの文字表示
        gameClearText.SetActive(true);
        restartButton.SetActive(true);
    }

    public void Restart()
    {
        // 同じステージを最初から始める
        Initiate.Fade("GameScene", Color.black, 1.0f);
    }
}
