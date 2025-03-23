using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using TMPro;

public class Spawnmanager : MonoBehaviour
{
    public static Spawnmanager Instance;

    [SerializeField] private Canvas canvas;

    [SerializeField] private float borderLimit;

    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private GameObject currentButton;

    [SerializeField] private float gameTime;
    public static event Action<float> OnChangeGameTime;

    [SerializeField] private float buttonSpawnTime;

    [SerializeField] private bool isGameRunning = false;

    private Coroutine spawnerRoutine;
    private Coroutine buttonRoutine;

    public float ButtonSpawnTime
    {
        get { return buttonSpawnTime; }
        set { buttonSpawnTime = value; }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public void StartSpawning()
    {
        spawnerRoutine = StartCoroutine(StartGameCount());
    }

    private Vector2 GetRandomizedSpawnPoint()
    {
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();

        float halfWidth = canvasRect.rect.width / 2f;
        float halfHeight = canvasRect.rect.height / 2f;

        float minX = -halfWidth + borderLimit;
        float maxX = halfWidth - borderLimit;
        float minY = -halfHeight + borderLimit;
        float maxY = halfHeight - borderLimit;

        float randomX = UnityEngine.Random.Range(minX, maxX);
        float randomY = UnityEngine.Random.Range(minY, maxY);

        return new Vector2(randomX, randomY);
    }

    public void ResetSpawner()
    {
        if(currentButton != null) {
            Destroy(currentButton);
        }

        if(spawnerRoutine != null)
        {
            StopCoroutine(spawnerRoutine);
        }

        if(buttonRoutine != null)
        {
            StopCoroutine(buttonRoutine);
        }

        if (isGameRunning)
        {
            isGameRunning = false;
        }
    }

    private IEnumerator StartGameCount()
    {
        isGameRunning = true;
        float currentGameTime = 0f;

        buttonRoutine = StartCoroutine(ButtonSpawnTimer());

        while (currentGameTime < gameTime)
        {
            currentGameTime += Time.deltaTime;
            OnChangeGameTime.Invoke(currentGameTime);
            yield return null;
        }
        isGameRunning = false;

        if (Gamemanager.Instance.EasyDone && !Gamemanager.Instance.HardDone)
        {
            UIManager.Instance.OpenTestModalUI("Thank you!",
                "Please note your Heartrate on the paper you recieved prior to the test @Test A.",
                "The upcoming game follows the same rules: Click buttons and score points.",
                "Your goal: 60 points.", () => Gamemanager.Instance.StartHardGame());
        } else if (!Gamemanager.Instance.EasyDone && Gamemanager.Instance.HardDone)
        {
            UIManager.Instance.OpenTestModalUI("Thank you!",
                "Please note your Heartrate on the paper you recieved prior to the test @Test B.",
                "The upcoming game follows the same rules: Click buttons and score points.",
                "Your goal: 20 points.", () => Gamemanager.Instance.StartEasyGame());
        }
        else
        {
            UIManager.Instance.OpenTestModalUI("Thank you!",
                "Please note your Heartrate on the paper you recieved prior to the test for the missing test (either A or B).",
                "You've finished the pre-test!",
                "I will explain you in a second how we'll proceed", () => {});
        }
    }

    private IEnumerator ButtonSpawnTimer()
    {
        while (isGameRunning)
        {
            GameObject newButton = Instantiate(buttonPrefab);
            RectTransform newRect = newButton.GetComponent<RectTransform>();

            newButton.transform.SetParent(canvas.transform, false);
            newRect.anchoredPosition = GetRandomizedSpawnPoint();

            currentButton = newButton;

            yield return new WaitForSeconds(buttonSpawnTime);

            if (currentButton != null)
            {
                if (Gamemanager.Instance.CurrentGameState == GameState.Hard)
                {
                    Soundmanager.Instance.PlayError();
                    newButton.GetComponent<Image>().color = Color.red;
                    newButton.GetComponent<Button>().interactable = false;
                    newButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
                    newButton.GetComponentInChildren<TextMeshProUGUI>().text = "TO LATE!";
                    yield return new WaitForSeconds(0.2f);
                }
                Destroy(newButton);
            }
        }
    }
}
