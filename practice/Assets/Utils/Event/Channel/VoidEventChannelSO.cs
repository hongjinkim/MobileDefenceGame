using UnityEngine.Events;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Void Event Channel")]
public class VoidEventChannelSO : EventChannelSO<Void>
{
    public void RaiseEvent()
	{
		RaiseEvent(new Void());
	}
}