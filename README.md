# Unity-Custom-Tools
Tools that meant to boost up game_dev speed

# SAS Maker
SAS stands for skills,abilities,spells
Watch it on YOUTUBE: https://youtu.be/VJ6Gv8bbCes 

     SkillInventory's Inspector  
     A collection of the skills you created, where you can obvserve and modify their bheaviours

     Skill.cs
     
     DESCRIPTION:
         This is the BASE CLASS to create any desired skills
     
     HOW TO USE: 
         1. Inherite this class for your new skill scripts
         2. Attach your new skill script to ANY game-object
         3. Edit the inspector
         4. Call newskill.CastNow() in your "player / AI" class
     
     IMPORTANT METHODS:
         1.The two methods that you are going to override the most are probably
             a. "While / AFTER_curCastDur_DO()" - for the skills that correlate with CoolDowns (instant = 0 cast duration).
             b. "CastNow()" - for the skills that DON'T correlate with CoolDowns
         
     HIGHLIGHTED FEATURES:
         1.DURATINOAL CONTROL:
             a.For curCoolDown, curCastDuration, curEffectDuration: They are counted from Max to 0.
                     TL- Timeline
                     CM- Control Methods (overridable)
     
                                  TL:    CastNow    >    (While Counting Duration)   >   (Counting Complete)      
                                                                     |                            |    
                                                                     |                            |
                                                                     |                            |
                                  CM:                       While_"DurType"_Do()         After_"DurType"_Do()         
                                     
         2. MULTIPLE META-SKILLS:
             a. For a single skill, it allows to have multiple meta skills.
             b. They are stored in "ExtraSkills" variable under "EFFECT SETTING"
             c. Manipulate and Call "ApplyExtraSkill()" in "CastNow()" for their behaviours
