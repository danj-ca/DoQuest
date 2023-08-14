/// <summary>
/// Contains methods for awarding levels and treasure
/// </summary>
static class Levelator
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="score">TODO Maybe this should be passed a Character object</param>
    public static void CheckForLevelUp(Character character)
    {
        var nextLevel = character.Level + 1;
        // TODO if it's 100, need to level up the character AND retire them!
        var scoreNeeded = Database.GetScoreNeededForLevel(nextLevel);
        if (character.TotalScore >= scoreNeeded)
        {
            // TODO Like, do level up stuff!
            Database.SetCharacterLevel(character.Id, nextLevel);
            // -Activate... whatever other behaviour should accompany a level up
            // -Report all of this to the User Interface somehow
        }
    }
}