using UnityEngine;

public class Upgrade : ScriptableObject
{
    [Header("Informations")]
    public string Name;
    public string Description;
    public bool HasBeenBought;

    [Header("Requirements")]
    public string Cost;
    public int RequiredLevel;

    [Header("Bonus")]
    public UpgradeType Type;
    public float Value;

    [Header("Look")]
    public Sprite Image;


    public Upgrade GetCopy()
    {
        return this.MemberwiseClone() as Upgrade;
    }
}


public enum UpgradeType
{
    BaseIncomeMultiplier = 0,

}
