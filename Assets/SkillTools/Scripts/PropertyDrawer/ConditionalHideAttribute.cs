using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Enum |
    AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]

public class ConditionalHideAttribute : PropertyAttribute
{
    public string ConditionalSourceField = "";           //The name of the bool field that will be in control
    public bool HideInInspector = false;                 //TRUE = Hide in inspector / FALSE = Disable in inspector 
    public List<int> ReactionValues = new List<int>();   //SourceFieldValues == one ofthe Reaction Values -> HideNow
    public CondATT_HideTriggers HideTriggers;            //Tells what the type of "Reaction Values"

    public ConditionalHideAttribute(string conditionalSourceField, string trigValues, CondATT_HideTriggers trig,  bool hideInInspector)
    {
        this.ConditionalSourceField = conditionalSourceField;
        this.HideInInspector = hideInInspector;
        switch (trig)
        {
            case CondATT_HideTriggers.IntTrig:
                RetrieveAndAdd_keyIntValues(trigValues);
                break;
            case CondATT_HideTriggers.BoolTrig:
                RetrieveAndAdd_keyBoolValues(trigValues);
                break;
            default:
                break;
        }
        this.HideTriggers = trig;
    }

    void RetrieveAndAdd_keyIntValues(string s)
    {
        foreach (char c in s)
        {
            if (c != ',')
            {
                //convert char to int
                int value = c - '0';
                ReactionValues.Add(value);
            }
        }
    }

    void RetrieveAndAdd_keyBoolValues(string s)
    {
        int boolValue = s[0] - '0';
        ReactionValues.Add(boolValue);
    }

}

[System.Serializable]
public enum CondATT_HideTriggers
{
    BoolTrig = 0,
    IntTrig = 1,
    StringTrig = 2
}