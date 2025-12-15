/// <summary>
/// Stores the default initial values of the player components like Health and Mana.
/// </summary>
[System.Serializable]
public struct PlayerComponentsData
{
    // Health component
    public float MaxHealth;
    public float CurrentHealth;

    // Mana component
    public float MaxMana;
    public float CurrentMana;
}
