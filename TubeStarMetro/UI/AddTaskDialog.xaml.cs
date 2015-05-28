using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TubeStarMetro
{
    public sealed partial class AddTaskDialog : UserControl, IPopupControl
    {
        private PopupHelper _parent;

        public event Action<TaskBase> TaskClick;

        public AddTaskDialog()
        {
            InitializeComponent();
            Translate();
            this.HandleDialogKeyboard(null, CancelButton_Click);
        }

        public void SetParent(PopupHelper parent)
        {
            _parent = parent;
        }

        public void OnFocus()
        {
        }

        private void Translate()
        {
            //Caption = EnglishStrings.AddTask.Translate();
            txtShootVideo.Text = EnglishStrings.ShootVideo.Translate();
            txtEditVideo.Text = EnglishStrings.EditVideo.Translate();
            txtStudy.Text = EnglishStrings.Study.Translate();
            btnCancel.Content = EnglishStrings.Cancel.Translate();

            btnCancel.HandPointerCursor();
            btnEditVideo.HandPointerCursor();
            btnShootVideo.HandPointerCursor();
            btnStudy.HandPointerCursor();
        }

        private async void AddTask_VideoShoot(object sender, RoutedEventArgs e)
        {
            if (TaskClick != null)
            {
                AddVideoDialog videoDialog = new AddVideoDialog();
                if (await videoDialog.ShowAsync() == true)
                {
                    if (videoDialog.Video != null)
                    {
                        var shootVideoTask = new ShootVideo(videoDialog.Video);
                        shootVideoTask.ExtraHours = videoDialog.Video.ExtraShootingHours;
                        TaskClick(shootVideoTask);
                    }
                }
            }
            _parent.Close(null);
        }

        private async void AddTask_VideoEdit(object sender, RoutedEventArgs e)
        {
            if (TaskClick != null)
            {
                List<Video> completedVideos = new List<Video>();
                foreach (var currentTask in Player.Current.TasksInProgress)
                {
                    var shootTask = currentTask as ShootVideo;
                    if (shootTask != null && !shootTask.Video.HasBeenEdited && currentTask.IsCompleted)
                    {
                        completedVideos.Add(shootTask.Video);
                    }
                }

                foreach (var video in Player.Current.Videos)
                {
                    if (!video.HasBeenEdited && !completedVideos.Contains(video))
                    {
                        completedVideos.Add(video);
                    }
                }

                foreach (var currentTask in Player.Current.TasksInProgress)
                {
                    var editTask = currentTask as EditVideo;
                    if (editTask != null)
                    {
                        completedVideos.Remove(editTask.Video);
                    }
                }

                if (completedVideos.Count == 0)
                {
                    CustomMessageBox.ShowDialog(EnglishStrings.TooSoon.Translate(), EnglishStrings.NoVideosToEdit.Translate(), MessagePicture.Puzzle);
                    return;
                }

                EditVideoDialog editDialog = new EditVideoDialog(completedVideos);
                if (await editDialog.ShowAsync() == true)
                {
                    if (editDialog.Video != null)
                    {
                        var editVideoTask = new EditVideo(editDialog.Video);
                        editVideoTask.ExtraHours = editDialog.Video.ExtraEditingHours;
                        TaskClick(editVideoTask);
                    }
                }
            }
            _parent.Close(null);
        }

        private async void AddTask_Study(object sender, RoutedEventArgs e)
        {
            if (TaskClick != null)
            {
                StudySelectionDialog studyDialog = new StudySelectionDialog();
                if (await studyDialog.ShowAsync() == true)
                {
                    if (studyDialog.ChosenStudy != null)
                        TaskClick(studyDialog.ChosenStudy);
                }
            }
            _parent.Close(null);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            _parent.Close(false);
        }
    }
}