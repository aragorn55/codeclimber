namespace CodeClimber.GoogleReaderConnector
{
    public enum ItemTag
    {
        None,
        ReadingList,
        Starred,
        Shared,
        Like,
        Read,
        SharedByFriends,
        KeptUnread,
        TrackingKeptUnread
    }

    public static class StateTypeExternsions
    {
        public static string ConvertToString(this ItemTag state)
        {
            switch (state)
            {
                case ItemTag.ReadingList:
                    return "reading-list";
                case ItemTag.Starred:
                    return "starred";
                case ItemTag.Shared:
                    return "broadcast";
                case ItemTag.SharedByFriends:
                    return "broadcast-friends";
                case ItemTag.Like:
                    return "like";
                case ItemTag.Read:
                    return "read";
                case ItemTag.KeptUnread:
                    return "kept-unread";
                case ItemTag.TrackingKeptUnread:
                    return "tracking-kept-unread";
                default:
                    return "";
            }
        }
    }
}
