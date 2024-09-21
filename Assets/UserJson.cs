using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemJson
{
	int itemType;
	long timeGet;
	long timeSpent;

	public ItemJson() { }

	public ItemJson(int _itemType, long _timeGet, long _timeSpent)
	{
		itemType = _itemType;
		timeGet = _timeGet;
		timeSpent = _timeSpent;
	}
}


public class UserJson
{
	public string userID;
	public string nickname;

	// current stats
	public float currentHP;
	public int currentAtk;
	public int currentDef;
	public int currentSkillAtk;
	public int currentSkillCount;

	// inventory
	public int currentPotionCount;

	// progress
	public int currentEnemyCount;

	public ItemJson[] itemList;

	public UserJson()
	{
	}

	public UserJson(string _userID, string _nickname, PlayerController player, int enemyCount)
	{
		userID = _userID;
		nickname = _nickname;

		currentHP = player.CurrentHP;
		currentAtk = player.CurrentAtk;
		currentDef = player.CurrentDef;
		currentSkillAtk = player.PlayerSP;
		currentSkillCount = player.SkillCount;

		currentPotionCount = player.PotionCount;
		currentEnemyCount = enemyCount;

	}

	public void SetItem(ItemJson[] _itemList)
	{
		itemList = _itemList;
	}
}
