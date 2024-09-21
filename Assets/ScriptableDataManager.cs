using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "ScriptableDataManager", menuName = "Utility/ScriptableDataManager", order = 1)]
public class ScriptableDataManager : ScriptableObject// : ScriptableSingleton<ScriptableDataManager>
{
	protected static ScriptableDataManager _instance;
	public static ScriptableDataManager instance
	{
		get
		{
			if (_instance == null )
			{
				var op = Addressables.LoadAssetAsync<ScriptableDataManager>(
					typeof(ScriptableDataManager).Name);

				_instance = op.WaitForCompletion(); //Forces synchronous load so that we can return immediately
			}

			return _instance;
		}
	}

	[Serializable]
	public class TemplateData
	{
		[SerializeField] public string TemplateName;
		[SerializeField] public int value1;
		[SerializeField] public int []valueArray;
	}

	[SerializeField] public TemplateData[] Configs;

	public static TemplateData Get(string TemplateName)
	{
		return instance.Configs.FirstOrDefault(x => x.TemplateName == TemplateName);
	}
}
