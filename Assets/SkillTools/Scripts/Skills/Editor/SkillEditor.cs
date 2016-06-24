/*Patrick's Skill Editor*/
using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Skill))]
public class SkillEditor : Editor{

    //SerializedProperty intentionProp;
    Skill curSkill;

    protected virtual void OnEnable()
    {
        //intentionProp = serializedObject.FindProperty("intention");
       curSkill  = (Skill)target;
    }



    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GUIStyle boxStyle;
        boxStyle = new GUIStyle(GUI.skin.box);
        boxStyle.richText = true;
        boxStyle.normal.textColor = Color.red;
        GUILayout.Box("<b>"+curSkill.name.ToUpper()+" SETTINGS</b>", boxStyle, GUILayout.ExpandWidth(true));
        EditorGUILayout.BeginVertical("box");
        DrawDefaultInspector();
        ValueRest();
        EditorUtility.SetDirty(target);
        EditorGUILayout.EndVertical();
        serializedObject.ApplyModifiedProperties();

    }

    void ValueRest ()
    {
        if (curSkill.castType == CastType.Instant)
            curSkill.castTime = 0;
    }
}







