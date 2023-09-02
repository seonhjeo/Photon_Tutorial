using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace Fusion114
{
    public class PlayerColor : NetworkBehaviour
    {
        public MeshRenderer meshRenderer;
        
        /// <summary>
        /// Networked : 네트워크를 통해 프로퍼티를 동기화시키기 위한 애트리뷰트. StateAuthority에서 다른 모든 클라이언트로 동기화시켜준다.
        /// StateAuthority가 아닌 프로퍼티가 변경되면 변경값은 동기화되지 않고 로컬 예측상황으로 간주되어 StateAuthority에 의해 오버라이딩 될 수 있다.
        /// 따라서 StateAuthority인 오브젝트에서만 Networked 프로퍼티를 변경하도록 주의하라.
        /// Networked 애트리뷰트는 프로퍼티에만 동작한다.
        /// </summary>
        [Networked(OnChanged = nameof(NetworkColorChanged))]
        public Color NetworkedColor { get; set; }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                // Changing the material color here directly does not work since this code is only executed on the client pressing the button and not on every client.
                NetworkedColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
            }
        }
        
        /// <summary>
        /// Networked 애트리뷰트의 OnChanged에 이벤트를 후킹하기 위한 함수.
        /// Changed PlayerColor은 NetworkBehaviour에서 제공하는 타입이다.
        /// </summary>
        /// <param name="changed"></param>
        private static void NetworkColorChanged(Changed<PlayerColor> changed)
        {
            changed.Behaviour.meshRenderer.material.color = changed.Behaviour.NetworkedColor;
        }
    }
}

