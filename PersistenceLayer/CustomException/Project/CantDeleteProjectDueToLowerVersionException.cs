﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PersistenceLayer.CustomException.Project
{
    public class CantDeleteProjectDueToLowerVersionException : Exception
    {
        public CantDeleteProjectDueToLowerVersionException()
        {
        }
    }
}
