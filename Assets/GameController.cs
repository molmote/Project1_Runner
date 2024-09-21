using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public enum GAME_STATE
	{ 
		PLAYER_CHOICE, // wait for user input
		PLAYER_ACTION, // camera animation
		ENEMY_ACTION,  // camera animation

		PLAYER_DIE,	   // wait for user input	
		ENEMY_DIE,	   // get reward	
		PLAYER_REWARD, // wait for user input
		ENEMY_NEXT,	   // walking animation
		WIN			   // wait for user input	
	}

	protected static GameController _instance;
	public static GameController Instance
	{
		get
		{
			return _instance;
		}
	}

	//[SerializeField] Transform GoSkillUI;
	[SerializeField] Transform GoStatsUI;
	[SerializeField] Transform GoItemsUI;
	[SerializeField] Transform GoActionUI;
	[SerializeField] Transform GoWinOrLoseUI;

	[SerializeField] CameraShake cameraEffect;	

	[SerializeField] Transform GoPlayerHP;
	[SerializeField] Transform GoEnemyHP;

	[SerializeField] TextMeshPro m_textState;
	GAME_STATE gamestate = GAME_STATE.ENEMY_NEXT;
	int enemyCount = 0;
	public string nickname;

	public void UserInfoAction(UserJson userJson)
	{
		if (userJson == null)
		{
			player.Reset();
			userJson = new UserJson(SystemInfo.deviceUniqueIdentifier, nickname,
				player, enemyCount);
			FirebaseController.Instance.SaveUserInfo(userJson);

			Debug.Log("no user data was found, create new one");
		}
		else
		{
			Debug.Log("Continues with saved data");
			player.LoadData(userJson);

			enemyCount = userJson.currentEnemyCount;
			nickname = userJson.nickname;
		}

		StartCoroutine(CheckTurn());
		enemy.Disappear();
	}

	// Start is called before the first frame update
	void Start()
    {
		_instance = this;

		FirebaseController.Instance.Initialize(TryLogin, null);
		GoActionUI.gameObject.SetActive(false);
		GoWinOrLoseUI.gameObject.SetActive(false);
	}

	private void TryLogin()
	{
		FirebaseController.Instance.UserLoggedIn(
			SystemInfo.deviceUniqueIdentifier, UserInfoAction);
	}

    // Update is called once per frame
    void Update()
    {
		//if ()
		//input return -> action selected 
		//gamestate = GAME_STATE.PLAYER_ACTION;
		//CheckTurn();
	}

	IEnumerator CheckTurn()
	{
		if (gamestate == GAME_STATE.ENEMY_NEXT)
		{
			player.Run();

			yield return new WaitForSeconds(3f);

			enemy.Appear(enemyCount);

			gamestate = GAME_STATE.PLAYER_CHOICE;

			//GoActionUI.gameObject.SetActive(true);
		}
		else if (gamestate == GAME_STATE.PLAYER_CHOICE)
		{
			GoActionUI.gameObject.SetActive(true);
			player.StopRunning();
			yield return new WaitForSeconds(0.1f);
		}
		else if (gamestate == GAME_STATE.PLAYER_ACTION)
		{
		}
		else if (gamestate == GAME_STATE.PLAYER_DIE)
		{
		}
		else if (gamestate == GAME_STATE.ENEMY_ACTION)
		{
			yield return new WaitForSeconds(1f);
			EnemyAttack();
		}
		else if (gamestate == GAME_STATE.ENEMY_DIE)
		{
			// enemy.Disappear();
		}
		else if (gamestate == GAME_STATE.PLAYER_REWARD)
		{
			int p = 0;
		}

		UpdateUI();

		yield return new WaitForSeconds(0.1f);
		StartCoroutine(CheckTurn());
	}

	public void UpdateUI()
	{
		var nextState = gamestate.ToString();
		if (m_textState.text != nextState)
		{
			m_textState.text = gamestate.ToString();
			FirebaseController.Instance.Log(m_textState.text);
		}
	}

	public void PlayerSelection(/*GAME_STATE state, */int additionalInfo)
	{
		UpdateUI();
		if (gamestate == GAME_STATE.PLAYER_CHOICE)
		{
			if (additionalInfo == 0) // attack
			{
				PlayerAttack(false);
			}
			else if (additionalInfo == 1 && player.SkillCount > 0) // use skill
			{
				PlayerAttack(true);
			}
			else if (additionalInfo == 2 && player.PotionCount > 0) // use potion
			{
				player.UsePotion();
				gamestate = GAME_STATE.ENEMY_ACTION;
			}
			else
			{
				m_textState.text = "You don't have enough resources";
				return;
			}

			// gamestate = GAME_STATE.PLAYER_ACTION;
			GoActionUI.gameObject.SetActive(false);
		}

		else if (gamestate == GAME_STATE.PLAYER_REWARD)
		{
			player.ApplyItems(additionalInfo);

			GoItemsUI.gameObject.SetActive(false);
			gamestate = GAME_STATE.ENEMY_NEXT;
		}

		else if (gamestate == GAME_STATE.WIN)
		{
			if (additionalInfo == 0) // Continue
			{
				player.Jump();
			}
			else if (additionalInfo == 1) // Reset the game
			{
				ResetGame();
			}

			GoWinOrLoseUI.gameObject.SetActive(false);
		}

		else if (gamestate == GAME_STATE.PLAYER_DIE)
		{
			if (additionalInfo == 0) // Try again this enemy
			{
				//do nothing
				TryLogin();
			}
			else if (additionalInfo == 1) // Reset the game
			{
				ResetGame();
			}
		}
	}

	private void ResetGame()
	{
		player.Reset();
		enemyCount = 0;
		var userJson = new UserJson(SystemInfo.deviceUniqueIdentifier, nickname,
			player, enemyCount);
		FirebaseController.Instance.SaveUserInfo(userJson);
		gamestate = GAME_STATE.ENEMY_NEXT;
	}

	public void PlayerAttack(bool isSkill)
	{
		float diff = (player.CurrentAtk - enemy.CurrentDef);
		if (isSkill)
		{
			diff = player.PlayerSP - enemy.EnemySPDef;
			player.SkillCount -= 1;
		}

		if (diff < 1)
		{
			diff = 1; // always do minimal damage
		}

		player.Attack();
		cameraEffect.Shake(.5f, .5f, enemy);
		if (enemy.Damaged(diff))
		{
			enemyCount++;
			var userJson = new UserJson(SystemInfo.deviceUniqueIdentifier, nickname,
				player, enemyCount);
			FirebaseController.Instance.SaveUserInfo(userJson);

			int maxEnemy = ScriptableDataManager.Get("MaxEnemyCount").value1;
			if (enemyCount > maxEnemy)
			{
				// you are winner
				gamestate = GAME_STATE.WIN;
				GoWinOrLoseUI.gameObject.SetActive(true);
			}
			else
			{
				gamestate = GAME_STATE.PLAYER_REWARD;
				GoItemsUI.gameObject.SetActive(true);
			}
		}
		else
		{
			gamestate = GAME_STATE.ENEMY_ACTION;
		}
		GoActionUI.gameObject.SetActive(false);
	}

	public void EnemyAttack()
	{
		float diff = (enemy.CurrentAtk - player.CurrentDef);
		if (diff < 1)
		{
			diff = 1; // always do minimal damage
		}

		enemy.Attack();
		cameraEffect.Shake(1f, 1f, player);
		if (player.Damaged(diff))
		{
			gamestate = GAME_STATE.PLAYER_DIE;
			GoWinOrLoseUI.gameObject.SetActive(true);
			// enemyCount = 0;
		}
		else
		{
			gamestate = GAME_STATE.PLAYER_CHOICE;
		}
	}

	[SerializeField] PlayerController player;
	[SerializeField] EnemyController enemy;
	//[SerializeField] EnemyController secondEnemy;
	//[SerializeField] EnemyController thirdEnemy;
}
