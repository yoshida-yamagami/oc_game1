using UnityEngine;

public class Chaos : MonoBehaviour
{
    [SerializeField] GameObject gameOverText;

    // カオスに足を踏み入れたらゲームオーバー
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // ゲームオーバー処理
            gameOverText.SetActive(true); // ゲームオーバー時表示テキスト
        }
    }
}
