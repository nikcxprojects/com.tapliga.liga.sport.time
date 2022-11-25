using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private int score = 0;
    private GameObject _last = null;

    [SerializeField] GameObject menu;
    [SerializeField] GameObject options;
    [SerializeField] GameObject top;
    [SerializeField] GameObject game;
    [SerializeField] GameObject result;

    [Space(10)]
    [SerializeField] Text scoreText;
    [SerializeField] Text finalScoreText;

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
    }

    private void UpdateScore()
    {
        score += Random.Range(10, 25);
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
            GameManager.Instance.StartGame();
        }
    }
}