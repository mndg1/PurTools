namespace PurTools.SkillWeek
{
    public class Participant
    {
        /// <summary>RuneScape username of the participant</summary>
        public string Name { get; set; }
        
        /// <summary>Amount of experience the participant had when joining the SkillWeek</summary>
        public int StartExperience { get; set; }
        
        /// <summary>The experience gains that the participant earned each day of the SkillWeek</summary>
        public int[] Gains { get; set; } = new int[7];

        /// <param name="threshold">The experience threshold that needs to be reached to participate succesfully</param>
        /// <returns>The amount of days that the participant has breaced the threshold</returns>
        public int GetDaysParticipated(int threshold)
        {
            int daysParticipated = 0;

            for (int i = 0; i < Gains.Length; i++)
                if (Gains[i] >= threshold) daysParticipated++;

            return daysParticipated;
        }

        /// <returns>The total amount of experience gained during the SkillWeek</returns>
        public int GetTotalGains()
        {
            int gains = Gains[0];

            for (int i = 1; i < Gains.Length; i++)
                gains += Gains[i];

            return gains;
        }
    }
}
