using UnityEngine;

[System.Serializable]
public class PlanetSystem : MonoBehaviour {
    // The distance from a reference point (e.g., in kilometers, light-years, etc.)
    public int distance;
    
    // A difficulty level rating (assumed range: 1 (easiest) to 10 (hardest))
    public int difficultyLevel;
    
    // The amount of food required for the expedition
    public int foodRequired;
    
    // The amount of water required for the expedition
    public int waterRequired;
    
    // The minimum crew size required for the expedition
    public int minimumCrewSize;

    // Indicates if the planet is open to receive an expedition
    public bool open;

    /// <summary>
    /// Calculates the ratio gain based on the supplied food, water, and crew size,
    /// and adjusts this gain according to the planet's difficulty level.
    /// The base ratio is defined as the minimum of the ratios of supplied-to-required resources.
    /// The difficulty factor is calculated such that a higher difficulty level results in a lower gain.
    /// </summary>
    /// <param name="suppliedFood">The amount of food supplied</param>
    /// <param name="suppliedWater">The amount of water supplied</param>
    /// <param name="suppliedCrew">The number of crew members supplied</param>
    /// <returns>A float value representing the adjusted ratio gain</returns>
    public float GetRatioGain(int suppliedFood, int suppliedWater, int suppliedCrew) {
        // Calculate the ratio for each resource
        float foodRatio = (float)suppliedFood / foodRequired;
        float waterRatio = (float)suppliedWater / waterRequired;
        float crewRatio = (float)suppliedCrew / minimumCrewSize;
        
        // Base ratio is determined by the most limiting resource
        float baseRatio = Mathf.Min(foodRatio, waterRatio, crewRatio);
        
        // Adjust the ratio based on difficulty.
        // Using a linear adjustment factor: (11 - difficultyLevel) / 10.0f.
        float adjustmentFactor = (11 - difficultyLevel) / 10.0f;
        
        return baseRatio * adjustmentFactor;
    }

    /// <summary>
    /// Generates a brief description of the planet, including all its properties.
    /// </summary>
    /// <returns>A string description of the planet.</returns>
    public string GetDescription() {
        return string.Format("Planet Description:\n" +
                             "Distance: {0} units\n" +
                             "Difficulty Level: {1}\n" +
                             "Food Required: {2}\n" +
                             "Water Required: {3}\n" +
                             "Minimum Crew Size: {4}\n" +
                             "Open for Expedition: {5}",
                             distance,
                             difficultyLevel,
                             foodRequired,
                             waterRequired,
                             minimumCrewSize,
                             open ? "Yes" : "No");
    }
}
