using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpineUIAnimationWefie : MonoBehaviour
{
    public SkeletonGraphic skeletonGraphic;
    public AnimationReferenceAsset normal, sakit, salah;
    public string currentAnimation;
    public bool initialized = false;
    private int catID = 1;
    private bool skinSet = false;
    public Transform skeletonObj;

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
        initialized = false;
        skinSet = false;
        foreach (Transform child in transform)
        {
            skeletonGraphic = child.GetComponent<SkeletonGraphic>();
            if (skeletonGraphic != null)
            {
                skeletonObj = child;
                break;
            }
        }
    }

    void Update()
    {
        if (!initialized)
        {
            try
            {
                catID = GameManager.instance.CatProfile.catScriptable.id;
                RenewAnimationReference(GameManager.instance.CatProfile.catScriptable.phase);
            }
            catch (NullReferenceException ex)
            {
                //Debug.LogWarning("CatScriptable is null: " + ex.Message);
            }
            catch (Exception ex)
            {
                //Debug.LogError("An error occurred: " + ex.Message);
            }
        }
        if (initialized && !skinSet)
        {
            SetSkin(catID.ToString());
            skinSet = true;
        }
    }

    public void PlayAnimation(AnimationReferenceAsset animation, bool loop, float timescale, string skinName = "")
    {
        if (animation != null)
        {
            if (animation.name.Equals(currentAnimation)) return;
            skeletonGraphic.Skeleton.SetToSetupPose();
            skeletonGraphic.AnimationState.ClearTracks();
            skeletonGraphic.AnimationState.SetAnimation(0, animation, loop);
            skeletonGraphic.AnimationState.TimeScale = timescale;

            // Set the skin (if specified)
            if (!string.IsNullOrEmpty(skinName))
            {
                skeletonGraphic.Skeleton.SetSkin(skinName);
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
        skeletonGraphic.AnimationState.ClearTracks();
    }

    public void SetSkin(string skinName)
    {
        if (!string.IsNullOrEmpty(skinName))
        {
            skeletonGraphic.Skeleton.SetSkin(skinName);
            // Debug.Log("skin sekarang: " + skinName);
        }
        else
        {
            // Debug.LogWarning("Skin not found: " + skinName);
        }
    }

    public float posYBaby;
    public float posYChild;
    public float posYAdult;

    public void RenewAnimationReference(CatPhase phase)
    {
        AnimationReferenceScriptable animationReferenceScriptable;
        animationReferenceScriptable = Resources.Load<AnimationReferenceScriptable>("AnimationReferenceScriptable/" + phase);
        if (animationReferenceScriptable != null)
        {
            skeletonGraphic.skeletonDataAsset = animationReferenceScriptable.dataAssetIdle;
            normal = animationReferenceScriptable.normal;
            sakit = animationReferenceScriptable.sakit;
            salah = animationReferenceScriptable.salah;
            InitializeTheSkeleton();
            float posY = -222;
            if (phase == CatPhase.Baby)
            {
                posY = posYBaby;
            }
            else if (phase == CatPhase.Child)
            {
                posY = posYChild;
            }
            else if (phase == CatPhase.Adult)
            {
                posY = posYAdult;
            }
            skeletonGraphic.rectTransform.anchoredPosition = new Vector2(skeletonGraphic.rectTransform.anchoredPosition.x, posY);
        }
        else
        {
            Debug.LogError("Animation Not Found");
        }
    }

    public void InitializeTheSkeleton()
    {
        skeletonGraphic.Initialize(true);
        skeletonGraphic.Initialize(true);
        initialized = true;
    }
}
