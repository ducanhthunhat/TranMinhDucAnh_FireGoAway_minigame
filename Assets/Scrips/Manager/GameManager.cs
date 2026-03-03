using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : Singleton<GameManager>
{
    [Header("Settings")]
    [SerializeField] private int maxMove = 10;

    [Header("UI")]
    public TextMeshProUGUI moveText;

    private int _currentMove;

    public bool CanMove => _currentMove > 0;
    private bool _isLevelWon = false;
    public GridManager GridManager { get; private set; }

    private void Start()
    {
        _currentMove = maxMove;
        UpdateUI();
    }

    private void OnEnable()
    {
        PipeCell.OnPipeRotated += OnPipeRotatedHandler;
        GridManager.OnLevelWin += HandleLevelWin;
    }

    private void OnDisable()
    {
        PipeCell.OnPipeRotated -= OnPipeRotatedHandler;
        GridManager.OnLevelWin -= HandleLevelWin;
    }
    private void HandleLevelWin()
    {
        _isLevelWon = true;
    }

    private void OnPipeRotatedHandler()
    {
        if (_currentMove > 0)
        {
            _currentMove--;
            UpdateUI();

            if (_currentMove <= 0 && _isLevelWon == false)
            {
                UIManager.Instance.OpenUI<UIGameOver>();
                UIManager.Instance.PauseGame();

            }
        }
    }

    private void UpdateUI()
    {
        if (moveText)
        {
            moveText.text = $"Moves: {_currentMove}";
            moveText.color = _currentMove <= 3 ? Color.red : Color.white;
        }
    }
}