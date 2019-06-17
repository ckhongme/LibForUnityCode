using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimTool : MonoBehaviour {

    protected bool isRunning = true;

    private Animation anim;
    private string curAnim;

    private void _InitAnim()
    {
        foreach (AnimationState temp in anim)
        {
            temp.speed = 1.0f;
        }
    }

    private void Start()
    {
        anim = GetComponent<Animation>();
        foreach (AnimationState temp in anim)
        {
            Debug.Log(temp.name);
        }
    }


    private void _PlayIdle()
    {
        curAnim = "idle";
        AnimationState state = anim["idle"];
        if (state == null) return;

        foreach (AnimationState temp in anim)
        {
            temp.weight = 0f;
        }
        anim.CrossFade("idle", 0.5f, PlayMode.StopAll);
    }

    /// <summary>
    /// 播放跑动动作
    /// </summary>
    private void _PlayRun()
    {
        curAnim = "run";
        AnimationState state = anim["run"];
        if (state == null) return;

        foreach (AnimationState temp in anim)
        {
            temp.weight = 0f;
        }
        anim.CrossFade("run", 0.1f, PlayMode.StopAll);
    }

    /// <summary>
    /// 被击动作
    /// </summary>
    private void _PlayBehit()
    {
        AnimationState state = anim["behit"];
        if (state == null) return;
        //攻击状态不能被打断
        if ("attack".Equals(curAnim)) return;

        //if (agent) agent.Stop();
        //else isStop = true;
        PlayAnim("behit");
    }

    /// <summary>
    /// 播放可打断其它的动画
    /// </summary>
    /// <param name="name"></param>
    public void PlayAnim(string name, string continueAnim = "")
    {
        if (!gameObject.activeSelf) return;

        curAnim = name;
        StopCoroutine("DoPlayAnim");
        StartCoroutine(DoPlayAnim(name, continueAnim));
    }

    IEnumerator DoPlayAnim(string animName, string continueAnim = "")
    {
        AnimationState state = anim[animName];
        state.time = 0;
        anim.Stop();
        anim.Play(animName, PlayMode.StopAll);
        while (state.normalizedTime < 1.0f)
        {
            yield return 0;
        }

        if (animName.Equals("dead"))
        {
            //_BeDead();
        }
        else if (!string.IsNullOrEmpty(continueAnim))
        {
            anim.CrossFade(continueAnim, 0.1f, PlayMode.StopAll);
        }
        else
        {
            if (isRunning)
            {
                _PlayRun();
                //if (agent) agent.Resume();
                //else isStop = false;
            }
            else
            {
                _PlayIdle();
            }
        }
    }
}
