
using Fusion;
using UnityEngine;

namespace Fusion114
{
    public class PlayerMovement : NetworkBehaviour
    {
        private CharacterController _controller;
        public Camera cam;

        public float playerSpeed = 2f;

        public float jumpForce = 5f;
        public float gravityValue = -9.81f;

        private Vector3 _velocity;
        private bool _jumpPressed;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
        }

        private void Update()
        {
            if (Input.GetButtonDown("Jump"))
            {
                _jumpPressed = true;
            }
        }
        
        
        /// <summary>
        /// Awake/Start 대신 사용되는 함수, Awake에서는 NetworkObject가 준비되었다는 보장이 없음
        /// </summary>
        public override void Spawned()
        {
            if (HasStateAuthority)
            {
                cam = Camera.main;
                cam.GetComponent<FirstPersonCamera>().target = GetComponent<NetworkTransform>().InterpolationTarget;
            }
        }
        
        
        /// <summary>
        /// Update/FixedUpdate 대신 사용되는 함수.
        /// 모든 클라이언트에서 오브젝트가 부드럽게 보간되어 움직임을 보장한다.
        /// </summary>
        public override void FixedUpdateNetwork()
        {
            // 클라이언트가 오브젝트를 컨트롤하고 있는지 확인
            if (HasStateAuthority == false)
            {
                
            }
            
            // 오브젝트가 땅 위에 있으면 가속도 초기화
            if (_controller.isGrounded)
            {
                _velocity = new Vector3(0, -1, 0);
            }
            
            // 네트워크상의 버튼 입력 처리 방식은 3가지가 있다.
            // 1.버튼 입력을 Update함수에서 받아 bool 형식으로 저장하고 사용한 후 이를 FixedUpdateNetwork에서 초기화해준다
            // 2.퓨전의 NetworkInput과 NetworkButtons시스템을 사용한다.
            // 3.유니티 인풋 시스템을 사용해 Update Mode를 Manual Update로 변경해준 후 FixedUpdateNetwork함수에서 InputSystem.Update를 호출한다.
            // 위의 방식은 Shared mode에서 전부 사용 가능하다. server/host 모드에서는 2번 방법만 사용이 가능하다.
            
            // NetworkTransform이 StateAuthority를 갖고 있는 클라이언트의 물체를 다른 모든 클라이언트에 동기화시켜준다
            // FixedUpdateNetwork() 함수 안에서는 Runner.DeltaTime을 사용해야 한다.
            var cameraRotationY = Quaternion.Euler(0, cam.transform.rotation.eulerAngles.y, 0);
            Vector3 move = cameraRotationY * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * Runner.DeltaTime * playerSpeed;

            // 점프 가속 추가
            _velocity.y += gravityValue * Runner.DeltaTime;
            if (_jumpPressed && _controller.isGrounded)
            {
                _velocity.y += jumpForce;
            }
            
            // 최종 움직임 추가
            _controller.Move(move + _velocity * Runner.DeltaTime);

            if (move != Vector3.zero)
            {
                gameObject.transform.forward = move;
            }
        }
    }
}

