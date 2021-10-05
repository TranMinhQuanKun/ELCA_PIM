﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.CustomException.ProjectException
{
    public class ProjectNumberDuplicateException : Exception
    {
        public ProjectNumberDuplicateException()
        {
        }

        public ProjectNumberDuplicateException(string message) : base(message)
        {
        }

        public ProjectNumberDuplicateException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ProjectNumberDuplicateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}