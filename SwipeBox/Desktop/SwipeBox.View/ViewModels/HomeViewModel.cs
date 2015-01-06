using SwipeBox.BusinessLogic.MeetingBL;
using SwipeBox.Shared.Constants;
using SwipeBox.Shared.Entities;
using SwipeBox.Shared.Interface;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Linq;

namespace SwipeBox.UI.ViewModel
{
    public class HomeViewModel : BaseViewModel, IAutoRefresher
    {
        private Timer m_refreshTimer;
        private IMeetingsBL m_meetingsBL;
        private Meeting m_selectedMeeting;

        public HomeViewModel()
        {
            m_meetingsBL = new MeetingsBL();
            m_refreshTimer = new Timer(Refresh, null, GlobalConstants.NoRefreshInterval, GlobalConstants.NoRefreshInterval);
            Meetings = new ObservableCollection<Meeting>();
            RefreshMeetings();
            StartTimer();
        }

        public ObservableCollection<Meeting> Meetings { get; set; }

        public Meeting SelectedMeeting
        {
            get
            {
                return m_selectedMeeting;
            }
            set
            {
                if (value != null && m_selectedMeeting != value)
                {
                    m_selectedMeeting = value;
                }
            }
        }


        private void StartTimer()
        {
            m_refreshTimer.Change(GlobalConstants.RefreshInterval, GlobalConstants.RefreshInterval);
        }

        private void RefreshMeetings()
        {
#if DEBUG
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();
#endif
            if (System.Windows.Application.Current != null)
            {
                System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    var selected = SelectedMeeting;
                    Meetings.Clear();

                    foreach (var client in m_meetingsBL.GetTodaysMeetings().OrderByDescending(m => m.MeetingDate))
                    {
                        Meetings.Add(client);
                    }

                    if (selected != null)
                    {
                        SelectedMeeting = Meetings.FirstOrDefault(c => c.ClientId == selected.ClientId);
                    }
                }));
            }
#if DEBUG
            sw.Stop();
            System.Diagnostics.Debug.WriteLine("Refresh Client List Time: " + sw.ElapsedMilliseconds + "ms");
#endif

        }

        #region IAutoRefresher implementation
        public void Refresh(object state)
        {
            RefreshMeetings();
        }

        public void Stop()
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}
