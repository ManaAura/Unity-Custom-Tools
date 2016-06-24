using UnityEngine;
using System.Collections;

public class Skill_FireBall : Skill{

    // Update is called once per frame
    protected override void After_CurCastDur_Do()
    {
        GameObject FireBall = Instantiate(this.gameObject);
        base.After_CurCastDur_Do();
        Debug.Log("IM FIRE-BALL" + " BYEBYE in 3SEC");
        Destroy(FireBall, 3f);
    }
}
