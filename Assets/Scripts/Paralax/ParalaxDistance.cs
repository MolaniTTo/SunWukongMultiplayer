
using Unity.Cinemachine;
using UnityEngine;

public class ParallaxStatic : MonoBehaviour
{
    [Header("Configuración de Cámara")]
    [Tooltip("Deja en null para usar Main Camera automáticamente")]
    public Transform cameraTarget;
    public bool useMainCamera = true; // ← ACTIVA ESTO

    [Header("Parallax Settings")]
    [Tooltip("1 = igual que el suelo, >1 más lejos, <1 más cerca")]
    public float distance = 2f;

    [Header("Anti-Jitter")]
    public bool useSmoothDamp = true;
    [Range(0.01f, 0.5f)]
    public float smoothTime = 0.1f;

    private Vector3 startPos;
    private float startCamX;
    private float velocityX;
    public bool stopPararallax = false;

    [System.Obsolete]
    void Start()
    {
        // Usa Main Camera para evitar jitter
        if (useMainCamera || cameraTarget == null)
        {
            cameraTarget = Camera.main.transform;
            Debug.Log($"Parallax usando Main Camera");
        }
        else
        {
            FindLocalPlayerCamera();
        }

        if (cameraTarget != null)
        {
            startPos = transform.position;
            startCamX = cameraTarget.position.x;
        }
    }

    [System.Obsolete]
    void FindLocalPlayerCamera()
    {
        CinemachineCamera[] vcams = FindObjectsOfType<CinemachineCamera>();
        CinemachineCamera activeVCam = null;
        int highestPriority = -1;

        foreach (var vcam in vcams)
        {
            if (vcam.Priority > highestPriority)
            {
                highestPriority = vcam.Priority;
                activeVCam = vcam;
            }
        }

        if (activeVCam != null && activeVCam.Follow != null)
        {
            cameraTarget = activeVCam.Follow;
        }
        else
        {
            cameraTarget = Camera.main.transform;
        }
    }

    void LateUpdate()
    {
        if (stopPararallax || cameraTarget == null) return;

        float camDeltaX = cameraTarget.position.x - startCamX;
        float targetOffsetX = camDeltaX * (1f - (1f / distance));

        float currentOffsetX = transform.position.x - startPos.x;

        // Suavizado para evitar jitter
        float smoothedOffsetX;
        if (useSmoothDamp)
        {
            smoothedOffsetX = Mathf.SmoothDamp(currentOffsetX, targetOffsetX, ref velocityX, smoothTime);
        }
        else
        {
            smoothedOffsetX = targetOffsetX;
        }

        transform.position = new Vector3(
            startPos.x + smoothedOffsetX,
            startPos.y,
            startPos.z
        );
    }
}