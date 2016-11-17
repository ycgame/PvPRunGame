using UnityEngine;
using UnityEditor;
using System.Collections;

public class DeleteSavedata
{
	[MenuItem ("Tools/Delete Savedata")]
	public static void Delete () 
	{
		PlayerPrefs.DeleteAll ();
	}

}
