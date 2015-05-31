// <copyright file="HomeViewModel.cs" company="Park Side Software">
// Copyright (c) 29/04/2015 All Right Reserved
// </copyright>
// <author>Daniel Blackmore</author>
// <date>29/04/2015</date>
// <summary>Home ViewModel for view interaction</summary>

using SwipeBox.BusinessLogic.MeetingBL;
using SwipeBox.Shared.Constants;
using SwipeBox.Shared.Entities;
using SwipeBox.Shared.Interface;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace SwipeBox.UI.ViewModel
{
    /// <summary>
    /// Home ViewModel for view interaction
    /// </summary>
    public class HomeViewModel : BaseViewModel, IAutoRefresher
    {
        private Timer m_refreshTimer;
        private IMeetingsBL m_meetingsBL;
        private Meeting m_selectedMeeting;

        /// <summary>
        /// Initializes a new instance of the home view model class
        /// </summary>
        public HomeViewModel()
        {
            m_meetingsBL = new MeetingsBL();
            m_refreshTimer = new Timer(Refresh, null, GlobalConstants.NoRefreshInterval, GlobalConstants.NoRefreshInterval);
            Meetings = new ObservableCollection<Meeting>();
            RefreshMeetings();
            StartTimer();
        }

        /// <summary>
        /// Gets or sets the list of meetings
        /// </summary>
        public ObservableCollection<Meeting> Meetings { get; set; }

        /// <summary>
        /// Gets or sets the selected meeeting
        /// </summary>
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


        /// <summary>
        /// Start the refresh timer
        /// </summary>
        private void StartTimer()
        {
            m_refreshTimer.Change(GlobalConstants.RefreshInterval, GlobalConstants.RefreshInterval);
        }

        /// <summary>
        /// Refresh today's meetings
        /// </summary>
        private void RefreshMeetings()
        {
#if DEBUG
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();
#endif
            // Execute the action on the main application thread
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
            System.Diagnostics.Debug.WriteLine("Refresh meetings List Time: " + sw.ElapsedMilliseconds + "ms");
#endif

        }

        #region IAutoRefresher implementation
        /// <summary>
        /// Execute the refresh action
        /// </summary>
        /// <param name="state">current state</param>
        public void Refresh(object state)
        {
            RefreshMeetings();
        }

        /// <summary>
        /// Stop refreshing
        /// </summary>
        public void Stop()
        {
            m_refreshTimer.Change(GlobalConstants.NoRefreshInterval, GlobalConstants.NoRefreshInterval);
        }
        #endregion
    }
}
