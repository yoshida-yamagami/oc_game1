using UnityEngine;

public class TitleDirector : MonoBehaviour
{
    /// <summary>
    /// ステージセレクトシーンに遷移
    /// </summary>
    public void GoToStageSelectScene()
    {
        Initiate.Fade("StageSelectScene", Color.black, 1.0f);
    }
}
