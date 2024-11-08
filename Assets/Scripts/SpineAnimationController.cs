using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpineAnimationController : MonoBehaviour
{
    public static SpineAnimationController instance { get; private set; }
    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset elus, kotor, makan, mandi, normal, sakit, salah, sedih, suap, bump, jumpUp, jumpDown, landing, run;
    public string currentAnimation;
    public bool isLandingPlaying = false;
    public bool isMakanPlaying = false;
    public bool isBumpPlaying = false;
    public bool initialized = false;
    private int catID = 1;
    private bool skinSet = false;
    private float originalTimeScale;
    private bool freezed = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void OnEnable()
    {
        // skeletonAnimation.state.Complete += OnAnimationComplete;
        GameEvents.OnPhaseChange += RenewAnimationReference;
        // skeletonAnimation.state.Complete += OnAnyAnimationComplete;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        if (skeletonAnimation != null)
        {
            skeletonAnimation.state.Complete -= OnAnimationComplete;
        }
        GameEvents.OnPhaseChange -= RenewAnimationReference;
        // skeletonAnimation.state.Complete -= OnAnyAnimationComplete;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void Start()
    {

    }

    void Update()
    {
        if (initialized && !skinSet)
        {
            SetSkin(catID.ToString());
            skinSet = true;
        }
        if (initialized && skinSet && changePos)
        {
            ChangeTransformPos(GameManager.instance.CatProfile.catScriptable.phase);
        }
    }

    public void PlayAnimation(AnimationReferenceAsset animation, bool loop, float timescale, string skinName = "")
    {
        if (freezed) return;
        if (animation != null)
        {
            if (animation.name.Equals(currentAnimation)) return;
            skeletonAnimation.state.ClearTracks();
            skeletonAnimation.state.SetAnimation(0, animation, loop);
            skeletonAnimation.state.TimeScale = timescale;

            originalTimeScale = timescale;

            if (animation == landing)
            {
                isLandingPlaying = true;
            }
            if (animation == bump)
            {
                isBumpPlaying = true;
            }
            if (animation == makan || animation == salah)
            {
                isMakanPlaying = true;
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
        isMakanPlaying = false;
    }

    public void FreezeAnimation()
    {
        // Check if there's a current animation playing
        //if (!string.IsNullOrEmpty(currentAnimation))
        //{
        // Freeze the animation by setting the time scale to zero
        skeletonAnimation.state.TimeScale = 0f;
        freezed = true;
        //Debug.Log("Animation frozen: " + currentAnimation);
        //}
        //else
        //{
        //    Debug.LogWarning("No animation is currently playing to freeze.");
        //}
    }

    public void UnfreezeAnimation()
    {
        // Check if there's a current animation playing
        //if (!string.IsNullOrEmpty(currentAnimation))
        //{
        // Restore the original time scale to resume the animation
        skeletonAnimation.state.TimeScale = originalTimeScale;
        freezed = false;
        //Debug.Log("Animation unfrozen: " + currentAnimation);
        //}
        //else
        //{
        //    Debug.LogWarning("No animation is currently frozen to unfreeze.");
        //}
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

    private void OnAnimationComplete(Spine.TrackEntry trackEntry)
    {
        Debug.Log("Animation completed: " + trackEntry.Animation.Name);
        if (trackEntry.Animation.Name == landing.name)
        {
            isLandingPlaying = false;
        }
        if (trackEntry.Animation.Name == bump.name)
        {
            isBumpPlaying = false;
        }
        if (trackEntry.Animation.Name.Equals(makan.name) || trackEntry.Animation.Name.Equals(salah.name))
        {
            isMakanPlaying = false;
        }
    }

    private void OnAnyAnimationComplete(Spine.TrackEntry trackEntry)
    {
        Debug.Log("Any animation completed: " + trackEntry.Animation.Name);
    }

    public void RenewAnimationReference(CatPhase phase)
    {
        freezed = false;
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
                // Debug.Log("IDLE LOAD");
            }
            elus = animationReferenceScriptable.elus;
            kotor = animationReferenceScriptable.kotor;
            makan = animationReferenceScriptable.makan;
            mandi = animationReferenceScriptable.mandi;
            normal = animationReferenceScriptable.normal;
            sakit = animationReferenceScriptable.sakit;
            salah = animationReferenceScriptable.salah;
            sedih = animationReferenceScriptable.sedih;
            suap = animationReferenceScriptable.suap;
            bump = animationReferenceScriptable.bump;
            jumpUp = animationReferenceScriptable.jumpUp;
            jumpDown = animationReferenceScriptable.jumpDown;
            landing = animationReferenceScriptable.landing;
            run = animationReferenceScriptable.run;
            InitializeTheSkeleton();
        }
        else
        {
            Debug.LogError("Animation Not Found");
        }
    }

    public void InitializeTheSkeleton()
    {
        skeletonAnimation.Initialize(true);
        skeletonAnimation.Initialize(true);
        initialized = true;

        skeletonAnimation.state.Complete += OnAnimationComplete;
        // skeletonAnimation.state.Complete += OnAnyAnimationComplete;
    }

    public IEnumerator InitializeSkeletonAfterDelay()
    {
        yield return new WaitForSeconds(0.2f);
        skeletonAnimation.Initialize(true);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "ChoosingCat" || scene.name == "CoverTitle") return;
        if (scene.name == "Feeding") changePos = true; else changePos = false;
        skeletonAnimation = FindObjectOfType<SkeletonAnimation>();
        catID = GameManager.instance.CatProfile.catScriptable.id;
        initialized = false;
        skinSet = false;
        if (skeletonAnimation != null)
        {
            if (GameManager.instance != null)
            {
                RenewAnimationReference(GameManager.instance.CatProfile.catScriptable.phase);
            }
            else
            {
                InitializeTheSkeleton();
            }
        }
    }

    public bool changePos = false;
    public float posYBaby = -1.52f;
    public float posYChild = -2.55f;
    public float posYAdult = -2.95f;
    private void ChangeTransformPos(CatPhase phase)
    {
        float posY = -1.52f;
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
        skeletonAnimation.transform.position = new Vector3(skeletonAnimation.transform.position.x, posY, skeletonAnimation.transform.position.z);
    }
}
