using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using Google.MiniJSON;
using System;
using System.Collections;
using UnityEngine;
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
//using Firebase.Crashlytics;
#endif

public class FirebaseController : MonoBehaviour
{

	protected static GameObject _goInstance = null;
	protected static FirebaseController _instance;
	public static FirebaseController Instance
	{
		get
		{
			if (_goInstance == null)
			{
				_goInstance = new GameObject("ThridPartyManagers");
				DontDestroyOnLoad(_goInstance);
			}

			if (_instance == null)
			{
				_instance = _goInstance.AddComponent<FirebaseController>();
				// _instance.Initialize();
			}
			return _instance;
		}
	}

    FirebaseApp fba;

	float byteToMB;

	double pauseStarted = 0;
	double totalPausedTime = 0;
	double pauseRealtimeStarted = 0;
	double totalPausedRealtime = 0;

	bool bInitialized = false;

	public delegate void VoidDelegate();
	public delegate void UserInfoDelegate(UserJson info);

	public void Initialize(VoidDelegate onSuccess, VoidDelegate onFail)
	{
		StartCoroutine(InitializeCoroutine(onSuccess, onFail));
	}

	public void AddItem(int itemType, int itemCount)
	{

	}

	public IEnumerator InitializeCoroutine(VoidDelegate onSuccess, VoidDelegate onFail)
	{
		if (bInitialized)
		{
			onSuccess?.Invoke();
			yield break;
		}

        // Initialize Firebase
        var asyncTask = Firebase.FirebaseApp.CheckAndFixDependenciesAsync();
        yield return new WaitUntil(() => asyncTask.IsCompleted);
        
        if(asyncTask.Result != Firebase.DependencyStatus.Available)
        {
            onFail?.Invoke();
            yield break;
        }

        fba = Firebase.FirebaseApp.DefaultInstance;

        // When this property is set to true, Crashlytics will report all
        // uncaught exceptions as fatal events. This is the recommended behavior.
        // Crashlytics.ReportUncaughtExceptionsAsFatal = true;

        // Set a flag here for indicating that your project is ready to use Firebase.
        bInitialized = true;
        SetCustomData("SystemLanguage", Application.systemLanguage.ToString());

		bInitialized = true;
		onSuccess?.Invoke();

		Application.quitting += OnQuit;

		byteToMB = 1.0f / 1024;
		byteToMB *= byteToMB;

		StartCoroutine(RegularRecordCoroutine());

#if TEST_BUILD
        Log("DEV_BUILD");
#endif
	}

	void OnQuit()
	{
		StopAllCoroutines();

		SetCustomData("Application.quitting -> OnQuit", "1");
	}

	public void SaveUserInfo(UserJson info)
	{
		DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
		string json = JsonUtility.ToJson(info);

		reference.Child("users").Child(info.userID).SetRawJsonValueAsync(json);
		Debug.Log(json);
	}

	public void ApplyItemGet(string userID, string itemID, int itemType)
	{
		DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

		var itemJson = new ItemJson(itemType, DateTime.Now.Ticks, 0);
		string json = JsonUtility.ToJson(itemJson);

		reference.Child("items").Child(itemID).SetRawJsonValueAsync(json);
	}

	public void ApplyItemUsedInstantly(string userID, string itemID, int itemType)
	{

	}

	public void ApplyItemUsed(string userID, string itemID, int itemType)
	{
		DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

		var itemJson = new ItemJson(itemType, DateTime.Now.Ticks, 0);
		//reference.Child("items").Child(itemID).
		//	UpdateChildrenAsync(itemJson)
	}

	public void UserLoggedIn(string userID, UserInfoDelegate UserInfoEvent)
	{
		DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

		UserJson userJson = null;
		FirebaseDatabase.DefaultInstance
	  .GetReference("users").Child(userID)
	  .GetValueAsync().ContinueWithOnMainThread(task => {
		  if (task.IsFaulted)
		  {
			  // Handle the error...
			  UserInfoEvent(userJson);
		  }
		  else if (task.IsCompleted)
		  {
			  DataSnapshot snapshot = task.Result;
			  // Do something with snapshot...
			  if (snapshot != null)
			  {
				  string json = task.Result.GetRawJsonValue();
				  Debug.Log(json);
				  userJson = JsonUtility.FromJson<UserJson>(json);

				  UserInfoEvent(userJson);
			  }
		  }
	  });
	}

	//set player environment key value
	public void SetCustomData(string key, string value)
	{
		if (bInitialized == false) return;

	}

	public void Log(string log)
	{
		if (bInitialized == false) return;

#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        //Crashlytics.Log(log);
#else
		Debug.Log(log);
#endif
	}

	public void SendException(string exceptionString)
	{
		if (bInitialized == false) return;

		try
		{
			throw new Exception(exceptionString);
		}
		catch (Exception e)
		{
			SendException(e);
		}
	}

	public void SendException(Exception e)
	{
		if (bInitialized == false) return;

#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        //Crashlytics.LogException(e);
#else
		Debug.LogError(e.Message + ", " + e.StackTrace);
#endif
	}

	IEnumerator RegularRecordCoroutine()
	{
		while (true)
		{
			RecordPlaytime();

			yield return new WaitForSeconds(10);
		}
	}

	void RecordPlaytime()
	{
		SetCustomData("Playtime", $"Time : {Time.time - totalPausedTime:f2} / Realtime : {Time.realtimeSinceStartup - totalPausedRealtime:f2}");
	}

	private void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			pauseStarted = Time.time;
			pauseRealtimeStarted = Time.realtimeSinceStartup;
		}
		else
		{
			totalPausedTime += Time.time - pauseStarted;
			totalPausedRealtime += Time.realtimeSinceStartup - pauseRealtimeStarted;
		}

		SetCustomData("OnApplicationPause", pause ? "1" : "0");
	}

	private void OnApplicationQuit()
	{
		SetCustomData("OnApplicationQuit", "1");
	}
}
