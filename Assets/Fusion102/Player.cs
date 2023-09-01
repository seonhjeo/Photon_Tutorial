using Fusion;

namespace Fusion102
{
	/// <summary>
	/// 플레이어에 부착된 컴포넌트
	/// 1.Network Object : 네트워크 상에서 해당 오브젝트를 구분하기 위한 ID 제공
	/// 2.Network Character Controller : NetworkTransform을 상속받아 캐릭터를 컨트롤하는 컨트롤러
	///   NetworkTransform은 해당 오브젝트의 트랜스폼을 네트워크상에서 동기화시켜줌
	///   해당 컴포넌트를 소유한 오브젝트의 자식에 메쉬를 추가한 후 Interpolation Target에 등록해 움직임 보간 작업 수행
	/// </summary>
	public class Player : NetworkBehaviour
	{
		private NetworkCharacterControllerPrototype _cc;

		private void Awake()
		{
			_cc = GetComponent<NetworkCharacterControllerPrototype>();
		}
		
		/// <summary>
		/// Photon Fusion에서 시뮬레이션을 Update하기 위해 사용하는 함수, NetworkBehaviour를 상속받은 클래스가 사용 가능
		/// 클라이언트에서도 독자적으로, 서버에서도 동기화를 위해 한 틱에 여러번 수행될 수 있다.
		/// </summary>
		public override void FixedUpdateNetwork()
		{
			// NetworkBehaviour컴포넌트가 부착되어 있기 때문에 GetInput으로 입력값을 가져올 수 있다.
			if (GetInput(out NetworkInputData data))
			{
				data.direction.Normalize();
				_cc.Move(5*data.direction*Runner.DeltaTime);
			}
		}
	}
}
