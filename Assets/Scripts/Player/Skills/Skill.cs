using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    protected enum SkillState
    {
        Available,
        Regenerating
    }
    [SerializeField] protected float totalSkillPoints;
    protected float CurrentSkillPoints;

    [SerializeField] protected float regenerationPoints;
    [SerializeField] protected float regenerationRate;
    [SerializeField] protected SkillIcon skillIcon;
    protected SkillState State = SkillState.Available;

    protected void AddSkillPoints(float points)
    {
        points = Mathf.Abs(points);
        CurrentSkillPoints = Mathf.Min(CurrentSkillPoints + points, totalSkillPoints);
    }
    
    protected void RemoveSkillPoints(float points)
    {
        points = Mathf.Abs(points);
        CurrentSkillPoints = Mathf.Max(CurrentSkillPoints - points, 0);
    }
}
