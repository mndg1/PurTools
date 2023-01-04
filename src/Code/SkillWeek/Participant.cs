namespace PurTools.SkillWeek
{
    public class Participant
    {
        /// <summary>RuneScape username of the participant</summary>
        public string Name { get; internal set; }
        
        /// <summary>Amount of experience the participant had when joining the SkillWeek</summary>
        public int StartExperience { get; internal set; }
        
        /// <summary>The experience gains that the participant earned each day of the SkillWeek</summary>
        public int[] Gains { get; internal set; } = new int[7];

        public int TotalGains => Gains.Sum();

        /// <param name="threshold">The experience threshold that needs to be reached to participate succesfully</param>
        /// <returns>The amount of days that the participant has breaced the threshold</returns>
        public int GetDaysParticipated(int threshold)
        {
            int daysParticipated = 0;

            for (int i = 0; i < Gains.Length; i++)
                if (Gains[i] >= threshold) daysParticipated++;

            return daysParticipated;
        }
    }
}
