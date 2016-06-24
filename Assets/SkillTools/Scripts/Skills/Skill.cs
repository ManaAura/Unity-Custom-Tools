using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public enum SkillIntention { Harmful = 0, Beneficial = 1, Both_Harmful_and_Beneficial = 2 }
public enum CastType { Instant = 0, Channel = 1 }
public enum ReleaseCastType { No_Target = 0, Unit_Target = 1, Point_Target = 2, Passive = 3 }
public enum DamageType { Physical = 0, Magical = 1 }
public enum SkillEffectType {None = 0 , Aoe = 1, BuffOrDebuff = 2 , OtherSkill = 3}

public class Skill : MonoBehaviour
{
    /// <summary>
    /// 
    /// DESCRIPTION:
    ///     This is the BASE CLASS for any of desire skills.
    /// 
    /// HOW TO USE: 
    ///     1. Inherite this class for your new skill scripts
    ///     2. Attach your new skill script to ANY game-object
    ///     3. Edit the inspector
    ///     4. Call newskill.CastNow() in your "player / AI" class
    /// 
    /// IMPORTANT METHODS:
    ///     1.The two method that you are going to override the most are probably
    ///         a. "While / AFTER_curCastDur_DO()" - for the skills that correlate with CoolDowns (instant = 0 cast duration).
    ///         b. "CastNow()" - for the skills that DON'T correlate with CoolDowns
    ///     
    /// HIGHLIGHTED FEATURES:
    ///     1.DURATINOAL CONTROL:
    ///         a.For curCoolDown, curCastDuration, curEffectDuration: They are counted from Max to 0.
    ///                 TL- Timeline
    ///                 CM- Control Methods (overridable)
    /// 
    ///                              TL:    CastNow    >    (While Counting Duration)   >   (Counting Complete)      
    ///                                                                 |                            |    
    ///                                                                 |                            |
    ///                                                                 |                            |
    ///                              CM:                       While_"DurType"_Do()         After_"DurType"_Do()         
    ///                                 
    ///     2. MULTIPLE META-SKILLS:
    ///         a. For a single skill, it allows to have multiple meta skills.
    ///         b. They are stored in "ExtraSkills" variable under "EFFECT SETTING"
    ///         c. Manipulate and Call "ApplyExtraSkill()" in "CastNow()" for their behaviours
    /// 
    /// </summary>

    #region Property: Basic Fields
    [Header ("---BASIC SETTING---")]
    public int level;
    public SkillIntention intention;
    [ConditionalHide("intention", "1", CondATT_HideTriggers.IntTrig, true)]
    public DamageType damageType;
    [ConditionalHide("intention", "1", CondATT_HideTriggers.IntTrig, true)]
    public float damage;          
    [ConditionalHide("intention", "0", CondATT_HideTriggers.IntTrig, true)]
    public float beneficialAmount;  
    public float coolDownTime;        
    [HideInInspector]
    public float curCoolDown;      
    [HideInInspector]
    public bool isOnCD = false;
    #endregion

    #region Property: Collision Target Fields
    [Header("---COLLISSION SETTING---")]
    public LayerMask targetLayer;
    public string[] targetTag;
    public GameObject[] targetParticular;
    [HideInInspector]
    public GameObject[] curTargets;
    #endregion

    #region Property: Cast Fields
    [Header("---CAST SETTING---")]
    public ReleaseCastType releaseCastType;  
    public CastType castType;       
    [ConditionalHide("castType", "0", CondATT_HideTriggers.IntTrig, true)]
    public float castTime;      
    [HideInInspector]
    public float curCastDuration;   
    [HideInInspector]
    public bool isCasting = false;  
    [ConditionalHide("releaseCastType", "0,3", CondATT_HideTriggers.IntTrig, true)]
    public float castRange;
    #endregion

    #region Property: Effect Fields
    [Header("---EFFECT SETTING---")]
    public SkillEffectType effectType;
    [ConditionalHide("effectType", "0", CondATT_HideTriggers.IntTrig, true)]
    public float effectRadius;
    [ConditionalHide("effectType", "0,1", CondATT_HideTriggers.IntTrig, true)]
    public float effectTime;
    [HideInInspector]
    public float curEffectDuration;
    [HideInInspector]
    public bool effectIsActive = false;
    [ConditionalHide("effectType", "0,1,2", CondATT_HideTriggers.IntTrig, true)]
    public List<Skill> extraSkill;
    #endregion

    #region Property: UI Resource Fields
    [Header("---UI SETTING---")]
    public string skillDescription;
    public Image skillIcon;
    #endregion
    
    protected virtual void Start()
    {
        RestAll();
    }

    public virtual void CastNow()
    {
        CD_HeadsUps_ForPlayers();
        if ((curCastDuration == castTime && curCoolDown == coolDownTime) && !isCasting)
        {
            if (castType == CastType.Channel)
            {
                isCasting = true;
               StartCoroutine(TimeManager.TimerCountDown_ThenReset(castTime, While_CurCastDur_Do, After_CurCastDur_Do));
            }
            else
            {
                After_CurCastDur_Do();
            }
            
        }
    }

    public virtual void CD_HeadsUps_ForPlayers()
    {
        if (releaseCastType == ReleaseCastType.Passive)
            Debug.Log(this.name + " IS ON COOLDOWN ");
        else if (isOnCD)
            Debug.Log(this.name + " IS ON COOLDOWN ");
        else if (isCasting && castType != CastType.Instant)
            Debug.Log(this.name + " IS CASTING");
    }

    public virtual void ApplyExtraSkill()
    {
        foreach (Skill skill in extraSkill)
            skill.CastNow();
    }

    public virtual void RestAll()
    {
        curCoolDown = coolDownTime;
        curCastDuration = castTime;
        curEffectDuration = effectTime;
    }

    #region Do Something While Count And/Or After Reset the Durations
    protected virtual void While_CurCastDur_Do ()
    {
        curCastDuration -= Time.deltaTime;
    }

    protected virtual void After_CurCastDur_Do()
    {
        curCastDuration = castTime;
        isCasting = false;
        isOnCD = true;
        StartCoroutine(TimeManager.TimerCountDown_ThenReset(coolDownTime, While_CurCD_Do, After_CurCD_Do));
    }

    protected virtual void While_CurCD_Do()
    {
        curCoolDown -= Time.deltaTime;
    }

    protected virtual void After_CurCD_Do()
    {
        curCoolDown = coolDownTime;
        isOnCD = false;
    }

    protected virtual void While_CurEffectDur_Do()
    {
        curEffectDuration -= Time.deltaTime;
    }

    protected virtual void After_CurEffectDur_Do()
    {
        curEffectDuration = effectTime;
        effectIsActive = false;
    }
    #endregion

}



