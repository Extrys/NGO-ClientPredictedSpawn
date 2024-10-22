using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

// This factory is responsible for creating the Spawners
public class ClientPredictedSpawnerFactory : NetworkBehaviour
{
	readonly static List<ClientPredictedSpawner> spawners = new();

	// if youre using DependencyInjection then i recommend inject the factory as dependency
	static ClientPredictedSpawnerFactory instance;
	void Awake()
	{
		if (instance == null)
			instance = this;
	}

	public static ClientPredictedSpawner CreateSpawner(NetworkObject prefab)
	{
		if (!instance)
			throw new Exception("Trying to create a Client predicted spawner, but the factory is null or not ready");

		var spawner = new ClientPredictedSpawner(spawners.Count, prefab, instance.SpawnServerRPC);
		spawners.Add(spawner);
		return spawner;
	}

	public override void OnNetworkSpawn()
	{
		foreach (var spawner in spawners)
			spawner.OnNetworkSpawn();
	}

	public override void OnNetworkDespawn()
	{
		foreach (var spawner in spawners)
			spawner.OnNetworkDespawn();
	}

	[ServerRpc(RequireOwnership = false)] // in the future i might avoid having a ServerCall method inside spawner classes, by caching the delegate
	void SpawnServerRPC(int spawnerIndex, ulong clientId, Vector3 position, Quaternion orientation) => spawners[spawnerIndex].Spawn_ServerCall(clientId, position, orientation);
}
