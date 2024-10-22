using Unity.Netcode;
using UnityEngine;

//Simply add this script to your Spawned prefab, to change its color for owner debugging
public class TestBlockPredSpawn : NetworkBehaviour
{
	public override void OnNetworkSpawn()
	{
		Debug.Log("OnNetworkSpawn KUBE");
		GetComponent<Renderer>().material.color = IsOwner ? Color.green : Color.red;
	}
}
