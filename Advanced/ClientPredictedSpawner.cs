using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

// The spawner class, which is responsible for creating the prefab instance, and latespawning the object
public class ClientPredictedSpawner : INetworkPrefabInstanceHandler, IDisposable
{
	int spawnerId;
	Queue<NetworkObject> queuedIntances = new();
	NetworkObject prefab;
	event Action<int,ulong,Vector3,Quaternion> SpawnServerRPC;


	public ClientPredictedSpawner(int spawnerId, NetworkObject prefab, Action<int, ulong, Vector3, Quaternion> spawnServerRPC)
	{
		SpawnServerRPC = spawnServerRPC;
		this.spawnerId = spawnerId;
		this.prefab = prefab;
		NetworkManager.Singleton.PrefabHandler.AddHandler(prefab, this);
	}

	public void Spawn(Vector3 position, Quaternion orientation)
	{
		if (SpawnServerRPC == null)
			throw new Exception("This Spawner has not been registered, you need to register it before spawning");

		var ngo = GameObject.Instantiate(prefab, position, orientation);
		ngo.SetSceneObjectStatus(false);
		queuedIntances.Enqueue(ngo);
		SpawnServerRPC(spawnerId, NetworkManager.Singleton.LocalClientId, position, orientation);
	}

	// This function should only be called on the server side automatically
	public void Spawn_ServerCall(ulong clientId, Vector3 position, Quaternion orientation)
	{
		NetworkManager.Singleton.SpawnManager.InstantiateAndSpawn(prefab, clientId, false, false, false, position, orientation);
	}

	public void OnNetworkSpawn()
	{
		List<NetworkObject> offlineInstances = queuedIntances.ToList();
		foreach (var instance in offlineInstances)
			SpawnServerRPC(spawnerId, NetworkManager.Singleton.LocalClientId, instance.transform.position, instance.transform.rotation);
	}
	public void OnNetworkDespawn() => Dispose();
	public void Dispose()
	{
		NetworkManager.Singleton.PrefabHandler.RemoveHandler(prefab);
		queuedIntances.Clear();
		prefab = null;
	}

	NetworkObject INetworkPrefabInstanceHandler.Instantiate(ulong ownerClientId, Vector3 position, Quaternion rotation)
	{
		return NetworkManager.Singleton.LocalClientId == ownerClientId ? queuedIntances.Dequeue() : GameObject.Instantiate(prefab, position, rotation);
	}
	void INetworkPrefabInstanceHandler.Destroy(NetworkObject networkObject) => GameObject.Destroy(networkObject);
}