// <copyright file="SwipeBoxService.svc.cs" company="Park Side Software">
// Copyright (c) 29/04/2015 All Right Reserved
// </copyright>
// <author>Daniel Blackmore</author>
// <date>29/04/2015</date>
// <summary>Swipe Box Service implementation</summary>

using SwipeBox.DAL.Repositories;
using SwipeBox.Services.DTO;
using SwipeBox.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace SwipeBox.Services
{
    /// <summary>
    /// Swipebox web service
    /// </summary>
    public class SwipeBoxService : ISwipeBoxService
    {

        private IMeetingRepository m_meetingRepo;
        private IClientRepository m_clientRepo;

        /// <summary>
        /// Initializes a new instance of the SwipeBoxService class.
        /// </summary>
        public SwipeBoxService()
        {
            m_meetingRepo = new EFMeetingRepository();
            m_clientRepo = new EFClientRepository();
        }

        /// <summary>
        /// Add a meeting for a client
        /// </summary>
        /// <param name="clientId">the client's id</param>
        /// <returns>true if the meeting is added successfully</returns>
        public bool AddMeeting(int clientId)
        {
            try
            {
                var meeting = new Meeting
                {
                    ClientId = clientId,
                    MeetingDate = DateTime.Now
                };

                return m_meetingRepo.Save(meeting);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Get a client from their email
        /// </summary>
        /// <param name="email">the user email</param>
        /// <returns>The client</returns>
        public ClientDTO GetClientByEmail(string email)
        {
            var client = m_clientRepo.GetByEmail(email);

            if (client == null)
            {
                throw new FaultException("The specified client e-mail doesn't exist");
            }

            var clientDTO = new ClientDTO
            {
                ClientID = client.ClientId,
                Name = client.Name,
                Email = client.Email,
                PhoneNumber = client.PhoneNumber
            };

            return clientDTO;
        }

        /// <summary>
        /// Gets a list of all active clients
        /// </summary>
        /// <returns>list of clients</returns>
        public List<ClientDTO> GetAllClients()
        {
            var clients = m_clientRepo.Get.Where(c => c.Active || c.Active == null);
            if (clients == null)
            {
                throw new FaultException("No clients found");
            }
            var retVal = new List<ClientDTO>();

            foreach (var client in clients)
            {
                var clientText = string.Format("Id={0};Name={1};Email={2};Phone={3};",
                                               client.ClientId,
                                               client.Name,
                                               client.Email,
                                               client.PhoneNumber);
                var clientDto = new ClientDTO
                {
                    ClientID = client.ClientId,
                    Name = client.Name,
                    Email = client.Email,
                    PhoneNumber = client.PhoneNumber
                };

                retVal.Add(clientDto);
            }

            return retVal;
        }

        /// <summary>
        /// Authorise a user using their email and password
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="pass">password</param>
        /// <returns>True if the user is authorized</returns>
        public bool AuthorizeUser(string email, string pass)
        {
            return m_clientRepo.AuthorizeUser(email, pass);
        }
    }
}
