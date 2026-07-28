using Godot;

public partial class persistance : Node
{
    private const string SavePath = "user://gamestate.cfg";

    public static void ResetSave()
    {
        if (FileAccess.FileExists(SavePath))
        {
            DirAccess.RemoveAbsolute(ProjectSettings.GlobalizePath(SavePath));
            GD.Print("Save file deleted.");
        }
    }
    public static void SetFlag(string key, bool value)
    {
        var config = new ConfigFile();
        config.Load(SavePath); // load existing so we don't wipe other keys (ignore error if file doesn't exist yet)

        config.SetValue("flags", key, value);
        config.Save(SavePath);
    }

    public static bool GetFlag(string key, bool defaultValue = false)
    {
        var config = new ConfigFile();
        var err = config.Load(SavePath);
        if (err != Error.Ok)
            return defaultValue;

        return (bool)config.GetValue("flags", key, defaultValue);
    }
}