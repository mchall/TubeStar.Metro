using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TubeStarMetro
{
    public sealed partial class DailyPlanner : UserControl
    {
        public event Action GameExit;
        public event Action Death;
        public event Action NewDayCompleted;

        public List<TaskBase> Appointments { get; private set; }
        private double _moneyAtStartOfDay;

        public DailyPlanner()
        {
            this.InitializeComponent();
            Translate();
            Appointments = new List<TaskBase>();

            btnAddTask.HandPointerCursor();
        }

        private void Translate()
        {
            txtAddTask.Text = EnglishStrings.AddTask.Translate();
        }

        private void AppointmentBlock_Click(object sender, ItemClickEventArgs e)
        {
            var appointmentBlock = e.ClickedItem as AppointmentBlock;
            if (appointmentBlock != null && appointmentBlock.Task != null)
            {
                //Special: quit job
                if (appointmentBlock.Task.TaskType == TaskType.Job)
                {
                    CustomMessageBox.ShowDialog(EnglishStrings.QuitJobHeader.Translate(), EnglishStrings.QuitJobText.Translate(), MessagePicture.Work, (result) =>
                    {
                        if (result == true)
                        {
                            Player.Current.QuitJob = true;
                            Appointments.RemoveAll(a => a.TaskType == TaskType.Job);
                            Update();
                        }
                    });
                }
                else if (appointmentBlock.Task.TaskType == TaskType.BowToRobotRulers)
                {
                    double cost = 10000;
                    if (Player.Current.Money < cost)
                    {
                        CustomMessageBox.ShowDialog(EnglishStrings.RiseUp.Translate(), String.Format(EnglishStrings.RebellionCashRequired.Translate(), cost.ToCurrencyString()), MessagePicture.Robot);
                    }
                    else
                    {
                        int chance = Player.Current.ChallengeMode ? 10 : 25;
                        CustomMessageBox.ShowDialog(EnglishStrings.RiseUp.Translate(), String.Format(EnglishStrings.RebellionStart.Translate(), chance), MessagePicture.Robot, (result) =>
                        {
                            if (result == true)
                            {
                                if (RandomHelpers.Chance(chance))
                                {
                                    CustomMessageBox.ShowDialog(EnglishStrings.Freedom.Translate(), EnglishStrings.RebellionSuccess.Translate(), MessagePicture.Robot);
                                    Player.Current.Money -= 10000;
                                    Player.Current.RobotRulers = false;
                                    Appointments.RemoveAll(a => a.TaskType == TaskType.BowToRobotRulers);
                                    Update();
                                }
                                else
                                {
                                    if (Death != null)
                                        Death();
                                }
                            }
                        });
                    }
                }
                else if (appointmentBlock.Task != null)
                {
                    RemoveAppointment(appointmentBlock.Task, false);
                }
            }
        }

        private async void AddTask_Click(object sender, RoutedEventArgs e)
        {
            AddTaskDialog dialog = new AddTaskDialog();
            dialog.TaskClick += (t) =>
            {
                AddTask(t);
            };
            await dialog.ShowAsync();
        }

        private void AddTask(TaskBase task)
        {
            foreach (var checkTask in Player.Current.TasksInProgress)
            {
                if (checkTask == task)
                    return;
            }

            Player.Current.TasksInProgress.Add(task);
            Update();
        }

        private void AddAppointment(TaskBase task)
        {
            if (Appointments.Count < 15)
            {
                task.HoursPutIn++;
                Appointments.Add(task);

                foreach (TodoTask t in tasksPanel.Items)
                {
                    t.UpdateText();
                }
                UpdateAppointments();
            }
        }

        private void RemoveAppointment(TaskBase task, bool removeTodoTask)
        {
            //Special case
            var shootVideoTask = task as ShootVideo;
            if (shootVideoTask != null)
            {
                foreach (var currentTask in Player.Current.TasksInProgress)
                {
                    var editVideoTask = currentTask as EditVideo;
                    if (editVideoTask != null && editVideoTask.Video == shootVideoTask.Video)
                    {
                        RemoveAppointment(currentTask, true);
                        break;
                    }
                }
            }

            if (removeTodoTask)
            {
                Appointments.RemoveAll(a => a == task);
                Player.Current.TasksInProgress.Remove(task);
            }
            else
            {
                Appointments.Remove(task);
                task.HoursPutIn--;
            }

            Update();
        }

        public void Update()
        {
            tasksPanel.Items.Clear();
            foreach (var task in Player.Current.TasksInProgress)
            {
                TodoTask appointmentTask = new TodoTask(task);
                appointmentTask.CancelTaskClick += (s, ev) =>
                {
                    bool isStudy = task is Study;
                    CustomMessageBox.ShowDialog(EnglishStrings.RemoveTask.Translate(), String.Format("{0} {1}", EnglishStrings.AreYouSure.Translate(), isStudy ? String.Format("\n{0}", EnglishStrings.NoRefunds.Translate()) : ""), MessagePicture.Question, (result) =>
                    {
                        if (result == true)
                        {
                            RemoveAppointment(task, true);
                        }
                    });
                };
                tasksPanel.Items.Add(appointmentTask);
                appointmentTask.UpdateText();
            }

            UpdateAppointments();
        }

        private void UpdateAppointments()
        {
            foreach (AppointmentBlock appointment in appointmentPanel.Items)
            {
                appointment.Task = null;
            }

            for (int i = 0; i < Appointments.Count; i++)
            {
                (appointmentPanel.Items[i] as AppointmentBlock).Task = Appointments[i];
            }
        }

        private void CleanTodoList()
        {
            foreach (TaskBase task in Player.Current.TasksInProgress)
            {
                if (task.IsCompleted)
                {
                    var studyTask = task as Study;
                    if (studyTask != null)
                    {
                        switch (studyTask.SkillModifierType)
                        {
                            case (SkillModifierType.Shooting):
                                Player.Current.ShootingSkill += studyTask.SkillModifier;
                                break;

                            case (SkillModifierType.PostProduction):
                                Player.Current.PostProductionSkill += studyTask.SkillModifier;
                                break;

                            case (SkillModifierType.VideoAttribute):
                                Player.Current.VideoAttributePoints += studyTask.SkillModifier;
                                break;

                            case (SkillModifierType.ViewQuality):
                                Player.Current.CanViewQualityBeforeUpload = true;
                                break;
                        }
                    }

                    var shootTask = task as ShootVideo;
                    if (shootTask != null)
                    {
                        Player.Current.Videos.Add(shootTask.Video);
                    }

                    var editTask = task as EditVideo;
                    if (editTask != null)
                    {
                        editTask.Video.HasBeenEdited = true;
                    }
                }
            }

            Player.Current.TasksInProgress.RemoveAll(t => t.IsCompleted);
        }

        public void NewDay()
        {
            _moneyAtStartOfDay = Player.Current.Money;

            Player.Current.Money -= 50; //Living Expenses
            Player.Current.Money -= Math.Max(0, Player.Current.CostOfLivingExtra);

            if (StoreItems.Current.Loan.Purchased)
            {
                Player.Current.LoanPayOff -= StoreItems.Current.Loan.AdditionalCost;
                Player.Current.Money -= StoreItems.Current.Loan.AdditionalCost;

                if (Player.Current.LoanPayOff <= 0)
                {
                    StoreItems.Current.Loan.Purchased = false;
                }
            }

            Appointments = new List<TaskBase>();
            if (Player.Current.RobotRulers)
            {
                var robots = new BowToRobotRulers();
                AddAppointment(robots);
                AddAppointment(robots);
                AddAppointment(robots);
            }
            if (!Player.Current.QuitJob)
            {
                Player.Current.Money += 100;
                var job = new Job();
                for (int i = 0; i <= job.HoursToComplete; i++)
                {
                    AddAppointment(job);
                }
                if (Player.Current.HasPromotion)
                {
                    Player.Current.Money += 50;
                    AddAppointment(job);
                    AddAppointment(job);
                }
                if (!Player.Current.QuitJob && Player.Current.Overtime)
                {
                    AddAppointment(job);
                    AddAppointment(job);
                    AddAppointment(job);
                    AddAppointment(job);
                    Player.Current.Overtime = false;
                }
            }
            CleanTodoList();
            Update();

            RunIterations();
        }

        private async void RunIterations()
        {
            await Task.Run(() =>
            {
                RunIterationWork();
            });
            if (Player.Current.Money < 0)
            {
                if (GameExit != null)
                    GameExit();
            }

            Player.Current.Iterations++;

            if (NewDayCompleted != null)
                NewDayCompleted();
        }

        private bool RunIterationWork()
        {
            foreach (var channel in Player.Current.Channels)
            {
                if (channel != Channel.UnreleasedVideos)
                {
                    double dailyIncome = 0;
                    double dailyExpenses = 0;
                    foreach (var video in channel.Videos)
                    {
                        VideoViewer.Iteration(channel, video, ref dailyIncome, ref dailyExpenses);
                        if (video.Iterations == 1)
                        {
                            dailyExpenses += video.Cost;
                            dailyExpenses += video.OnceOffCost;
                        }
                    }

                    //Channel stats
                    channel.SubscribersOverTime.Add(channel.Subscribers.Count);
                    channel.IncomeOverTime.Add(dailyIncome);
                    channel.ExpensesOverTime.Add(dailyExpenses);

                    //Player costs
                    Player.Current.Money += (dailyIncome - dailyExpenses);
                }
            }
            return true;
        }

        private void tasksPanel_ItemClick(object sender, ItemClickEventArgs e)
        {
            var todoTask = e.ClickedItem as TodoTask;
            if (todoTask != null && todoTask.Task != null)
            {
                if (!todoTask.Task.IsCompleted)
                {
                    AddAppointment(todoTask.Task);
                }
            }
        }
    }

    public struct IncomExpenses
    {
        public double Income { get; set; }
        public double Expenses { get; set; }
    }
}