using Fusion;
using UnityEngine;

namespace Fusion105
{
	public class Player : NetworkBehaviour
	{
		[SerializeField] private Ball _prefabBall;
		[SerializeField] private PhysxBall _prefabPhysxBall;

		[Networked] private TickTimer delay { get; set; }

		private NetworkCharacterControllerPrototype _cc;
		private Vector3 _forward;

		private void Awake()
		{
			_cc = GetComponent<NetworkCharacterControllerPrototype>();
			_forward = transform.forward;
		}

		public override void FixedUpdateNetwork()
		{
			if (GetInput(out NetworkInputData data))
			{
				data.direction.Normalize();
				_cc.Move(5*data.direction*Runner.DeltaTime);

				if (data.direction.sqrMagnitude > 0)
					_forward = data.direction;

				if (delay.ExpiredOrNotRunning(Runner))
				{
					if ((data.buttons & NetworkInputData.MOUSEBUTTON1) != 0)
					{
						delay = TickTimer.CreateFromSeconds(Runner, 0.5f);
						Runner.Spawn(_prefabBall, transform.position+_forward, Quaternion.LookRotation(_forward), Object.InputAuthority, (runner, o) =>
						{
							o.GetComponent<Ball>().Init();
						});
						spawned = !spawned;
					}
					else if ((data.buttons & NetworkInputData.MOUSEBUTTON2) != 0)
					{
						delay = TickTimer.CreateFromSeconds(Runner, 0.5f);
						Runner.Spawn(_prefabPhysxBall, transform.position+_forward, Quaternion.LookRotation(_forward), Object.InputAuthority, (runner, o) =>
						{
							o.GetComponent<PhysxBall>().Init( 10*_forward );
						});
						spawned = !spawned;
					}
				}
			}
		}
		
		/// <summary>
		/// 서버에 전송 가능한 Bool 변수형
		/// 시스템마다 변수형들의 정의 및 사용이 다르기 때문에 이를 통일시켜줄 네트워크 변수형 선언
		/// [Networked]애트리뷰트를 사용해 네트워크에 동기화를 하겠다고 선언
		/// (OnChanged)함수를 통해 현재 화면 프레임에서 해당 변수값이 변경되었을 때 수행할 함수 선택 가능
		/// </summary>
		[Networked(OnChanged = nameof(OnBallSpawned))]
		public NetworkBool spawned { get; set; }

		public static void OnBallSpawned(Changed<Player> changed)
		{
			changed.Behaviour.material.color = Color.white;
		}

		private Material _material;
		Material material 
		{
			get
			{
				if(_material==null)
					_material = GetComponentInChildren<MeshRenderer>().material;
				return _material;
			}
		}

		/// <summary>
		/// 각 클라이언트에서 오브젝트들이 프레임 단위로 수행하게 되는 함수
		/// 유니티의 LateUpdate와 비슷하게 쓰인다.
		/// </summary>
		public override void Render()
		{
			material.color = Color.Lerp(material.color, Color.blue, Time.deltaTime );
		}
	}
}
