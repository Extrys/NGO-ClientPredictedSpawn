using Unity.Netcode;
using UnityEngine;

// example for using the Advanced method
public class MonoPredictedSpawnerAdvanced_Example : NetworkBehaviour
{
	public NetworkObject prefab;
	ClientPredictedSpawner spawner;

	private void Start()
	{
		// you need to create the client predicted spawner
		spawner = ClientPredictedSpawnerFactory.CreateSpawner(prefab);
	}

	private void Update()
	{
		// once created, you can use the spawner to spawn your prefab, it will network automatically
		if (UnityEngine.InputSystem.Mouse.current.backButton.wasPressedThisFrame)
			spawner.Spawn(Camera.main.transform.position + Camera.main.transform.forward * 3, transform.rotation);
	}
}
