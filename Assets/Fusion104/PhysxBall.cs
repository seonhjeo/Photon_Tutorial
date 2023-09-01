using Fusion;
using UnityEngine;

namespace Fusion104
{
	/// <summary>
	/// 물리 엔진을 사용하고 싶은 오브젝트에 일반 RigidBody컴포넌트와 NetworkRigidBody컴포넌트 장착.
	/// NetworkRigidBody는 성능 이슈로 어셈블리로 작성되었음.
	/// 하지만 그래도 성능을 많이 요구하기 때문에 되도록 지양.
	/// </summary>
	public class PhysxBall : NetworkBehaviour
	{
		[Networked] private TickTimer life { get; set; }

		public void Init(Vector3 forward)
		{
			life = TickTimer.CreateFromSeconds(Runner, 5.0f);
			GetComponent<Rigidbody>().velocity = forward;
		}

		public override void FixedUpdateNetwork()
		{
			if(life.Expired(Runner))
				Runner.Despawn(Object);
		}
	}
}
