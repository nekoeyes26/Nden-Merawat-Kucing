using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Requirement", menuName = "Requirement")]
public class RequirementScriptable : ScriptableObject
{
    public int toLevel;
    public int hungry;
    public int shower;
    public int play;
    public int photo;
}
