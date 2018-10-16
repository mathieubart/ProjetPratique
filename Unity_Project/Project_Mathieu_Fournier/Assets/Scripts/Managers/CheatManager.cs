using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheatManager : MonoBehaviour
{
#if CHEATS_ACTIVATED
    private static CheatManager m_Instance;
    public static CheatManager Instance
    {
        get { return m_Instance; }
    }

    public bool m_AreCheatsActive = false;

    [HideInInspector]
    public List<string> m_CheatTexts = new List<string>();

    [HideInInspector]
    public TextMeshProUGUI m_UIText;

    private void Awake()
    {
        if (m_Instance != null)
        {
            Destroy(this);
        }
        else
        {
            m_Instance = this;
        }
        DontDestroyOnLoad(gameObject);

        AddText("Cheats codes are enabled. \n");
    }

    private void Update()
    {
        if(!m_AreCheatsActive)
        {
            ShowCheatUI(false);
        }
        else
        {
            ShowCheatUI(true);
        }
    }

    private void UpdateUI()
    {
        if (m_UIText)
        {
            string newText = "";

            for (int i = 0; i < m_CheatTexts.Count; i++)
            {
                newText += m_CheatTexts[i];
            }

            m_UIText.text = newText;
        }
    }

    public void AddText(string a_Text)
    {
        bool sameFound = false;

        for(int i = 0; i < m_CheatTexts.Count; i++)
        {
            if(m_CheatTexts[i].Equals(a_Text))
            {
                sameFound = true;
            }
        }

        if(!sameFound)
        {
            m_CheatTexts.Add(a_Text);
            UpdateUI();
        }
    }

    public void ResetTexts()
    {
        m_CheatTexts = new List<string>();

        UpdateUI();
    }

    private void ShowCheatUI(bool isActive)
    {
        if(m_UIText)
        {
            m_UIText.enabled = isActive;
        }
    }
#else
    private void Awake()
    {
        Debug.LogError("Cheats are removed from the build. Mathf")
    }
#endif
}
