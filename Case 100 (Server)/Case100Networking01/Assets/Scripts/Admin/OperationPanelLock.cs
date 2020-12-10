public static class OperationPanelLock
{
    public static bool operationPanelOn = false;

    public static void OnOpen()
    {
        operationPanelOn = true;
    }

    public static void OnClose()
    {
        operationPanelOn = false;
    }
}