using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private int score = 0;
    private float currentTime = 60;

    private GameObject _last = null;

    [SerializeField] GameObject menu;
    [SerializeField] GameObject options;
    [SerializeField] GameObject top;
    [SerializeField] GameObject game;
    [SerializeField] GameObject result;

    [Space(10)]
    [SerializeField] Text scoreText;
    [SerializeField] Text finalScoreText;

    [Space(10)]
    [SerializeField] Text timerText;

    private void Awake()
    {
        OpenWindow(0);

        Ball.OnPressed += () =>
        {
            UpdateScore();
        };
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && game.activeSelf)
        {
            OpenWindow(0);
        }

        if(game.activeSelf)
        {
            currentTime -= Time.deltaTime;
            if(currentTime <=0)
            {
                currentTime = 0;
                timerText.text = string.Format("{0:00}:{1:00}", 0, 0);
                return;
            }

            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);

            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    private void UpdateScore()
    {
        score += Random.Range(2, 6);
        scoreText.text = $"{score}";
    }

    public void OpenWindow(int windowIndex)
    {
        if(_last)
        {
            _last.SetActive(false);
        }

        switch(windowIndex)
        {
            case 0: _last = menu; break;
            case 1: _last = options; break;
            case 2: _last = top; break;
            case 3: _last = game; break;
            case 4: _last = result; break;
        }

        _last.SetActive(true);
        if(windowIndex == 3)
        {
            currentTime = 60;
            GameManager.Instance.StartGame();
        }
    }
}