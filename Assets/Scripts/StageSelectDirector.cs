using UnityEngine;

public class StageSelectDirector : MonoBehaviour
{
    /// <summary>
    /// ƒQ[ƒ€ƒV[ƒ“‚É‘JˆÚ
    /// </summary>
    public void GoToGameScene()
    {
        Initiate.Fade("GameScene", Color.black, 1.0f);
    }
}
