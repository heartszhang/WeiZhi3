using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WeiZhi3
{
    public class WeiZhiCommands
    {
        public static readonly RoutedCommand NextPage = new RoutedCommand("NextPage", typeof(WeiZhiCommands));
        public static readonly RoutedCommand PreviousPage = new RoutedCommand("PreviousPage", typeof(WeiZhiCommands));

        public static readonly RoutedCommand Retweet
            = new RoutedCommand("Retweet", typeof(WeiZhiCommands));

        public static readonly RoutedCommand RetweetDirectly
            = new RoutedCommand("RetweetDirectly", typeof(WeiZhiCommands));

        public static readonly RoutedCommand Reply
            = new RoutedCommand("Reply", typeof(WeiZhiCommands));

        public static readonly RoutedCommand ReplyComment
            = new RoutedCommand("ReplyComment", typeof(WeiZhiCommands));

        public static readonly RoutedCommand NavigateUrl
            = new RoutedCommand("NavigateUrl", typeof(WeiZhiCommands));

        public static readonly RoutedCommand Reload
            = new RoutedCommand("Reload", typeof(WeiZhiCommands));


        public static readonly RoutedCommand FollowUnfollow
            = new RoutedCommand("FollowUnfollow", typeof(WeiZhiCommands));

        public static readonly RoutedCommand ShowDetail
           = new RoutedCommand("ShowDetail", typeof(WeiZhiCommands));

        public static readonly RoutedCommand ShowFriends
           = new RoutedCommand("ShowFriends", typeof(WeiZhiCommands));

        public static readonly RoutedCommand SwitchView
            = new RoutedCommand("SwitchView", typeof(WeiZhiCommands));

        public static readonly RoutedCommand DeleteComment = new RoutedCommand("DeleteCommentCommand",typeof(WeiZhiCommands));
        public static readonly RoutedCommand DeleteTweet = new RoutedCommand("DeleteTweet",typeof(WeiZhiCommands));
        public static readonly RoutedCommand Favorite = new RoutedCommand("Favorite",typeof(WeiZhiCommands));

        public static readonly RoutedCommand CreateTweet = new RoutedCommand("CreateTweet",typeof(WeiZhiCommands));

        public static readonly RoutedCommand GotoLatest = new RoutedCommand("GotoLatest",typeof(WeiZhiCommands));

        public static readonly RoutedCommand Navigate = new RoutedCommand("Navigate",typeof(WeiZhiCommands));

        public static readonly RoutedCommand PasteOrigin = new RoutedCommand("PasteOrigin", typeof(WeiZhiCommands));
        public static readonly RoutedCommand ClearContent = new RoutedCommand("ClearContent", typeof(WeiZhiCommands));

        public static readonly RoutedCommand PlayVideo = new RoutedCommand("PlayVideo",typeof(WeiZhiCommands));
        public static readonly RoutedCommand PlayMusic = new RoutedCommand("PlayMusic", typeof(WeiZhiCommands));
        public static readonly RoutedCommand Play = new RoutedCommand("Play",typeof(WeiZhiCommands));

        public static readonly RoutedCommand ToggleCommentFlag = new RoutedCommand("ToggleCommentFlag", typeof(WeiZhiCommands));

        public static readonly RoutedCommand Submit  = new RoutedCommand("Submit", typeof(WeiZhiCommands));
        public static readonly RoutedCommand SelectImage = new RoutedCommand("SelectImage",typeof(WeiZhiCommands));

        public static readonly RoutedCommand ScrollUp = new RoutedCommand("ScrollUp",typeof(WeiZhiCommands));
        public static readonly RoutedCommand ScrollDown = new RoutedCommand("ScrollDown", typeof(WeiZhiCommands));

        public static readonly RoutedCommand CopyTweet = new RoutedCommand("CopyTweet",typeof(WeiZhiCommands));
        public static readonly RoutedCommand CopyName = new RoutedCommand("CopyName",typeof(WeiZhiCommands));
        public static readonly RoutedCommand ViewWeiboViaWeb = new RoutedCommand("ViewWeiboViaWeb",typeof(WeiZhiCommands));
        public static readonly RoutedCommand ViewUserViaWeb = new RoutedCommand("ViewUserViaWeb", typeof(WeiZhiCommands));
        public static readonly RoutedCommand CommentRetweet = new RoutedCommand("CommentRetweet", typeof(WeiZhiCommands));

    }
}
