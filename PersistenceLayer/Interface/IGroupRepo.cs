﻿using DomainLayer;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistenceLayer.Interface
{
    public interface IGroupRepo
    {
         IList<Group> GetAllGroup(ISession session);
        Group GetGroupById(long groupID, ISession session);
    }
}
