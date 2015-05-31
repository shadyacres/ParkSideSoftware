// <copyright file="IMeetingRepository.cs" company="Park Side Software">
// Copyright (c) 29/04/2015 All Right Reserved
// </copyright>
// <author>Daniel Blackmore</author>
// <date>29/04/2015</date>
// <summary>Interface for meetings repository</summary>

using SwipeBox.Shared.Entities;

namespace SwipeBox.DAL.Repositories
{
    /// <summary>
    /// Interface for meetings repository
    /// </summary>
    public interface IMeetingRepository : IRepository<Meeting>
    {
    }
}
