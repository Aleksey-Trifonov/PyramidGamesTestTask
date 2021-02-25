using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettingsScriptableObject", order = 1)]
public class GameSettings : ScriptableObject
{
    public float GolfHoleXOffset = 1.5f;
    public float GolfBallXVelocityIncrementRate = 1.2f;
    public float GolfBallYVelocityIncrementRate = 1f;
    public float ConsecutiveWinVelocityModifier = 0.1f;
    public int MaxNumberOfBounces = 3;
    [Tooltip("In seconds")] 
    public float GolfBallLifetime = 3f;
}
