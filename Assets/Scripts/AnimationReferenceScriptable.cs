using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

[CreateAssetMenu(fileName = "New Anim", menuName = "Anim Reference")]
public class AnimationReferenceScriptable : ScriptableObject
{
    public AnimationReferenceAsset elus, kotor, makan, mandi, normal, sakit, salah, sedih, suap, bump, jumpUp, jumpDown, landing, run;
    public SkeletonDataAsset dataAssetIdle, dataAssetRun;
}
