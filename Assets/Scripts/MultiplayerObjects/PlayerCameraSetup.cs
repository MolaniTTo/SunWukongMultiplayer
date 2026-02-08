using Unity.Cinemachine;
using Mirror;
using UnityEngine;

public class PlayerCameraSetup : NetworkBehaviour
{
    [SerializeField] private CinemachineCamera virtualCamera;

    public override void OnStartLocalPlayer()
    {
        // Solo activa la cámara para el jugador local
        if (virtualCamera != null)
        {
            virtualCamera.Priority = 10; // Prioridad alta para la cámara local
        }
    }

    void Start()
    {
        // Si no es el jugador local, desactiva o baja la prioridad
        if (!isLocalPlayer && virtualCamera != null)
        {
            virtualCamera.Priority = 0; // Prioridad baja
            // O directamente: virtualCamera.enabled = false;
        }
    }
}