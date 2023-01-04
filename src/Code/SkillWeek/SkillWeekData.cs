namespace PurTools.SkillWeek
{
    public class SkillWeekData
    {
        /// <summary>Name of the skill that the SkillWeek is about</summary>
        public string SkillName { get; internal set; } = "Overall";
        
        /// <summary>Amount of days that the SkillWeek has been active for</summary>
        public int DaysActive { get; internal set; }

        /// <summary>A dictionary holding the participants with their usernames as keys</summary>
        public Dictionary<string, Participant> Participants { get; internal set; } = new Dictionary<string, Participant>();
    }
}
