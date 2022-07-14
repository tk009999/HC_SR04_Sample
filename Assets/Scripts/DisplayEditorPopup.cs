
public class DisplayEditorPopup
{
    public static bool DisplayDialog(string title, string message, string ok, string cancel)
    {
        bool status = false;
#if UNITY_EDITOR
        status = UnityEditor.EditorUtility.DisplayDialog(title, message, ok, cancel);
#endif
        return status;
    }
}