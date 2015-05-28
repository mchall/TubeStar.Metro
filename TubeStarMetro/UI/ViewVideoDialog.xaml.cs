using System;
using System.Collections.Generic;
using System.Linq;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace TubeStarMetro
{
    public sealed partial class ViewVideoDialog : UserControl, IPopupControl
    {
        private PopupHelper _parent;
        private Video _video;

        public ViewVideoDialog(Video video)
        {
            InitializeComponent();
            _video = video;

            string html = @"<iframe width=""640"" height=""390"" src=""http://www.youtube.com/embed/" + video.YouTubeVideoId + @"?rel=0"" frameborder=""0""></iframe>";
            this.webView.NavigateToString(html);
            Translate();
            if (GetCount(_video) == 0)
            {
                lblComments.Visibility = commentStack.Visibility = Visibility.Collapsed;
            }
            Populate(video);

            btnOk.HandPointerCursor();
            this.HandleDialogKeyboard(OKButton_Click, OKButton_Click);
        }

        public void SetParent(PopupHelper parent)
        {
            _parent = parent;
        }

        private void Translate()
        {
            //Caption = EnglishStrings.VideoComments.Translate();
            lblComments.Text = String.Format("{0} ({1})", EnglishStrings.Comments.Translate(), GetCount(_video).ToNumberString());
            btnOk.Content = EnglishStrings.Ok.Translate();
        }

        private int GetCount(Video video)
        {
            int count = video.Comments.Count;
            foreach (var comment in video.Comments)
            {
                count += comment.Likes;
            }
            return count;
        }

        private void Populate(Video video)
        {
            List<VideoComment> orderedComments = new List<VideoComment>();
            foreach (var item in video.Comments)
            {
                VideoComment likedComment = new VideoComment(item.Comment, item.Likes);
                orderedComments.Add(likedComment);
            }

            foreach (var comment in orderedComments)
            {
                CommentBlock block = new CommentBlock(comment.Comment, comment.Likes);
                commentStack.Children.Insert(0, block);
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            _parent.Close(true);
        }

        public void OnFocus()
        {
        }
    }
}