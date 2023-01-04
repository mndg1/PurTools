using System.Configuration;
using System.Text.Json;
using PurTools.Util;

namespace PurTools.SkillWeek
{
    public class SkillWeek
    {
        private static string DefaultDirectory
        {
            get
            {
                var dir = ConfigurationManager.AppSettings["skillWeekDirectory"];

                if (string.IsNullOrEmpty(dir))
                    dir = Path.Combine(Directory.GetCurrentDirectory(), "SkillWeek");

                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                return dir;
            }
        }

        private readonly string _label;
        public string Label { get => _label; }

        private SkillWeekData _skillWeekData = new();

        internal SkillWeek(string label) => _label = label;

        /// <summary>
        /// Creates a new SkillWeek object
        /// </summary>
        /// <param name="skillName">Name of the SkillWeek's subject skill</param>
        /// <param name="label">Optional custom name for the SkillWeek</param>
        /// <param name="entryThreshold">The minimum amount of experience that participants must earn each day
        ///                              in order to have particpated</param>
        /// <returns>Newly created SkillWeek object</returns>
        public static async Task<SkillWeek> CreateAsync(string skillName, string label = "") 
            => await CreateAsync(skillName, DefaultDirectory, label);

        /// <summary>
        /// Creates a new SkillWeek object
        /// </summary>
        /// <param name="skillName">Name of the SkillWeek's subject skill</param>
        /// <param name="directory">Optional directory path where the SkillWeek data should be stored</param>
        /// <param name="label">Optional custom name for the SkillWeek</param>
        /// <param name="entryThreshold">The minimum amount of experience that participants must earn each day
        ///                              in order to have particpated</param>
        /// <returns>Newly created SkillWeek object</returns>
        public static async Task<SkillWeek> CreateAsync(string skillName, string directory, string label)
        {
            // Checks if a skill with skillName exists
            if (Skills.GetSkillIndex(skillName) == -1)
            {
                Logger.Current.Error($"{skillName} is not a valid skill.");
                throw new ArgumentException();
            }

            if (string.IsNullOrWhiteSpace(label))
                label = skillName;

            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
            else label = Utility.GetUniqueFileLabel(directory, ".json", label);

            var skillWeek = new SkillWeek(label);
            skillWeek._skillWeekData.SkillName = skillName;
            await skillWeek.SaveAsync();

            return skillWeek;
        }

        /// <summary>
        /// Loads an existing SkillWeek object
        /// </summary>
        /// <param name="label">Name of the SkillWeek to load</param>
        /// <returns>SkillWeek loaded with the saved data</returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static async Task<SkillWeek> LoadAsync(string label) => await LoadAsync(label, DefaultDirectory);

        /// <summary>
        /// Loads an existing SkillWeek object
        /// </summary>
        /// <param name="label">Name of the SkillWeek to load</param>
        /// <param name="directory">Directory in which the SkillWeek's data is stored</param>
        /// <returns>SkillWeek loaded with the saved data</returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static async Task<SkillWeek> LoadAsync(string label, string directory)
        {
            string path = Path.Combine(directory, $"{label}.json");

            if(!Directory.Exists(directory))
            {
                Logger.Current.Error($"Directory \"{directory}\" does not exits.");
                throw new DirectoryNotFoundException();
            }

            if (!File.Exists(path))
            {
                Logger.Current.Error($"No skillweek with label \"{label}\" was found.");
                throw new FileNotFoundException();
            }

            var skillWeek = new SkillWeek(label);
            await skillWeek.LoadFromFileAsync(path);

            return skillWeek;
        }

        /// <summary>
        /// Changes the default directory in which SkillWeek data is stored
        /// </summary>
        /// <param name="newPath">Path to the new directory</param>
        /// <param name="migrate">Whether or not to move the old default directory and its contents</param>
        public static void ChangeDefaultDirectory(string newPath, bool migrate = true)
        {
            if (migrate)
                Directory.Move(DefaultDirectory, newPath);

            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Remove("skillWeekDirectory");
            config.AppSettings.Settings.Add("skillWeekDirectory", newPath);
            config.Save();
            ConfigurationManager.RefreshSection("appSettings");
        }

        /// <summary>
        /// Load SkillWeek data from save file
        /// </summary>
        /// <param name="path">Path to the SkillWeek save file</param>
        /// <exception cref="Exception"></exception>
        private async Task LoadFromFileAsync(string path)
        {
            using var stream = File.OpenRead(path);

            if (stream.Length == 0)
            {
                string error = $"{path} is an empty file. The skillweek could not be loaded.";
                Logger.Current.Error(error);
                throw new Exception(error);
            }

            var deserialzeTask = await JsonSerializer.DeserializeAsync<SkillWeekData>(stream);

            if (deserialzeTask == null)
            {
                string error = $"Something went wrong whilst loading {path}.";
                Logger.Current.Error(error);
                throw new Exception(error);
            }

            _skillWeekData = deserialzeTask;
        }

        /// <summary>
        /// Saves the SkillWeek's data to a JSON file
        /// </summary>
        public async Task SaveAsync() => await SaveAsync(DefaultDirectory);

        /// <summary>
        /// Saves the SkillWeek's data to a JSON file
        /// </summary>
        public async Task SaveAsync(string directory)
        {
            if(!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            using var writer = File.Create(Path.Combine(directory, $"{_label}.json"));
            await JsonSerializer.SerializeAsync(writer, _skillWeekData);
            await writer.DisposeAsync();
        }

        /// <summary>
        /// Adds a participant to the SkillWeek
        /// </summary>
        /// <param name="name">RuneScape username of the participant</param>
        public async Task AddPlayerAsync(string name)
        {
            if (_skillWeekData.Participants.ContainsKey(name.ToLower()))
            {
                Console.WriteLine($"{name} is already tracked");
                return;
            }

            // Get data from RuneScape's HiScoresLite API
            string[]? stats = await RuneScapeAPIs.HiScoresLite(name);

            // Stats can be null if the API call failed
            // There is no need to log an error or message as that is done inside the RuneScapeAPIs.HiScoresLite() method
            if (stats == null)
                return;

            // skillData structure: [ global rank, skill level, experience ]
            string[] skillData = stats[Skills.GetSkillIndex(_skillWeekData.SkillName)].Split(",");
            int experience = Convert.ToInt32(skillData[2]);
            var participant = new Participant();

            participant.Name = name;
            participant.StartExperience = experience;

            _skillWeekData.Participants.Add(name.ToLower(), participant);
        }

        /// <summary>
        /// Updates SkillWeek scores for all participants
        /// </summary>
        public async Task UpdateAllAsync()
        {
            foreach (var participant in _skillWeekData.Participants.Keys)
                await UpdateParticipantAsync(participant);
        }

        /// <summary>
        /// Update SkillWeek scores for a specific participant
        /// </summary>
        /// <param name="name">RuneScape username of the participant</param>
        public async Task UpdateParticipantAsync(string name)
        {
            if (!_skillWeekData.Participants.TryGetValue(name.ToLower(), out var participant))
                return;

            // Get data from RuneScape's HiScoresLite API
            string[]? stats = await RuneScapeAPIs.HiScoresLite(name);

            // Stats can be null if the API call failed
            // There is no need to log an error or message as that is done inside the RuneScapeAPIs.HiScoresLite() method
            if (stats == null)
                return;

            // skillData structure: [ global rank, skill level, experience ]
            string[] skillData = stats[Skills.GetSkillIndex(_skillWeekData.SkillName)].Split(",");
            int experience = Convert.ToInt32(skillData[2]);
            int gains = experience - (participant.StartExperience + participant.TotalGains);
            participant.Gains[_skillWeekData.DaysActive] += gains;
        }

        /// <summary>
        /// Increases the SkillWeek's active day by one
        /// </summary>
        public void IncrementDay() => SetDay(_skillWeekData.DaysActive + 1);

        /// <summary>
        /// Set the SkillWeek's active day to a specific day
        /// </summary>
        /// <param name="day">Number of the active day</param>
        public void SetDay(int day) => _skillWeekData.DaysActive = day;

        /// <returns>Dictionary of all participants with their RuneScape usernames as keys</returns>
        public Dictionary<string, Participant> GetParticipants()
        {
            // New dictionary to copy values into so the original cannot be changed from outside the SkillWeek
            var participants = new Dictionary<string, Participant>();

            foreach (var participant in _skillWeekData.Participants)
                participants.Add(participant.Key, participant.Value);

            return participants;
        }
    }
}
