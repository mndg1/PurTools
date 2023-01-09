# PurTools
PurTools is a project I made and maintain in order to provide useful functionalities for our RuneScape gameplay. 
The idea was formed around an in game event we wanted to host and organize for our RuneScape clan: Pur Perfection.

## What does this project provide?
This project provides various functions that are used by me and my friends in order to make our time in RuneScape more fun. 
In order to use the provided functions, an implementation using this library must be written. 
This can be anything like a console application, Discord bot, or website for example.

As this project is still subject to be expanded, the different sets of functionalities will be divided into modules.

## SkillWeek
SkillWeek is an event that we organize in RuneScape. 
During the weeklong event, our clan members are encouraged to gain experience points in a specific skill for the duration of the event.

### Creation
The SkillWeek module provides methods for keeping track of the experience gains of all the participants. To get started, the `SkillWeek.CreateAsync(string)` must be used. This method requires at minimum a parameter `string skillName` which is the name of the skill that the SkillWeek should track. Additionally, a `string label`, the name that the SkillWeek will go by, and a `string directory`, the directory where the SkillWeek's data should be stored, can be passed as parameters.

The following method allows the caller to create a SkillWeek object by passing in the `skillName` as a parameter. 
Optionally, a `label` can be provided to give the SkillWeek object a specific name.
The method may return `null` if the provided `skillName` is not an existing skill.
```
public static async Task<SkillWeek?> CreateAsync(string skillName, string label = "") { ... }
```

The following method is a more detailed version of the aforementioned method. 
The caller needs to provide the `skillName`, a specific directory to store the data file, and a `label` as the name of the SkillWeek object.
Again, the method may return `null` if the `skillName` is not an existing skill.
```
public static async Task<SkillWeek?> CreateAsync(string skillName, string directory, string label) { ... }
```

### Loading and saving
In order to load existing SkillWeek data into a SkillWeek object the `SkillWeek.LoadAsync(string)` method may be used. 
The method takes a `string label` as a parameter. The `label` is the same as the name of the save file. 
When applicable a `string directory` may also be passed as a parameter if the file was created to have a unique directory.

The following method allows the caller to load a SkillWeek object with pre-existing data.
The method may return `null` if at any point during loading the data could not be found. 
This could mean that either the file or directory does not exist, or that the given file has no data.
```
public static async Task<SkillWeek?> LoadAsync(string label) { ... }
```

The following method allows loading from a specified directory. The directory should not include the file name.
Similarly, the method may return `null` if at any point the data could not be loaded.
```
public static async Task<SkillWeek?> LoadAsync(string label, string directory) { ... }
```

Saving the SkillWeek object's data is done by calling the `SkillWeek.SaveAsync()` method.

```
public async Task SaveAsync() { ... }
```

The caller may also specify a directory to save the file to using the following method.
```
public async Task SaveAsync(string directory) { ... }
```

### Managing participants
A SkillWeek object provides a handful of methods for managing participants.

Participants can be added using the `SkillWeek.AddPlayerAsync(string)` method. This method takes a `name` parameter which is the participant's RuneScape username.
This method may fail if the `name` parameter is not an actual username. This method also relies on an API call.
```
public async Task AddPlayerAsync(string name)
```

Participants' scores can be updated either all at once, or individually. To update all the participants at once the `SkillWeek.UpdateAllAsync()` method may be used. 
To update a specific participant the `UpdateParticipantAsync(string)` method may be used. 
The latter requires the participant's RuneScape username to be passed as a parameter.
Both methods rely on an API call.
```
public async Task UpdateAllAsync() { ... }
```

Additionally, this method may fail if the given `name` parameter is not a valid RuneScape username.
```
public async Task UpdateParticipantAsync(string name) { ... }
```

### Updating the day
The SkillWeek object does not keep track of time by itself. 
The current day needs to be set using either the `SkillWeek.IncrementDay()` method, or the `SkillWeek.SetDay(int)` method.

To simply increment the day that the SkillWeek is on, the caller can use the following method.
```
public void IncrementDay() { ... }
```

To set a specific that the the SkillWeek is on, the caller can use the following method where `day` is the amount of days that the SkillWeek should have been active for. 
This is means it is 0 based.
```
public void SetDay(int day) { ... }
```

### Misc
If a list of the participants and their SkillWeek statistics is needed, the `SkillWeek.GetParticipants()` method may be used. 
This returns a dictionary with each participant's RuneScape username as the key and a Participant object as value. 
The Participant object stores information about the given Participant's SkillWeek performance.
This method cannot be used to manipulate the SkillWeek's participant data in any way.

```
public Dictionary<string, Participant> GetParticipants() { ... }
```
