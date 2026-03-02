using UnityEngine;
using DG.Tweening;
using System;

public class PipeCell : MonoBehaviour
{
    [Header("Settings")]
    public Direction initialConnections; // Cài đặt lỗ ban đầu trong Inspector (Dùng Enum Flag)
    public bool isSource;   // Là nguồn nước?
    public bool isEndPoint; // Là đích đến (Nhà dân)?
    public bool isFixed;

    [Header("Runtime State")]
    public Direction currentConnections; // Các lỗ hiện tại (sau khi xoay)
    public bool isWet;
    public int x { get; private set; }
    public int y { get; private set; }
    [Header("Grid Settings")]
    [SerializeField] private float cellSize = 1f;

    [SerializeField] private GameObject waterVisual; // Sprite nước/Hiệu ứng
    private bool _isRotating;

    // Sự kiện báo cho Manager biết
    public static event Action OnPipeRotated;

    private void Awake()
    {
        // x = Mathf.RoundToInt(transform.position.x);
        // y = Mathf.RoundToInt(transform.position.z);
        float size = cellSize > 0 ? cellSize : 1f;
        x = Mathf.RoundToInt(transform.position.x / size);
        y = Mathf.RoundToInt(transform.position.z / size);

        // 2. Gán hướng ban đầu
        currentConnections = initialConnections;

        // 3. Nếu là nguồn -> Luôn có nước
        if (isSource)
        {
            isWet = true;
        }

        UpdateVisual();
    }

    private void OnMouseDown()
    {
        if (isFixed || _isRotating) return;
        RotatePipe();
    }

    private void RotatePipe()
    {
        _isRotating = true;

        // --- BƯỚC 1: TÍNH TOÁN LOGIC MỚI ---
        // Tạo biến hướng mới
        Direction newDir = Direction.None;

        // Duyệt qua 4 hướng, nếu hướng cũ có thì xoay nó sang phải 90 độ
        if (currentConnections.HasFlag(Direction.Up)) newDir |= Direction.Right;
        if (currentConnections.HasFlag(Direction.Right)) newDir |= Direction.Down;
        if (currentConnections.HasFlag(Direction.Down)) newDir |= Direction.Left;
        if (currentConnections.HasFlag(Direction.Left)) newDir |= Direction.Up;

        currentConnections = newDir;

        // --- BƯỚC 2: XOAY HÌNH ẢNH (VISUAL) ---
        transform.DORotate(new Vector3(0, 90, 0), 0.2f, RotateMode.LocalAxisAdd)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                _isRotating = false;
                // Xoay xong mới báo Manager tính lại nước (để tránh lỗi logic khi đang xoay dở)
                OnPipeRotated?.Invoke();
            });
    }

    public void SetWaterState(bool state)
    {
        isWet = state;
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (waterVisual) waterVisual.SetActive(isWet);
    }

    // --- ĐOẠN CODE NÀY CHỈ ĐỂ DEBUG (XEM TỌA ĐỘ) ---
    // --- ĐOẠN CODE NÀY CHỈ ĐỂ DEBUG (XEM TỌA ĐỘ) ---
    // --- ĐOẠN CODE NÀY CHỈ ĐỂ DEBUG (XEM TỌA ĐỘ) ---
    private void OnDrawGizmos()
    {
        // 1. Lấy size
        float size = cellSize > 0 ? cellSize : 1f;

        // 2. Tính Index
        int debugX = Mathf.RoundToInt(transform.position.x / size);
        int debugY = Mathf.RoundToInt(transform.position.z / size);

        // 3. Tính vị trí vẽ (Tâm hộp)
        // QUAN TRỌNG: Vẽ tại vị trí của OBJECT (transform.position) 
        // thay vì tính toán lại, để đảm bảo nó luôn bao quanh vật thể.
        Vector3 center = transform.position;

        // Hoặc nếu muốn vẽ theo Grid Snapping (để xem vật thể có lệch không):
        // Vector3 center = new Vector3(debugX * size, 0, debugY * size);

        Gizmos.color = Color.green;

        // Vẽ cục tròn màu vàng để đánh dấu tâm (Dễ nhìn hơn hộp)
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(center, 0.1f * size);

        // Vẽ hộp xanh
        Gizmos.color = Color.green;
        Vector3 boxSize = new Vector3(size * 0.9f, 0.5f, size * 0.9f);
        Gizmos.DrawWireCube(center, boxSize);

#if UNITY_EDITOR
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.white;
        style.fontSize = 20;
        // Vẽ chữ tọa độ
        UnityEditor.Handles.Label(center + Vector3.up * 0.5f, $"({debugX}, {debugY})", style);
#endif
    }
}