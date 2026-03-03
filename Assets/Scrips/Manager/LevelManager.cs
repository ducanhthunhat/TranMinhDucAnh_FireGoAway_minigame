using UnityEngine;
using System;
using System.Collections.Generic;
using DG.Tweening; //

public class LevelManager : MonoBehaviour
{
    // --- SINGLETON PATTERN ---
    public static LevelManager Instance { get; private set; }

    [Header("Data")]
    [SerializeField] public List<GameObject> levelPrefabs; // Danh sách các Level Prefab
    [SerializeField] public Transform levelContainer;      // Parent để chứa Level

    [Header("Debug")]
    [SerializeField] private bool loadFirstLevelOnStart = true;

    // Biến lưu trạng thái
    private GameObject _currentLevelObj;
    private int _currentLevelIndex = 0;

    // --- OBSERVER PATTERN (Events) ---
    public static event Action<int> OnLevelLoaded;
    public static event Action OnAllLevelsCompleted;

    private void Awake()
    {
        // Khởi tạo Singleton chuẩn
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (loadFirstLevelOnStart)
        {
            LoadLevel(_currentLevelIndex);
        }
    }

    /// <summary>
    /// Hàm cốt lõi để spawn level.
    /// </summary>
    public void LoadLevel(int index)
    {
        // 1. Dọn dẹp level cũ (Tránh Memory Leak & Lỗi DOTween)
        if (_currentLevelObj != null)
        {
            // --- BƯỚC QUAN TRỌNG: GIẾT TWEEN TRƯỚC KHI GIẾT OBJECT ---
            // Dừng mọi chuyển động đang chạy trên Object level cũ
            DOTween.KillAll();

            // Sau đó mới Destroy
            Destroy(_currentLevelObj);
        }

        // 2. Validate Index (Kiểm tra xem có level đó không)
        if (index >= 0 && index < levelPrefabs.Count)
        {
            // Cập nhật index hiện tại
            _currentLevelIndex = index;

            GameObject prefab = levelPrefabs[index];

            // 3. Spawn Level mới
            // --- SỬA LẠI: Lấy vị trí & góc xoay TỪ PREFAB (thay vì Vector3.zero) ---
            _currentLevelObj = Instantiate(prefab, prefab.transform.position, prefab.transform.rotation, levelContainer);

            // 4. Bắn sự kiện cho các bên liên quan biết
            OnLevelLoaded?.Invoke(_currentLevelIndex);

            Debug.Log($"<color=green>Đã load Level {_currentLevelIndex + 1}</color>");
        }
        else if (index >= levelPrefabs.Count)
        {
            // Trường hợp đã hết level -> Phá đảo
            Debug.Log("<color=yellow>Đã hết Level! Win Game!</color>");
            OnAllLevelsCompleted?.Invoke();
        }
        else
        {
            Debug.LogError($"Level Index {index} không hợp lệ!");
        }
    }

    /// <summary>
    /// Gọi hàm này khi thắng level (nút Next)
    /// </summary>
    public void LoadNextLevel()
    {
        LoadLevel(_currentLevelIndex + 1);
    }

    /// <summary>
    /// Gọi hàm này khi chơi lại (nút Replay)
    /// </summary>
    public void RestartCurrentLevel()
    {
        LoadLevel(_currentLevelIndex);
    }

    public int GetTotalLevels()
    {
        return levelPrefabs.Count;
    }
}