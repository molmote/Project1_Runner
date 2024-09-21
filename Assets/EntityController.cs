using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour
{
	public float CurrentHP;
	public int CurrentAtk;
	public int CurrentDef;

	//When the mouse hovers over the GameObject, it turns to this color (red)
	protected Color mDamagedEffectColor = Color.red;

	//This stores the GameObject¡¯s original color
	protected Color m_OriginalColor;

	//Get the GameObject¡¯s mesh renderer to access the GameObject¡¯s material and color
	 [SerializeField] protected SkinnedMeshRenderer m_Renderer;
	public void FlipColor(bool flip)
	{
		if (flip)
		{
			foreach (var render in m_Renderer.materials)
			{
				render.color = mDamagedEffectColor;
			}
		}
		else
		{
			foreach (var render in m_Renderer.materials)
			{
				render.color = m_OriginalColor;
			}
		}
	}
}
