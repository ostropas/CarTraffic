using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[CreateAssetMenu(
	    fileName = "WaypointCollection.asset",
	    menuName = SOArchitecture_Utility.COLLECTION_SUBMENU + "Waypont",
	    order = 120)]
	public class WaypointCollection : Collection<WayPoint>
	{
	}
}