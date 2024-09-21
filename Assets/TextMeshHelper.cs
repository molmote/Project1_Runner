using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextMeshHelper : MonoBehaviour
{
	//When the mouse hovers over the GameObject, it turns to this color (red)
	Color m_MouseOverColor = Color.yellow;
	Color m_OriginalColor;

	[SerializeField] bool allowClickEvent;
	[SerializeField] GameController.GAME_STATE nextState;
	[SerializeField] int additionalInfo;

	//This stores the GameObject¡¯s original color

	MeshRenderer m_Renderer;

	TextMeshPro m_text;

	BoxCollider collider;
	// Start is called before the first frame update
	void Start()
    {
        if (collider == null)
		{
			collider = GetComponent<BoxCollider>();
			if (collider == null)
			{
				collider = transform.gameObject.AddComponent<BoxCollider>();
			}
		}

		/*if (m_Renderer == null)
		{
			m_Renderer = transform.gameObject.GetComponent<MeshRenderer>();
		}*/

		if (m_text == null)
		{
			m_text = transform.gameObject.GetComponent<TextMeshPro>();

			m_OriginalColor = m_text.color;
		}
	}

    // Update is called once per frame
    void Update()
    {

	}

	void OnMouseOver()
	{
		// Change the color of the GameObject to red when the mouse is over GameObject
		m_text.color = m_MouseOverColor;
	}

	void OnMouseExit()
	{
		// Reset the color of the GameObject back to normal
		// Change the color of the GameObject to red when the mouse is over GameObject
		m_text.color = m_OriginalColor;
	}

	private void OnMouseUp()
	{
		GameController.Instance.PlayerSelection(additionalInfo);
	}
}
