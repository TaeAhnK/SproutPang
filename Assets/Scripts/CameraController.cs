using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [SerializeField] private float minWidth = 10f;
    [SerializeField] private float minHeight = 6f;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();

        float sizeForHeight = minHeight * 0.5f;
        float sizeForWidth = minWidth / (2f * cam.aspect);

        cam.orthographicSize = Mathf.Max(
            sizeForHeight,
            sizeForWidth
        );
    }
}