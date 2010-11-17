using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeClimber.GoogleReaderConnector
{
    public enum UrlType
    {
        Feed,
        Tag,
        State,
        UserInfo,
        UnreadCount,
        SubscriptionsEdit, //subscription/list + edit
        ItemEdit, //edit-tag
        FriendsEdit //friend/list + edit
    }
}
