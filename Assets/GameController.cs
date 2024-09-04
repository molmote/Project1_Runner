using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

	private enum GAME_STATE
	{ 
		PLAYER_CHOICE,
		PLAYER_ACTION, //animation
		ENEMY_ACTION,
		
		PLAYER_DIE,
		RESTART,   
		ENEMY_DIE,  
		PLAYER_ITEM,
		ENEMY_NEXT, // PLAYER_CHOICE
		WIN
	}
	// player turn 
	// player action (attack or skill)
	// enemy attack

	// enemy or player hp becomes 0
	// win -> next enemy will show up or ending 
	// loss -> show game over and start it over after 3 seconds



	[SerializeField] Transform GoSkillUI;
	[SerializeField] Transform GoStatsUI;
	[SerializeField] Transform GoItemsUI;
	[SerializeField] Transform GoActionUI;


	[SerializeField] Transform GoPlayerHP;
	[SerializeField] Transform GoEnemyHP;

	GAME_STATE gamestate = GAME_STATE.ENEMY_NEXT;
	int enemyCount = 0;

	// Start is called before the first frame update
	void Start()
    {
		StartCoroutine (CheckTurn());

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
			yield return new WaitForSeconds(2f);

			player.Run();
			enemy.Appear(enemyCount);

			gamestate = GAME_STATE.PLAYER_CHOICE;
		}
		else if (gamestate == GAME_STATE.PLAYER_CHOICE)
		{
			player.StopRunning();
			yield return new WaitForSeconds(0.5f);
		}
		else if (gamestate == GAME_STATE.PLAYER_ACTION)
		{

		}
		else if (gamestate == GAME_STATE.PLAYER_DIE)
		{

		}
		else if (gamestate == GAME_STATE.ENEMY_ACTION)
		{
			enemy.Attack();
			yield return new WaitForSeconds(2f);
			EnemyAttack();
		}
		else if (gamestate == GAME_STATE.ENEMY_DIE)
		{
			enemy.Disappear();

			gamestate = GAME_STATE.PLAYER_ITEM;
		}
		else if (gamestate == GAME_STATE.PLAYER_ITEM)
		{
		}

		UpdateUI();

		yield return new WaitForSeconds(0.5f);
		StartCoroutine(CheckTurn());
	}

	public void UpdateUI()
	{

	}

	public void PlayerAttack(bool isSkill)
	{
		float diff = (player.PlayerAtk - enemy.EnemyDef);
		if (isSkill)
		{
			diff = player.PlayerSP - enemy.EnemySPDef;
		}

		if (diff < 1)
		{
			diff = 1; // always do minimal damage
		}

		enemy.EnemyHP -= diff;
	}

	public void EnemyAttack()
	{
		float diff = (enemy.EnemyAtk - player.PlayerDef);
		if (diff < 1)
		{
			diff = 1; // always do minimal damage
		}

		player.PlayerHP -= diff;
	}

	[SerializeField] PlayerController player;
	[SerializeField] EnemyController enemy;
	//[SerializeField] EnemyController secondEnemy;
	//[SerializeField] EnemyController thirdEnemy;
}
