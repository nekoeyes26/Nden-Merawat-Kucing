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
        if (skeletonGraphic == null)
        {
            foreach (Transform child in transform)
            {
                skeletonGraphic = child.GetComponent<SkeletonGraphic>();
                if (skeletonGraphic != null)
                {
                    break;
                }
            }
            if (skeletonGraphic == null)
            {
                Debug.LogError("SkeletonGraphic not found!");
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

    public Vector2 posBaby;
    public Vector2 posChild;
    public Vector2 posAdult;

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
            Vector2 pos;
            if (phase == CatPhase.Baby)
            {
                pos = posBaby;
            }
            else if (phase == CatPhase.Child)
            {
                pos = posChild;
            }
            else if (phase == CatPhase.Adult)
            {
                pos = posAdult;
            }
            else
            {
                Debug.LogError("Invalid CatPhase");
                return;
            }
            skeletonGraphic.rectTransform.anchoredPosition = pos;
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
