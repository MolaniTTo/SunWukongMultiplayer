using Mirror;
using UnityEngine;
using System.Collections.Generic;

public class CustomNetworkManager : NetworkManager
{
    [Header("Prefabs de Jugadores")]
    public GameObject hostPlayerPrefab;
    public GameObject clientPlayerPrefab;

    [Header("Puntos de Spawn")]
    public List<Transform> startPositionsSpawn = new List<Transform>();

    public override void OnStartServer()
    {
        base.OnStartServer();
        Debug.Log("=== SERVIDOR INICIADO ===");
    }

    public override void OnStartHost()
    {
        base.OnStartHost();
        Debug.Log("=== HOST INICIADO ===");
    }

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);
        Debug.Log($"Servidor: Cliente conectado con ID {conn.connectionId}");
    }

    // Cuando el cliente esté listo, crear su jugador
    public override void OnServerReady(NetworkConnectionToClient conn)
    {
        base.OnServerReady(conn);
        Debug.Log($"Servidor: Cliente {conn.connectionId} está listo");

        // Crear el jugador manualmente
        SpawnPlayer(conn);
    }

    void SpawnPlayer(NetworkConnectionToClient conn)
    {
        Debug.Log($"=== SpawnPlayer llamado para conexión ID: {conn.connectionId} ===");

        // Verificar que tenemos los prefabs asignados
        if (hostPlayerPrefab == null)
        {
            Debug.LogError("ERROR: hostPlayerPrefab no está asignado!");
            return;
        }
        if (clientPlayerPrefab == null)
        {
            Debug.LogError("ERROR: clientPlayerPrefab no está asignado!");
            return;
        }

        // Determinar qué prefab usar
        GameObject playerPrefab = (conn.connectionId == 0) ? hostPlayerPrefab : clientPlayerPrefab;

        Debug.Log($"→ Usando prefab: {playerPrefab.name}");

        // Obtener posición de spawn
        Vector3 spawnPosition = GetStartPosition().position;
        Debug.Log($"→ Posición de spawn: {spawnPosition}");

        // Crear el jugador
        GameObject player = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
        player.name = $"{playerPrefab.name}_Conn{conn.connectionId}";
        Debug.Log($"→ Jugador instanciado: {player.name}");

        // Spawnearlo en la red
        NetworkServer.AddPlayerForConnection(conn, player);
        Debug.Log($"✓ Jugador añadido exitosamente. Total jugadores: {numPlayers}");
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);
        Debug.Log($"Servidor: Cliente desconectado con ID {conn.connectionId}");
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        Debug.Log("Cliente: Conectado al servidor");
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        Debug.Log("Cliente: Desconectado del servidor");
    }

    Transform GetStartPosition()
    {
        // Primero intenta usar tu lista personalizada
        if (startPositionsSpawn.Count > 0)
        {
            int index = numPlayers % startPositionsSpawn.Count;
            Debug.Log($"Usando spawn point personalizado {index}");
            return startPositionsSpawn[index];
        }

        // Luego la lista base
        if (startPositions.Count > 0)
        {
            int index = numPlayers % startPositions.Count;
            Debug.Log($"Usando spawn point base {index}");
            return startPositions[index];
        }

        // Por defecto
        Debug.LogWarning("No hay spawn points, usando (0,0,0)");
        return transform;
    }
}