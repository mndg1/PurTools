namespace PurTools.Util
{
    internal static class Skills
    {
        internal static readonly string[] SkillNames =
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

        private static readonly Dictionary<string, int> _skillIndexes = new();

        /// <param name="skillName">Name of the skill to get the index of</param>
        /// <returns>Index of the specified skill</returns>
        internal static int GetSkillIndex(string skillName)
        {
            // Check if _skillIndexes is populated
            // If not, populate
            if (_skillIndexes.Count == 0)
                for (int i = 0; i < SkillNames.Length; i++)
                    _skillIndexes.Add(SkillNames[i], i);

            skillName = skillName.ToLower();

            if (!SkillNames.Contains(skillName))
            {
                Logger.Current.Error($"No skill with name {skillName} exists.");
                return -1;
            }

            return _skillIndexes[skillName];
        }
    }
}
