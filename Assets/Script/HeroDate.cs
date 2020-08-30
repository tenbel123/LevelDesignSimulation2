
using UnityEngine;

[CreateAssetMenu(fileName = "New Hero", menuName = "ScriptableObject/Create Hero")]

public class HeroDate : ScriptableObject
{
   public string Job = "New Job";
   public float HP;
   public float ATK; 
    public float movementSpeed;
    public float HPGrowthValue;
    public float ATKGrowthValue;
    public float movementSpeedGrowthValue;
   

}
