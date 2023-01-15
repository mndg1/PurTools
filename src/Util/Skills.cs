namespace PurTools.Util;

internal static class Skills
{
    internal static readonly List<string> SkillNames = new()
    {
        "overall", "attack", "defence", "strength",
        "constitution", "ranged", "prayer", "magic",
        "cooking", "woodcutting", "fletching", "fishing",
        "firemaking", "crafting", "smithing", "mining",
        "herblore", "agility", "thieving", "slayer",
        "farming", "runecrafting", "hunter", "construction",
        "summoning", "dungeoneering", "divination",
        "invention", "archaeology"
    };

    /// <param name="skillName">Name of the skill to get the index of</param>
    /// <returns>Index of the specified skill</returns>
    internal static int GetSkillIndex(string skillName)
    {
        skillName = skillName.ToLower();

        if (!SkillNames.Contains(skillName))
        {
            Logger.Current.Error($"No skill with name {skillName} exists.");
            return -1;
        }

        return SkillNames.IndexOf(skillName);
    }
}
