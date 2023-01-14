namespace PurTools;

internal static class RuneScapeAPIs
{
    private static readonly HttpClient _httpClient = new();

    /// <param name="rsn">RuneScape username of the player to get the data from</param>
    /// <returns>Data from RuneScape's HiScoresLite API</returns>
    internal static async Task<string[]?> HiScoresLite(string rsn)
    {
        string url = $"https://secure.runescape.com/m=hiscore/index_lite.ws?player={rsn}";
        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            Logger.Current.Error($"Failed api call for {rsn}");
            return null;
        }

        // Data divides data with the new line character between each skill and activity
        var reader = new StreamReader(response.Content.ReadAsStream());
        return reader.ReadToEnd().Split('\n');
    }
}
