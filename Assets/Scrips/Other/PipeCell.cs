using UnityEngine;
using DG.Tweening;
using System;

public class PipeCell : MonoBehaviour
{
    [Header("Settings")]
    public Direction initialConnections;
    public bool isSource;
    public bool isEndPoint;
    public bool isFixed;

    [Header("Runtime State")]
    public Direction currentConnections;
    public bool isWet;
    public int x { get; private set; }
    public int y { get; private set; }
    [Header("Grid Settings")]
    [SerializeField] private float cellSize = 1f;

    [SerializeField] private GameObject waterVisual;
    private bool _isRotating;

    public static event Action OnPipeRotated;

    private void Awake()
    {
        // x = Mathf.RoundToInt(transform.position.x);
        // y = Mathf.RoundToInt(transform.position.z);
        float size = cellSize > 0 ? cellSize : 1f;
        x = Mathf.RoundToInt(transform.position.x / size);
        y = Mathf.RoundToInt(transform.position.z / size);

        currentConnections = initialConnections;

        if (isSource)
        {
            isWet = true;
        }

        UpdateVisual();
    }

    private void OnMouseDown()
    {
        if (isFixed || _isRotating || !GameManager.Instance.CanMove)
        {
            return;
        }
        ;
        RotatePipe();
    }

    private void RotatePipe()
    {
        _isRotating = true;

        Direction newDir = Direction.None;

        if (currentConnections.HasFlag(Direction.Up)) newDir |= Direction.Right;
        if (currentConnections.HasFlag(Direction.Right)) newDir |= Direction.Down;
        if (currentConnections.HasFlag(Direction.Down)) newDir |= Direction.Left;
        if (currentConnections.HasFlag(Direction.Left)) newDir |= Direction.Up;

        currentConnections = newDir;

        transform.DORotate(new Vector3(0, 90, 0), 0.2f, RotateMode.LocalAxisAdd)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                _isRotating = false;

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

    private void OnDrawGizmos()
    {

        float size = cellSize > 0 ? cellSize : 1f;


        int debugX = Mathf.RoundToInt(transform.position.x / size);
        int debugY = Mathf.RoundToInt(transform.position.z / size);


        Vector3 center = transform.position;


        Gizmos.color = Color.green;


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
    private void OnDestroy()
    {
        transform.DOKill();
    }
}