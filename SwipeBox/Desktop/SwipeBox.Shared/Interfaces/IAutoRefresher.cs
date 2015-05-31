// <copyright file="IAutoRefresher.cs" company="Park Side Software">
// Copyright (c) 29/04/2015 All Right Reserved
// </copyright>
// <author>Daniel Blackmore</author>
// <date>29/04/2015</date>
// <summary>Auto refresher interface</summary>

namespace SwipeBox.Shared.Interface
{
    /// <summary>
    /// Auto refresher interface
    /// </summary>
    public interface IAutoRefresher
    {
        /// <summary>
        /// Refresh action
        /// </summary>
        /// <param name="state">refresh state</param>
        void Refresh(object state);

        /// <summary>
        /// Stop refresh action
        /// </summary>
        void Stop();
    }
}
