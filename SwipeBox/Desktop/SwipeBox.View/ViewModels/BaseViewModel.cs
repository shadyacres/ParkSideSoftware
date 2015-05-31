// <copyright file="BaseViewModel.cs" company="Park Side Software">
// Copyright (c) 29/04/2015 All Right Reserved
// </copyright>
// <author>Daniel Blackmore</author>
// <date>29/04/2015</date>
// <summary>Base View model for Child view models to inherit</summary>

using System.ComponentModel;

namespace SwipeBox.UI.ViewModel
{
    /// <summary>
    /// Base View model for Child view models to inherit
    /// </summary>
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Event to occur when a property value is changed
        /// </summary>
        /// <param name="propertyName">The property name</param>
        protected void OnNotifyPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Property changed event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
