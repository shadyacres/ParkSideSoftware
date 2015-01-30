using SwipeBox.DAL.Context;
using SwipeBox.DAL.Repositories;
using SwipeBox.Services.DTO;
using SwipeBox.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SwipeBox.Services
{
    public class SwipeBoxService : ISwipeBoxService
    {

        private IMeetingRepository m_meetingRepo;
        private IClientRepository m_clientRepo;

        public SwipeBoxService()
        {
            m_meetingRepo = new EFMeetingRepository();
            m_clientRepo = new EFClientRepository();
        }

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

        public ClientDTO GetClientByEmail(string email)
        {
            var client = m_clientRepo.GetByEmail(email);

            if (client == null)
            {
                throw new FaultException("The specified client e-mail doesn't exist");
            }

            var clientDTO = new ClientDTO
            {
                Name = client.Name,
                Email = client.Email,
                PhoneNumber = client.PhoneNumber
            };

            return clientDTO;
        }
    }
}
