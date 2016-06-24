using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(SkillInventory))]
public class SkillInventoryEditor : Editor
{
    SerializedProperty skillList;
    SkillInventory sInv;
    List<SaveState> saved = new List<SaveState>();
    List<GameObject> skillInstances = new List<GameObject>();
    public enum SaveState { Saved = 0, NotSaved = 1, Null = 2 };

    void OnEnable()
    {
        sInv = (SkillInventory)target;
        skillList = serializedObject.FindProperty("skills");
        for (int i = 0; i < skillList.arraySize; i++)
        {
            if (saved.Count < skillList.arraySize)
            {
                saved.Add(SaveState.Null);
                if (sInv.skills[i] != null)
                    sInv.skills[i].RestAll();
            }
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawDefaultInspector();
       
        GUI.backgroundColor = Color.grey;
        EditorGUILayout.BeginVertical("box");

        for (int i = 0; i < sInv.skills.Count; i++)
        {
            SerializedProperty curSkill = skillList.GetArrayElementAtIndex(i); 
            if(sInv.skills[i] != null)
            {
                GUI.backgroundColor = MakeColor_ByHex("#7E2927FF"); //red
                EditorGUILayout.BeginVertical("box");
                GUI.backgroundColor = MakeColor_ByHex("#1E2121FF"); //grey inner
                EditorGUILayout.BeginVertical("box");

                GUIStyle boxStyle = new GUIStyle(GUI.skin.box);
                boxStyle.normal.background = MakeTex(2, 2, MakeColor_ByHex("#272828FF"));
                boxStyle.richText = true;


                if (saved.Count < skillList.arraySize)
                {
                    saved.Add(SaveState.Null);
                    if (sInv.skills[i] != null)
                        sInv.skills[i].RestAll();
                }

                if (saved[i] == SaveState.NotSaved)
                {
                    EditSkills(curSkill);
                    if (GUILayout.Button("<b> <color=green>Save: " + curSkill.objectReferenceValue.name + "</color></b>", boxStyle, GUILayout.ExpandWidth(true)))
                    {
                        sInv.skills[i].RestAll();
                        saved[i] = SaveState.Saved;
                    }          
                }
                else if(saved[i] == SaveState.Saved || saved[i] == SaveState.Null)
                {
                    ViewSkills(curSkill);
                    if (GUILayout.Button("<b> <color=green>Modify: " + curSkill.objectReferenceValue.name + "</color></b>", boxStyle, GUILayout.ExpandWidth(true)))
                    {
                        saved[i] = SaveState.NotSaved;
                       
                    }
                }


                EditorUtility.SetDirty(target);
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndVertical();

            }
        }
        EditorGUILayout.EndVertical();


        serializedObject.ApplyModifiedProperties();
    }

    private void ViewSkills(SerializedProperty curSkill)
    {
        Editor curSkillEditor = Editor.CreateEditor(curSkill.objectReferenceValue);
        string lvlVal = curSkillEditor.serializedObject.FindProperty("level").intValue.ToString();

        string[] dmgTypeNames = curSkillEditor.serializedObject.FindProperty("damageType").enumDisplayNames;
        string dmgTypeVal = dmgTypeNames[curSkillEditor.serializedObject.FindProperty("damageType").enumValueIndex];
        string dmgVal = curSkillEditor.serializedObject.FindProperty("damage").floatValue.ToString();

        float cdTimeVal = curSkillEditor.serializedObject.FindProperty("coolDownTime").floatValue;
        float cdVal = curSkillEditor.serializedObject.FindProperty("curCoolDown").floatValue;
        bool isOnCd = curSkillEditor.serializedObject.FindProperty("isOnCD").boolValue;

        string[] castTypeNames = curSkillEditor.serializedObject.FindProperty("castType").enumDisplayNames;
        string castTypeVal = castTypeNames[curSkillEditor.serializedObject.FindProperty("castType").enumValueIndex];

        float castTimeVal = curSkillEditor.serializedObject.FindProperty("castTime").floatValue;
        float castDurVal = curSkillEditor.serializedObject.FindProperty("curCastDuration").floatValue;
        bool isCasting = curSkillEditor.serializedObject.FindProperty("isCasting").boolValue;

        string[] effTypeNames = curSkillEditor.serializedObject.FindProperty("effectType").enumDisplayNames;
        string effTypeVal = effTypeNames[curSkillEditor.serializedObject.FindProperty("effectType").enumValueIndex];
        float effRadiusVal = curSkillEditor.serializedObject.FindProperty("effectRadius").floatValue;
        float effTimeVal = curSkillEditor.serializedObject.FindProperty("effectTime").floatValue;


        //Header
        GUIStyle boxStyle = new GUIStyle(GUI.skin.box);
        boxStyle.normal.background = MakeTex(2, 2, MakeColor_ByHex("#272828FF"));
        boxStyle.richText = true;
        EditorGUILayout.LabelField("<b> <color=#E24B47FF>" + curSkill.objectReferenceValue.name + "</color></b>", boxStyle, GUILayout.ExpandWidth(true));

        GUIStyle richTextStyle = new GUIStyle();
        richTextStyle.richText = true;

        EditorGUILayout.SelectableLabel(ColorOn("Level: ", "#999999FF") + BoldColorOn(lvlVal, "#D0D0D0FF"), richTextStyle, GUILayout.MaxHeight(18f));
        EditorGUILayout.SelectableLabel(ColorOn("DamageType: ", "#999999FF") + BoldColorOn(dmgTypeVal, "#D0D0D0FF"), richTextStyle, GUILayout.MaxHeight(18f));
        EditorGUILayout.SelectableLabel(ColorOn("Damage: ", "#999999FF") + BoldColorOn(dmgVal, "#D0D0D0FF"), richTextStyle, GUILayout.MaxHeight(18f));

        if (castTypeVal == "Instant")
        {
            EditorGUILayout.SelectableLabel(ColorOn("CastTime: ", "#999999FF") + BoldColorOn(castTypeVal, "#D0D0D0FF"), richTextStyle, GUILayout.MaxHeight(18f));
            curSkillEditor.serializedObject.FindProperty("coolDownTime").floatValue = 0;
        }
        else if (!isCasting)
            EditorGUILayout.SelectableLabel(ColorOn("CastTime: ", "#999999FF") + BoldColorOn(castTimeVal.ToString()+ " sec", "#D0D0D0FF"), richTextStyle, GUILayout.MaxHeight(18f));
        else
            ProgressBar(castDurVal / castTimeVal, "CastCompleteIn: " + castDurVal.ToString("F2"));

        if (!isOnCd)
            EditorGUILayout.SelectableLabel(ColorOn("CoolDown: ", "#999999FF") + BoldColorOn(cdTimeVal.ToString() + " sec", "#D0D0D0FF"), richTextStyle, GUILayout.MaxHeight(18f));
        else
            ProgressBar(cdVal / cdTimeVal, "CoolDown: " + cdVal.ToString("F2"));

        EditorGUILayout.SelectableLabel(ColorOn("EffectType: ", "#999999FF") + BoldColorOn(effTypeVal, "#D0D0D0FF"), richTextStyle, GUILayout.MaxHeight(18f));
        if (effTypeVal != "None")
        {
            EditorGUILayout.SelectableLabel(ColorOn("EffectRadius: ", "#999999FF") + BoldColorOn(effRadiusVal.ToString(), "#D0D0D0FF"), richTextStyle, GUILayout.MaxHeight(18f));
            EditorGUILayout.SelectableLabel(ColorOn("EffectTime: ", "#999999FF") + BoldColorOn(effTimeVal.ToString(), "#D0D0D0FF"), richTextStyle, GUILayout.MaxHeight(18f));
        }
        
        EditorUtility.SetDirty(curSkill.objectReferenceValue);

    }

    private void EditSkills(SerializedProperty curSkill)
    {
        if (curSkill.objectReferenceValue != null)
        {
            if (Event.current.type != EventType.DragPerform)
            {
                GUI.backgroundColor = Color.white;
                EditorGUILayout.BeginVertical("box");

                Editor curSkillEditor = Editor.CreateEditor(curSkill.objectReferenceValue);
                curSkillEditor.OnInspectorGUI();

                EditorUtility.SetDirty(curSkill.objectReferenceValue);
            }
        }
    }

    private void ProgressBar(float value, string label)
    {
        // Get a rect for the progress bar using the same margins as a textfield:
        Color previousColor = GUI.color;
        GUI.backgroundColor = Color.yellow;
        GUI.contentColor = Color.red;
        Rect rect = GUILayoutUtility.GetRect(18, 18, "TextField");
        EditorGUI.ProgressBar(rect, value, label);
        EditorGUILayout.Space();
        GUI.color = previousColor;
    }

    private Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; ++i)
        {
            pix[i] = col;
        }
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }

    Color MakeColor_ByHex(string hex)
    {
        Color myColor = new Color();
        ColorUtility.TryParseHtmlString(hex, out myColor);
        return myColor;
    }

    private string BoldColorOn(string content, string hexcolor)
    {
        string answer = "<b><color=" + hexcolor + ">" + content + "</color></b>";
        return answer;
    }

    private string ColorOn(string content, string hexcolor)
    {
        string answer = "<color=" + hexcolor + ">" + content + "</color>";
        return answer;
    }

    private bool KeyCopied(string haystack, params string[] needles)
    {
        foreach (string needle in needles)
        {
            if (haystack.Contains(needle))
                return true;
        }
        return false;
    }

}
