using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : EntityController
{
	//public float PlayerHP;
	public int PlayerSP;
	//public int PlayerAtk;
	//public int PlayerDef;
	public int SkillCount;
	public int PotionCount;

	[SerializeField] Transform model;
	[SerializeField] Animator animator;

	[SerializeField] TextMeshPro m_textHP;
	[SerializeField] TextMeshPro m_textAtk;
	[SerializeField] TextMeshPro m_textDef;
	[SerializeField] TextMeshPro m_textSkillAtk;
	[SerializeField] TextMeshPro m_textSkillCount;
	[SerializeField] TextMeshPro m_textPotionCount;

	// Start is called before the first frame update
	void Start()
    {
		m_OriginalColor = m_Renderer.material.color;
	}

	public void Reset()
	{
		CurrentHP = ScriptableDataManager.Get("PlayerHP").value1;
		PlayerSP = ScriptableDataManager.Get("PlayerSP").value1;
		CurrentAtk = ScriptableDataManager.Get("PlayerAtk").value1;
		CurrentDef = ScriptableDataManager.Get("PlayerDef").value1;
		SkillCount = ScriptableDataManager.Get("SkillCount").value1;
		PotionCount = ScriptableDataManager.Get("PotionCount").value1;

		UpdateUI();
	}

	public void LoadData(UserJson userSaved)
	{
		CurrentHP = userSaved.currentHP;
		PlayerSP = userSaved.currentSkillAtk;
		CurrentAtk = userSaved.currentAtk;
		CurrentDef = userSaved.currentDef;
		SkillCount = userSaved.currentSkillCount;
		PotionCount = userSaved.currentPotionCount;

		UpdateUI();
	}

	public void ApplyItems(int itemID)
	{
		if (itemID == 0) // HP UP
		{
			PotionCount += 1;
		}
		else if (itemID == 1) // ATK UP
		{
			CurrentAtk += ScriptableDataManager.Get("ItemAtkUp").value1;
		}
		else if (itemID == 2) // DEF UP
		{
			CurrentDef += ScriptableDataManager.Get("ItemDefUp").value1;
		}

		UpdateUI();
	}

	public void UpdateUI()
	{
		m_textHP.text = $"HP: {CurrentHP}";
		m_textSkillAtk.text = $"SP: {PlayerSP}";
		m_textAtk.text = $"ATK: {CurrentAtk}";
		m_textDef.text = $"DEF: {CurrentDef}";
		m_textSkillCount.text = $"Skill x{SkillCount}";
		m_textPotionCount.text = $"Potion x{PotionCount}";
	}

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Run()
    {
        animator.SetFloat("Speed", 10);
	}

    public void StopRunning()
	{
        animator.SetFloat("Speed", 0);
	}

	public void UsePotion()
	{
		CurrentHP += ScriptableDataManager.Get("ItemPotionHeal").value1;
		PotionCount -= 1;

		UpdateUI();
	}

	public void Attack()
	{
		//animator.SetBool("Jump", false);
		//animator.SetBool("Grounded", true);
	}

	public bool Damaged(float damage)
	{
		CurrentHP -= damage;

		UpdateUI();

		if (CurrentHP <= 0)
		{
			return true;
		}
		else
		{
			//animator.SetBool("Grounded", false);
		}

		return false;
	}

	public void Jump()
	{
		animator.SetBool("Jump", true);
	}
}
