
using Fusion;
using UnityEngine;

namespace Fusion115
{
    /// <summary>
    /// RPC 적용 클래스
    /// 타 클래스가 주 권한을 가진 네트워크 오브젝트의 속성을 변경하는 방법으로 RPC(원격 프로시저 호출)을 사용한다.
    /// 코드에 의해 네트워크 속성의 값을 변경할 수 있지만, 변경 사항은 로컬에서만 적용되고 네트워크를 통해 복제되지 않기 때문에 RPC를 사용한다.
    /// </summary>
    public class Health : NetworkBehaviour
    {
        [Networked(OnChanged = nameof(NetworkedHealthChanged))]
        public float NetworkedHealth { get; set; } = 100;

        /// <summary>
        /// 슈터가 적 플레이어에게 데미지를 주는 함수
        /// RpcSources.All을 이용해 아무나 RPC를 호출할 수 있다. 미사용시 InputAuthority(SharedMode에서는 StateAuthority와 동일)를 가진 네트워크만 RPC 호출 가능.
        /// RpcTargets.StateAuthority을 이용해 StateAuthority를 가진 네트워크만 RPC를 받을 수 있게 작성.
        /// RPC 기능 내부의 코드는 RpcTarget 클라이언트에서 실행되므로 이 경우 자체 네트워크 속성을 변경할 수 있는 StateAuthority에서 실행됨.
        /// </summary>
        /// <param name="damage"></param>
        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void DealDamageRpc(float damage)
        {
            // The code inside here will run on the client which owns this object (has state and input authority).
            Debug.Log("Received DealDamageRpc on StateAuthority, modifying Networked variable");
            NetworkedHealth -= damage;
        }
        
        private static void NetworkedHealthChanged(Changed<Health> changed)
        {
            // Here you would add code to update the player's healthbar.
            Debug.Log($"Health changed to: {changed.Behaviour.NetworkedHealth}");
        }
    }
}

