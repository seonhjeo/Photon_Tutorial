using Fusion;

namespace Fusion103
{
	/// <summary>
	/// Ball에 부착된 컴포넌트
	/// 1.Network Transform : 해당 오브젝트의 트랜스폼을 네트워크에 동기화시키기 위한 컴포넌트
	/// 2.Network Object : 네트워크상에서 해당 오브젝트를 구분하기 위한 ID 제공 컴포넌트
	/// </summary>
	public class Ball : NetworkBehaviour
	{
		/// <summary>
		/// 서버에 타이머 시간을 동기화시키기 위한 TickTimer 구조체
		/// </summary>
		[Networked] private TickTimer life { get; set; }
		
		/// <summary>
		/// 공이 만들어질 때 실행되는 함수.
		/// </summary>
		public void Init()
		{
			// 틱을 Awake()함수에서 초기화할 시 서버와 동기화가 이루어지지 않는다. 이 점 유의
			life = TickTimer.CreateFromSeconds(Runner, 5.0f);
		}
		
		/// <summary>
		/// 다른 클라이언트에서 생성한 오브젝트의 움직임을 예측하기 위해 한 번씩 실행되게 됨
		/// </summary>
		public override void FixedUpdateNetwork()
		{
			if(life.Expired(Runner))
				Runner.Despawn(Object);
			else
				transform.position += 5 * transform.forward * Runner.DeltaTime;
		}
	}
}
