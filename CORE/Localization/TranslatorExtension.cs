namespace CORE.Localization;

public static class TranslatorExtension
{
    public static string Translate(this Messages message)
    {
        ;
        return MsgResource.ResourceManager.GetString(message.ToString())
               ?? throw new ArgumentNullException($"{message.ToString()} - Key was not found in MessageResource file");
    }
}