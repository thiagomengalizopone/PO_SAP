namespace Zopone.AddOn.PO
{
    public class RightClickEventHandler
    {
        public static void Interface_RightClickEvent(ref SAPbouiCOM.ContextMenuInfo eventInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;
        }
    }
}
