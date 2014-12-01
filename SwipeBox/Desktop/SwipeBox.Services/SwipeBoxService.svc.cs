using SwipeBox.DAL.Context;
using SwipeBox.DAL.Repositories;
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

        public bool AddMeeting(int clientId)
        {
            throw new NotImplementedException();
        }
    }
}
