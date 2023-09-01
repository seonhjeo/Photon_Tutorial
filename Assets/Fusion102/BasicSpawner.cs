using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Fusion102
{
	// INetworkRunnerCallbacks : NetworkRunner가 해당 클래스를 조작하기 위해 필요한 콜백 함수를 제공하는 인터페이스
	public class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks 
	{
		private NetworkRunner _runner;

		private void OnGUI()
		{
			if (_runner == null)
			{
				if (GUI.Button(new Rect(0,0,200,40), "Host"))
				{
					StartGame(GameMode.Host);
				}
				if (GUI.Button(new Rect(0,40,200,40), "Join"))
				{
					StartGame(GameMode.Client);
				}
			}
		}

		async void StartGame(GameMode mode)
		{
			// Create the Fusion runner and let it know that we will be providing user input
			// NetworkRunner : 포톤의 가장 중요한 컴포넌트, 네트워크 시뮬레이션 등 중요한 기능 수행
			_runner = gameObject.AddComponent<NetworkRunner>();
			_runner.ProvideInput = true;
	    
			// Start or join (depends on gamemode) a session with a specific name
			await _runner.StartGame(new StartGameArgs()
			{
				GameMode = mode, 
				SessionName = "TestRoom", 
				Scene = SceneManager.GetActiveScene().buildIndex,
				SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
			});
		}

		[SerializeField] private NetworkPrefabRef _playerPrefab; // Character to spawn for a joining player
		private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();
		
		/// <summary>
		/// 플레이어가 방에 입장했을 때 수행할 함수  
		/// NetworkRunner.Spawn()함수에서 인게임 씬에 오브젝트를 소환함과 동시에 해당 오브젝트의 컨트롤 권한을 가진 플레이어를 등록
		/// </summary>
		/// <param name="runner">네트워크 러너 클래스</param>
		/// <param name="player">서버가 해당 플레이어를 구분하는데 사용하는 값</param>
		public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
		{
			if (runner.IsServer)
			{
				Vector3 spawnPosition = new Vector3((player.RawEncoded%runner.Config.Simulation.DefaultPlayers)*3,1,0);
				NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
				_spawnedCharacters.Add(player, networkPlayerObject);
			}
		}

		// 플레이어가 방을 떠날 때 수행할 함수
		public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
		{
			if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
			{
				runner.Despawn(networkObject);
				_spawnedCharacters.Remove(player);
			}
		}
		
		// 플레이어의 입력을 받는 함수
		public void OnInput(NetworkRunner runner, NetworkInput input)
		{
			var data = new NetworkInputData();

			// 입력값 설정
			if (Input.GetKey(KeyCode.W))
				data.direction += Vector3.forward;

			if (Input.GetKey(KeyCode.S))
				data.direction += Vector3.back;

			if (Input.GetKey(KeyCode.A))
				data.direction += Vector3.left;

			if (Input.GetKey(KeyCode.D))
				data.direction += Vector3.right;
			
			// 설정된 데이터를 서버에 전송
			input.Set(data);
		}
		
		public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
		public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
		public void OnConnectedToServer(NetworkRunner runner) { }
		public void OnDisconnectedFromServer(NetworkRunner runner) { }
		public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
		public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
		public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
		public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
		public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
		public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
		public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
		public void OnSceneLoadDone(NetworkRunner runner) { }
		public void OnSceneLoadStart(NetworkRunner runner) { }
	}
}
