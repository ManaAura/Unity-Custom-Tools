using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillInventory : MonoBehaviour {

    public List<Skill> skills;
	// Use this for initialization
	void Start ()
    {
       

	}
    
    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyDown("q"))
            skills[0].CastNow();
        if (Input.GetKeyDown("w"))
            skills[1].CastNow();
        if (Input.GetKeyDown("e"))
            skills[2].CastNow();
        if (Input.GetKeyDown("r"))
            skills[3].CastNow();
    }
    

}
