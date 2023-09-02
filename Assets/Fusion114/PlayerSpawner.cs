
using Fusion;
using UnityEngine;

namespace Fusion114
{
    public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
    {
        public GameObject playerPrefab;

        /// <summary>
        /// 플레이어가 플레이어와 같은 게임 오브젝트에 있으면 세션에 참여할 때마다 호출되는 함수
        /// </summary>
        /// <param name="player">서버가 해당 플레이어를 구분하는데 사용하는 값</param>
        public void PlayerJoined(PlayerRef player)
        {
            if (player == Runner.LocalPlayer)
            {
                Runner.Spawn(playerPrefab, new Vector3(0, 1, 0), Quaternion.identity, player);
            }
        }
    }
}


