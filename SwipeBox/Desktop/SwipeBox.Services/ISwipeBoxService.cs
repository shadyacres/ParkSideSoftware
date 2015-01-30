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
    
    [ServiceContract]
    public interface ISwipeBoxService
    {
        [OperationContract]
        bool AddMeeting(int clientId);

        [OperationContract]
        ClientDTO GetClientByEmail(string email); 
    }
}
