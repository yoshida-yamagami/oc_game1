using UnityEngine;

public class StageSelectDirector : MonoBehaviour
{
    /// <summary>
    /// ゲームシーンに遷移
    /// </summary>
    public void GoToGameScene()
    {
        Initiate.Fade("GameScene", Color.black, 1.0f);
    }
}
