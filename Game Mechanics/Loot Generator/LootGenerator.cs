using UnityEngine;
using ByteSizedAttributes;

/// <summary>
/// This is a simple script where the core logic of loot prize / reward 
/// are randomly selected. The presentation of the loot is left empty for
/// the various styles of showcasing the loot to the player.
/// </summary>

[DisallowMultipleComponent]
public abstract class LootGenerator : MonoBehaviour {
    #region Variables
    [Header("Gatcha Loot Properties")]
    [Tooltip("Possible loot rewards with chances.")]
    [SerializeField] LootProperties[] lootTable = null;

    [System.Serializable]
    public class LootProperties {
        public string LootCatagory = null;
        [AbsoluteValue()] public int LootChance = 0;
        public GameObject[] LootRewards = null;
    }
    #endregion

    #region Loot Generator Functions
    /// <summary>
    /// Randomly selects loot from the provided loot table.
    /// </summary>
    /// <param name="NumberOfRewards"></param>
    /// <returns></returns>
    public GameObject[] GenerateLoot(int NumberOfRewards) {
        int MaxLootPool = 1;
        GameObject[] GeneratedLoot = null;

        // Creating a poll of numbers for the total chance of winning any loot
        for (int i = 0; i < lootTable.Length; i++) {
            MaxLootPool += lootTable[i].LootChance;
        }

        // Choosing the loot reward
        for (int x = 0; x < NumberOfRewards; x++) {
            int TierNumber = Random.Range(0, MaxLootPool);
            int SelectedCatagory = 0;
            int LastMinNumber = 0;
            int LastMaxNumber = 0;

            for (int y = 0; y < lootTable.Length; y++) {
                LastMinNumber = LastMaxNumber;
                LastMaxNumber += lootTable[y].LootChance;

                if (TierNumber > LastMinNumber && TierNumber <= LastMaxNumber) {
                    SelectedCatagory = y;
                    break;
                }
            }

            // Picks and save the selected loot reward
            int SelectedReward = Random.Range(0, lootTable[SelectedCatagory].LootRewards.Length);
            Debug.Log("The selected reward is: " + lootTable[SelectedCatagory].LootRewards[SelectedReward] + ", which is a " + lootTable[SelectedCatagory].LootCatagory + " reward catagory");
            GeneratedLoot[x] = lootTable[SelectedCatagory].LootRewards[SelectedReward];
        }

        // The saved loot reward
        return GeneratedLoot;
    }
    #endregion
}