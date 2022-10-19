namespace Project.Core.Middlewares.Translation;

public static class Localization
{
    public static string Translate(Messages message)
    {
        return MsgResource.ResourceManager.GetString(message.ToString())!;
    }
}