using Fusion;
using UnityEngine;

namespace Fusion102
{
	/// <summary>
	/// 입력 데이터를 정의하는 구조체
	/// INetworkInput을 상속받아야 한다
	/// </summary>
	public struct NetworkInputData : INetworkInput
	{
		public Vector3 direction;
	}
}
