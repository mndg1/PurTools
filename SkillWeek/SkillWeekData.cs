namespace PurTools.SkillWeek
{
    public class SkillWeekData
    {
        /// <summary>Name of the skill that the SkillWeek is about</summary>
        public string SkillName { get; set; } = "Overall";
        
        /// <summary>Amount of experience a participant needs to gain in order to gain entries</summary>
        public int EntryThreshold { get; set; }

        /// <summary>Amount of days that the SkillWeek has been active for</summary>
        public int DaysActive { get; set; }

        /// <summary>A dictionary holding the participants with their usernames as keys</summary>
        public Dictionary<string, Participant> Participants { get; set; } = new Dictionary<string, Participant>();
    }
}
