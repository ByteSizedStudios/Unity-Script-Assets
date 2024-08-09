using UnityEngine;
using ByteSizedAttributes;

/// <summary>
/// This script allows for the manipulation of player or game stats,
/// based on the level of the modifier. The modifier allows for a buff
/// or de-buff like affect on any given numerical stats. 
/// 
/// The script is similar to game mechancis like mods, stat based 
/// abilites, and upgrades.
/// </summary>

public class StatModifier : MonoBehaviour {
    #region Variables
    public enum ModifyType { Additive, Percentage };
    public enum StatType { Buff, Debuff };

    [Header("Stat Modifier Properties")]
    [Tooltip("Name of the stat modifier.")]
    [SerializeField] string modifierName = null;
    [Tooltip("Limited levels or number of upgrades for the modifier")]
    [SerializeField] bool limitedModifierLevels = true;
    [Tooltip("The max level or number of upgrades for the modifier, limit is set to 20.")]
    [SerializeField] [ConditionalHide("limitedModifierLevels", true)] int maxModifierLevel = 1;
    [Space(5)] [LineDivider(4, color: LineColors.Black)]
    [Tooltip("Number of stats to be modified and its properties.")]
    [SerializeField] StatProperties[] statProperties = new StatProperties[1];

    [System.Serializable]
    public class StatProperties
    {
        [Tooltip("Stat buff or debuff.")]
        public StatType statType = StatType.Buff;
        [Tooltip("Type of stat modifier.")]
        public ModifyType modifierType = ModifyType.Additive;
        [Tooltip("Stat value at level 0.")]
        [AbsoluteValue] public float baseStatValue = 0;
        [Tooltip("How much the stat will increase by.")]
        [AbsoluteValue] public float increaseStatBy = 0;
        [Tooltip("Purpose of stat.")]
        public string statDescription = null;
    }

    [ExecuteInEditMode]
    private void OnValidate()
    {
        if (maxModifierLevel < 1)
        {
            maxModifierLevel = 1;
        }
        else if (maxModifierLevel > 20 && limitedModifierLevels)
        {
            maxModifierLevel = 20;
        }

        if (statProperties.Length != 0)
        {
            foreach (StatProperties stats in statProperties)
            {
                if (stats.baseStatValue < 0)
                {
                    stats.baseStatValue = 0;
                }
                if (stats.increaseStatBy < 0)
                {
                    stats.increaseStatBy = 0;
                }
            }
        }
    }

    // Private variables
    int CurrentLevel = 0;
    float[] CurrentStatValues;

    // String variable names used for saving and loading stat data
    string StatLevel_ID;
    string[] BaseStat_ID;
    string[] IncreaseStat_ID;
    [HideInInspector] protected string[] FinalStatDescription;
    #endregion

    // Start is called before the first frame update
    void Start() {
        StatLevel_ID = modifierName + "_CurrentLevel";

        BaseStat_ID = new string[statProperties.Length];
        for (int i = 0; i < statProperties.Length; i++) {
            BaseStat_ID[i] = modifierName + "_Stat_" + i;
        }
        IncreaseStat_ID = new string[statProperties.Length];
        for (int i = 0; i < statProperties.Length; i++) {
            IncreaseStat_ID[i] = modifierName + "_IncreaseStat_" + i;
        }
    }

    #region Stat Modifier Functions
    /// <summary>
    /// Level up modifier by the amount indicated by the user
    /// </summary>
    /// <param name="NumOfUpgrades"></param>
    public void UpgradeModifier(int NumOfUpgrades) {
        CurrentLevel += NumOfUpgrades;

        // Check to see if upgrade went over the limit or under zero
        if (CurrentLevel > maxModifierLevel && limitedModifierLevels) {
            CurrentLevel = maxModifierLevel;
        }
        else if (CurrentLevel < 0) {
            CurrentLevel = 0;
        }

        // Update stats based on the current level of modifier
        CurrentStatValues = new float[statProperties.Length];
        for (int i = 0; i < statProperties.Length; i++) {
            CurrentStatValues[i] = statProperties[i].baseStatValue + (CurrentLevel * statProperties[i].increaseStatBy);
        }
    }

    /// <summary>
    /// Clear all data relating to the stat modifier
    /// </summary>
    public void ClearStats() {
        // Check if there is something to clear
        if (!PlayerPrefs.HasKey(StatLevel_ID)) {
            return;
        }

        // Delete the player prefs
        PlayerPrefs.DeleteKey(StatLevel_ID);
        for (int i = 0; i < statProperties.Length; i++) {
            PlayerPrefs.DeleteKey(BaseStat_ID[i]);
        }
        for (int i = 0; i < statProperties.Length; i++) {
            PlayerPrefs.DeleteKey(IncreaseStat_ID[i]);
        }
    }

    /// <summary>
    /// Returns the final stat values
    /// </summary>
    /// <returns></returns>
    protected float[] StatResults() {
        // Container for all stats output value
        float[] FinalStatResults = new float[statProperties.Length];
        FinalStatDescription = new string[statProperties.Length];

        // Determine final stat value as a buff or debuff
        for (int i = 0; i < statProperties.Length; i++) {
            switch (statProperties[i].statType) {
                case StatType.Buff:
                    FinalStatResults[i] = Mathf.Abs(CurrentStatValues[i]);
                    FinalStatDescription[i] = statProperties[i].statDescription + " +" + FinalStatResults[i];
                    if (statProperties[i].modifierType == ModifyType.Percentage) {
                        FinalStatResults[i] /= 100;
                        FinalStatDescription[i] = FinalStatDescription[i] + "%";
                    }
                    break;
                case StatType.Debuff:
                    FinalStatResults[i] = (Mathf.Abs(CurrentStatValues[i]) * -1f);
                    FinalStatDescription[i] = statProperties[i].statDescription + " " + FinalStatResults[i];
                    if (statProperties[i].modifierType == ModifyType.Percentage) {
                        FinalStatResults[i] /= 100;
                        FinalStatDescription[i] = FinalStatDescription[i] + "%";
                    }
                    break;
            }
        }

        return FinalStatResults;
    }

    /// <summary>
    /// Saves any data relating to the stat modifier
    /// </summary>
    protected void SaveStats() {
        PlayerPrefs.SetInt(StatLevel_ID, CurrentLevel);

        // Only store the base stat and incrementation
        for (int i = 0; i < statProperties.Length; i++) {
            PlayerPrefs.SetFloat(BaseStat_ID[i], statProperties[i].baseStatValue);
        }
        for (int i = 0; i < statProperties.Length; i++) {
            PlayerPrefs.SetFloat(IncreaseStat_ID[i], statProperties[i].baseStatValue);
        }
    }

    /// <summary>
    /// Loads any data relating to the stat modifier
    /// </summary>
    protected void LoadStats() {
        // Check if there is something to load
        if (!PlayerPrefs.HasKey(StatLevel_ID)) {
            return;
        }

        else {
            CurrentLevel = PlayerPrefs.GetInt(StatLevel_ID);

            // Only get the base stat and incrementation
            for (int i = 0; i < statProperties.Length; i++) {
                statProperties[i].baseStatValue = PlayerPrefs.GetFloat(BaseStat_ID[i]);
            }
            for (int i = 0; i < statProperties.Length; i++) {
                statProperties[i].baseStatValue = PlayerPrefs.GetFloat(IncreaseStat_ID[i]);
            }
        }
    }

    /// <summary>
    /// Returns the current level of the stat modifier.
    /// </summary>
    /// <returns></returns>
    public int GetCurrentLevel() { 
        return CurrentLevel;   
    }
    #endregion
}