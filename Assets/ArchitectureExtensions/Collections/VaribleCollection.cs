using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[CreateAssetMenu(
	    fileName = "VaribleCollection.asset",
	    menuName = SOArchitecture_Utility.COLLECTION_SUBMENU + "VaribleCollection",
	    order = 120)]
	public class VaribleCollection : Collection<BaseVariable>
	{
	}
}