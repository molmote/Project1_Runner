using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float PlayerHP;
	public int PlayerSP;
	public int PlayerAtk;
	public int PlayerDef;

    public bool IsTurn;

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

    public void Run()
    {
        animator.SetFloat("Speed", 10);
	}

    public void StopRunning()
	{
        animator.SetFloat("Speed", 0);
	}

    public void Damaged()
    {
        animator.SetBool("Jump", true);
    }
}
