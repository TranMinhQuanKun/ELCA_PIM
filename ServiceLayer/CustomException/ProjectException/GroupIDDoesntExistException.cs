﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.CustomException.ProjectException
{
    public class GroupIDDoesntExistException : Exception
    {
        public GroupIDDoesntExistException()
        {
        }

        public GroupIDDoesntExistException(string message) : base(message)
        {
        }

        public GroupIDDoesntExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        
    }
}