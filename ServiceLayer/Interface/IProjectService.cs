﻿using ContractLayer;
using DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interface
{
    public interface IProjectService
    {
        IList<ProjectListModel> GetProjectList(string searchTerm, string searchStatus);
        AddEditProjectModel GetProjectByID(long id);
    }
}