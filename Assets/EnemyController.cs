using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	public float EnemyHP;
	public int EnemyAtk;
	public int EnemyDef;
	public int EnemySPDef;
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
        if (count == 0)
        {
            EnemyHP = 5;
            EnemyAtk = 5;
            EnemyDef = 5;
			EnemySPDef = 1;
		}
        else if (count == 1)
		{
			EnemyHP = 10;
			EnemyAtk = 10;
			EnemyDef = 10;
			EnemySPDef = 5;
		}
        else
		{
			EnemyHP = 20;
			EnemyAtk = 20;
			EnemyDef = 10;
			EnemySPDef = 10;
		}

		model.gameObject.SetActive(true);
	}

	public void Disappear()
	{
		model.gameObject.SetActive(false);
	}

	public void Attack()
	{

	}

	public void Damaged()
	{
		animator.SetBool("Jump", true);
	}
}
