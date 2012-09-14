using System.Runtime.Serialization;

namespace Weibo.DataModel.Status
{
    [DataContract]
    public enum StatusReadyType
    {
        [EnumMember]
        Reserved,
        [EnumMember]
        FriendsTimelineRefresh,
        [EnumMember]
        FriendsTimelineNextPage,
        [EnumMember]
        PublicTimelineRefresh,
        [EnumMember]
        PublicTimelineNextPage,

        [EnumMember]
        MentionsMeRefresh,

        [EnumMember]
        MentionsMeNextPage,

        [EnumMember]
        CommentsTimelineRefresh,
        [EnumMember]
        CommentsTimelineNextPage,

        [EnumMember]
        HotRepostsWeekly,
        [EnumMember]
        HotRepostsDaily,
        [EnumMember]
        HotCommentsWeekly,
        [EnumMember]
        HotCommentsDaily,

    }
}