using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyController : EntityController
{
	public int EnemySPDef;

	[SerializeField] TextMeshPro m_textHP;
	[SerializeField] TextMeshPro m_textSkillDef;
	[SerializeField] TextMeshPro m_textAtk;
	[SerializeField] TextMeshPro m_textDef;

	[SerializeField] Transform model;
	[SerializeField] Animator animator;

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Appear(int count)
	{
		var hpValue = ScriptableDataManager.Get("EnemyHP").valueArray;

		if (count < hpValue.Length)
		{
			CurrentHP = ScriptableDataManager.Get("EnemyHP").valueArray[count];
			CurrentAtk = ScriptableDataManager.Get("EnemyAtk").valueArray[count];
			CurrentDef = ScriptableDataManager.Get("EnemyDef").valueArray[count];
			EnemySPDef = ScriptableDataManager.Get("EnemySPDef").valueArray[count];
		}
        else
		{
			CurrentHP = ScriptableDataManager.Get("EnemyHP").valueArray[hpValue.Length-1];
			CurrentAtk = ScriptableDataManager.Get("EnemyAtk").valueArray[hpValue.Length - 1];
			CurrentDef = ScriptableDataManager.Get("EnemyDef").valueArray[hpValue.Length - 1];
			EnemySPDef = ScriptableDataManager.Get("EnemySPDef").valueArray[hpValue.Length - 1];
		}

		model.gameObject.SetActive(true);
		UpdateUI();
	}

	public void UpdateUI()
	{
		m_textHP.text = $"HP: {CurrentHP}";
		m_textSkillDef.text = $"SP: {EnemySPDef}";
		m_textAtk.text = $"ATK: {CurrentAtk}";
		m_textDef.text = $"DEF: {CurrentDef}";
	}

	public void Disappear()
	{
		model.gameObject.SetActive(false);
	}

	public void Attack()
	{
		//animator.SetBool("Jump", false);
	}

	public bool Damaged(float damage)
	{
		CurrentHP -= damage;
		UpdateUI();

		if (CurrentHP <= 0)
		{
			Disappear();
			return true;
		}
		else
		{
			//animator.SetBool("Jump", true);
		}
		return false;
	}
}
