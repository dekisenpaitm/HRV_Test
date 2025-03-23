using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject startUI;

    [SerializeField] private GameObject testModalUI;

    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI info1Text;
    [SerializeField] private TextMeshProUGUI info2Text;
    [SerializeField] private TextMeshProUGUI info3Text;
    [SerializeField] private Button button;

    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private TextMeshProUGUI timeText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    private void OnEnable()
    {
        Gamemanager.OnChangeScore += UpdateScore;
        Spawnmanager.OnChangeGameTime += UpdateTime;
    }

    private void OnDisable()
    {
        Gamemanager.OnChangeScore -= UpdateScore;
        Spawnmanager.OnChangeGameTime -= UpdateTime;
    }

    public void OpenTestModalUI(string titel = "", string info1 = "", string info2 = "", string info3 = "", Action onClick = null)
    {
        button.onClick.RemoveAllListeners();
        Gamemanager.Instance.SwitchState(GameState.Paused);

        titleText.text = titel;
        info1Text.text = info1;
        info2Text.text = info2;
        info3Text.text = info3;
        button.onClick.AddListener(() => onClick());

        testModalUI.SetActive(true);
        Spawnmanager.Instance.ResetSpawner();
    }

    public void CloseStartUI()
    {
        startUI.SetActive(false);
    }

    public void CloseTestModalUI()
    {
        testModalUI.SetActive(false);
    }

    public void UpdateScore(int amount)
    {
        scoreText.text = "Score: " + amount.ToString();
    }

    public void UpdateTime(float amount)
    {
        timeText.text = "Time: " + amount.ToString("F1");
    }
}
