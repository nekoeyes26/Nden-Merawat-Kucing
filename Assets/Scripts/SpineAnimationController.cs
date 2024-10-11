using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using UnityEngine.SceneManagement;

public class SpineAnimationController : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset elus, makan, mandi, normal, sakit, salah, suap, bump, jumpUp, jumpDown, landing, run;
    public string currentAnimation;
    public bool isLandingPlaying = false;

    void OnEnable()
    {
        GameEvents.OnPhaseChange += RenewAnimationReference;

    }
    void OnDisable()
    {
        GameEvents.OnPhaseChange -= RenewAnimationReference;
    }
    void Start()
    {
        skeletonAnimation.state.Complete += OnLandingComplete;
        RenewAnimationReference(GameManager.instance.CatProfile.phase);
    }

    void OnDestroy()
    {
        skeletonAnimation.state.Complete -= OnLandingComplete;
    }

    public void PlayAnimation(AnimationReferenceAsset animation, bool loop, float timescale, string skinName = "")
    {
        if (animation != null)
        {
            if (animation.name.Equals(currentAnimation)) return;
            skeletonAnimation.state.ClearTracks();
            skeletonAnimation.state.SetAnimation(0, animation, loop);
            skeletonAnimation.state.TimeScale = timescale;
            if (animation == landing)
            {
                isLandingPlaying = true;
            }

            // Set the skin (if specified)
            if (!string.IsNullOrEmpty(skinName))
            {
                skeletonAnimation.Skeleton.SetSkin(skinName);
                // Debug.Log("skin sekarang: " + skinName);
            }
            else
            {
                // Debug.LogWarning("Skin not found: " + skinName);
            }
            currentAnimation = animation.name;
        }
        else
        {
            Debug.LogError("Animation not found");
        }
    }

    public void StopAnimation()
    {
        skeletonAnimation.state.ClearTracks();
        isLandingPlaying = false;
    }

    public void SetSkin(string skinName)
    {
        if (!string.IsNullOrEmpty(skinName))
        {
            skeletonAnimation.Skeleton.SetSkin(skinName);
            // Debug.Log("skin sekarang: " + skinName);
        }
        else
        {
            // Debug.LogWarning("Skin not found: " + skinName);
        }
    }

    private void OnLandingComplete(Spine.TrackEntry trackEntry)
    {
        if (currentAnimation == landing.name)
        {
            isLandingPlaying = false;
        }
    }

    public void RenewAnimationReference(CatPhase phase)
    {
        AnimationReferenceScriptable animationReferenceScriptable;
        animationReferenceScriptable = Resources.Load<AnimationReferenceScriptable>("AnimationReferenceScriptable/" + phase);
        if (animationReferenceScriptable != null)
        {
            if (SceneManager.GetActiveScene().name == "Playing")
            {
                skeletonAnimation.skeletonDataAsset = animationReferenceScriptable.dataAssetRun;
            }
            else
            {
                skeletonAnimation.skeletonDataAsset = animationReferenceScriptable.dataAssetIdle;
            }
            elus = animationReferenceScriptable.elus;
            makan = animationReferenceScriptable.makan;
            mandi = animationReferenceScriptable.mandi;
            normal = animationReferenceScriptable.normal;
            sakit = animationReferenceScriptable.sakit;
            salah = animationReferenceScriptable.salah;
            suap = animationReferenceScriptable.suap;
            bump = animationReferenceScriptable.bump;
            jumpUp = animationReferenceScriptable.jumpUp;
            jumpDown = animationReferenceScriptable.jumpDown;
            landing = animationReferenceScriptable.landing;
            run = animationReferenceScriptable.run;
        }
        else
        {
            Debug.LogError("Animation Not Found");
        }
    }

    public void InitializeTheSkeleton()
    {
        skeletonAnimation.Initialize(true);
    }

    public IEnumerator InitializeSkeletonAfterDelay()
    {
        yield return new WaitForSeconds(0.2f);
        skeletonAnimation.Initialize(true);
    }
}
