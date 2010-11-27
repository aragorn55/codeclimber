namespace CodeClimber.GoogleReaderConnector
{
    public enum StateType
    {
        ReadingList,
        Starred,
        Shared,
        Like,
        Read,
        SharedByFriends,
        KeptUnread
    }

    public static class StateTypeExternsions
    {
        public static string ConvertToString(this StateType state)
        {
            switch (state)
            {
                case StateType.ReadingList:
                    return  "reading-list";
                case StateType.Starred:
                    return "starred";
                case StateType.Shared:
                    return "broadcast";
                case StateType.SharedByFriends:
                    return "broadcast-friends";
                case StateType.Like:
                    return "like";
                case StateType.Read:
                    return "read";
                case StateType.KeptUnread:
                    return "tracking-kept-unread";
                default:
                    return "";
            }
        }
    }
}
