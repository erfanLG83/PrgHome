namespace PrgHome.Web.Classes
{
    public enum IconType
    {
        Success,
        Warning
    }
    public class Popup
    {
        public Popup()
        {
        }
        public Popup(string title)
        {
            Title = title;
        }
        public Popup(string title, string message)
        {
            Title = title;
            Message = message;
        }
        public Popup(string title, string message, IconType icon)
        {
            Title = title;
            Message = message;
            IconType = icon;
        }
        public Popup(string title, string message, IconType icon,string buttonText)
        {
            Title = title;
            Message = message;
            IconType = icon;
            ButtonText = buttonText;
        }
        private static void ShowPopup(Popup popup)
        {
            PopupModel = popup;
        }
        public static bool IsShow
        {
            get
            {
                if (PopupModel == null)
                    return false;
                else
                    return true;
            }
        }
        public string Title { get; set; }
        public string Message { get; set; }
        public string ButtonText { get; set; }
        public IconType IconType { get; set; }
        public static Popup PopupModel = null;
    }
}
