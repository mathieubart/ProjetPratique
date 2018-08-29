using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class EnumModifierEditor : EditorWindow
{
	[SerializeField]
	private ScriptableObject m_Target; //The scriptable object to modify.
	private ScriptableObject m_CurrentTarget; //The Actual scriptable object to modify. *Prevent to re-Load the Target at each frame*
	
	private MonoScript m_TargetMonoScript; //The Script Instance Reference of the ScriptableObject.

	private string m_FileText; //The file content has string.
	private string m_EnumName;
//	private List<string> m_EnumElements = new List<string>();

	//Create A Menu Button (Where The File, Edit, Windows Buttons Are)
	//Call The Fucntion Below On Clic
	[MenuItem("Tools/Custom Enum Modifier")]
	private static void Init()
	{
		//Create A Tool Tab/Window (Like Inspector, Console, Scene, Game, ...)
		EnumModifierEditor tab = GetWindow<EnumModifierEditor>();
		tab.Show();
	}

	//Used To Populate The Tab.
	private void OnGUI()
	{
		EditorGUI.BeginChangeCheck();

		m_Target = (ScriptableObject)EditorGUILayout.ObjectField("Target", m_Target, typeof(ScriptableObject), true); // Get the ScriptableObject From a Drag and Drop in the Tab Window.

		if(EditorGUI.EndChangeCheck() && m_Target != null)
		{   
			if(m_Target != m_CurrentTarget)
			{
				SetMonoScript(m_Target);
			}

			SetScriptText();

			/* 
			m_EnumElements = GetFileEnumElements();
			Transform[] allTransforms = m_Target.GetComponentsInChildren<Transform>(true);
			m_Components = new Dictionary<Transform, Component[]>();

			for(int i = 0; i < allTransforms.Length; i++)
			{
				Component[] components = allTransforms[i].GetComponents<Component>();
				m_Components.Add(allTransforms[i], components);
			}
			m_Foldouts = new bool[allTransforms.Length];
			*/
		}		

		/* 
		EditorGUI.BeginDisabledGroup(m_Target == null);

		m_Filter = EditorGUILayout.TextField("Filter", m_Filter);
		
		EditorGUILayout.BeginHorizontal();

		GUI.color = Color.cyan;
		if(GUILayout.Button("Expand All"))
		{
			for (int i = 0; i < m_Foldouts.Length; i++)
			{
				m_Foldouts[i] = true;			
		}	}

		GUI.color = Color.magenta;
		if(GUILayout.Button("Collapse All"))
		{
			for (int i = 0; i < m_Foldouts.Length; i++)
			{
				m_Foldouts[i] = false;			
		}	}

		GUI.color = Color.white;		
		EditorGUILayout.EndHorizontal();

		EditorGUI.EndDisabledGroup();
		*/
	}

	private void SetMonoScript(ScriptableObject a_Target)
	{
		m_TargetMonoScript = MonoScript.FromScriptableObject( a_Target ); //Get the Mono Script Reference.
		m_CurrentTarget = a_Target;
	}

	private void SetScriptText()
	{
		m_FileText = System.IO.File.ReadAllText(AssetDatabase.GetAssetPath(m_TargetMonoScript)); //Get the MonoScript content has a string and set it to m_FileText.
	}

	private List<string> GetFileEnumElements()
	{
		List<string> enumElements = new List<string>();

		if(m_FileText.Contains("enum"))
		{

		}
		else
		{
			CreateEnum();
		}

	/* 
		enum NOM
		{
			One = 1,
			Two = 2,

		}
	*/

		return enumElements;
    }

	private void CreateEnum()
	{

	}
}
