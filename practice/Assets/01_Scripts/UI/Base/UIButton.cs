using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIButton : MonoBehaviour
{
    private Button m_Button = null;

    protected Button button
	{
		get
		{
			if (m_Button == null)
			{
				m_Button = GetComponentInChildren<Button>();
				if (m_Button == null)
				{
					m_Button = this.gameObject.AddComponent<Button>();
				}
			}
            return m_Button;
        }
	}
  

    protected virtual void OnEnable()
    {
        button.onClick.AddListener(OnClicked);
    }

    protected virtual void OnDisable()
    {
        button.onClick.RemoveListener(OnClicked);
    }

    public void SetInteractable(bool interactable)
    {
        button.interactable = interactable;
    }

    protected virtual void OnClicked() { }
}
