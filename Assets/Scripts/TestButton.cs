using UnityEngine;

public class TestButton : MonoBehaviour
{
    public void OnClick()
    {
        Gamemanager.Instance.AddToScore(1);
        Destroy(gameObject);
    }
}
