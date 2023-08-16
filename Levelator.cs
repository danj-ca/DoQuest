/// <summary>
/// Contains methods for awarding levels and treasure
/// </summary>
static class Levelator
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="character"></param>
    public static (bool, int) CheckForLevelUp(Character character)
    {
        var nextLevel = character.Level + 1;
        var leveledUp = false;
        // TODO if it's 100, need to level up the character AND retire them!
        var scoreNeeded = Database.GetScoreNeededForLevel(nextLevel);
        if (character.TotalScore >= scoreNeeded)
        {
            // TODO Like, do level up stuff!
            Database.SetCharacterLevel(character.Id, nextLevel);
            // -Activate... whatever other behaviour should accompany a level up
            leveledUp = true;
        }
        return (leveledUp, nextLevel);
    }
}