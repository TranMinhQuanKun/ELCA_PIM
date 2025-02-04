﻿using ContractLayer;
using ContractLayer.AngularResponseModel;
using Newtonsoft.Json;
using PIM_Tool_ELCA.Constant;
using PIM_Tool_ELCA.Enum;
using ServiceLayer;
using ServiceLayer.CustomException.ProjectException;
using ServiceLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Utilities;
using Utilities.Constant;

namespace PIM_Tool_ELCA.Controllers
{
    public class ProjectController : CustomController
    {

        private readonly IProjectService _projectService;
        private readonly IEmployeeService _employeeService;
        private readonly IGroupService _groupService;
        public ProjectController(IProjectService projectService, IEmployeeService employeeService,
            IGroupService groupService)
        {
            _projectService = projectService;
            _employeeService = employeeService;
            _groupService = groupService;
        }

        [HandleError]
        public ActionResult Index()
        {
            return RedirectToAction("ProjectList");
        }
        [HandleError]
        [HttpGet]
        public ActionResult ProjectListAPI()
        {
            SearchProjectRequestModel request = new SearchProjectRequestModel()
            {
                PageIndex = ProjectSearchConstant.DefaultPageIndex,
                PageSize = ProjectSearchConstant.DefaultPageSize,
                SearchTerm = Session[ProjectSearchConstant.SearchTermSessionVar] != null
                    ? Session[ProjectSearchConstant.SearchTermSessionVar].ToString() : string.Empty,
                SearchStatus = Session[ProjectSearchConstant.SearchStatusSessionVar] != null
                    ? Session[ProjectSearchConstant.SearchStatusSessionVar].ToString() : string.Empty,
            };
            var result = _projectService.GetProjectList(request);

            //------------------------------

            //ViewBag.ProjectList = result.ProjectList.ToList<ProjectListModel>();
            //var searchStatus = Session[ProjectSearchConstant.SearchStatusSessionVar]?.ToString();
            //ViewBag.StatusList = BuildStatusListForProjectListPage(searchStatus);
            //ViewBag.ResultCount = result.ResultCount;
            //ViewBag.CurrentPage = ProjectSearchConstant.DefaultPageIndex;
            //ViewBag.CurrentPageSize = ProjectSearchConstant.DefaultPageSize;
            //return View("ProjectList");
            //---------------------------------------
            var response = new ProjectListResponseModel
            {
                ProjectList = result.ProjectList.ToList<ProjectListModel>(),
                PageIndex = ProjectSearchConstant.DefaultPageIndex,
                PageSize = ProjectSearchConstant.DefaultPageSize,
                ResultCount = result.ResultCount
            };
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        //return
        [HandleError]
        public ActionResult ProjectList()
        {

            SearchProjectRequestModel request = new SearchProjectRequestModel()
            {
                PageIndex = ProjectSearchConstant.DefaultPageIndex,
                PageSize = ProjectSearchConstant.DefaultPageSize,
                SearchTerm = Session[ProjectSearchConstant.SearchTermSessionVar] != null
                    ? Session[ProjectSearchConstant.SearchTermSessionVar].ToString() : string.Empty,
                SearchStatus = Session[ProjectSearchConstant.SearchStatusSessionVar] != null
                    ? Session[ProjectSearchConstant.SearchStatusSessionVar].ToString() : string.Empty,
            };
            var result = _projectService.GetProjectList(request);

            //------------------------------
            ViewBag.ProjectList = result.ProjectList.ToList<ProjectListModel>();
            var searchStatus = Session[ProjectSearchConstant.SearchStatusSessionVar]?.ToString();
            ViewBag.StatusList = BuildStatusListForProjectListPage(searchStatus);
            ViewBag.ResultCount = result.ResultCount;
            ViewBag.CurrentPage = ProjectSearchConstant.DefaultPageIndex;
            ViewBag.CurrentPageSize = ProjectSearchConstant.DefaultPageSize;
            //---------------------------------------
            return View("ProjectList");
            //return new FilePathResult("~/Scripts/ClientApp/index.html", "text/html");
        }
        [HttpGet]
        public ActionResult SearchProjectAPI([Bind(Include = "SearchTerm, SearchStatus, PageIndex, PageSize")]
                                                    SearchProjectModel searchProjectModel)
        {
            if (searchProjectModel.PageIndex == 0 || searchProjectModel.PageSize == 0)
                return Json(new ProjectListResponseModel
                {
                    ProjectList = null,
                    PageIndex = 0,
                    PageSize = 0,
                    ResultCount = 0
                }, JsonRequestBehavior.AllowGet);

            Session["SearchTerm"] = searchProjectModel.SearchTerm;
            Session["SearchStatus"] = searchProjectModel.SearchStatus;

            SearchProjectRequestModel request = new SearchProjectRequestModel()
            {
                PageIndex = searchProjectModel.PageIndex,
                PageSize = searchProjectModel.PageSize,
                SearchStatus = searchProjectModel.SearchStatus,
                SearchTerm = searchProjectModel.SearchTerm,
            };

            var result = _projectService.GetProjectList(request);

            //ViewBag.ProjectList = result.ProjectList
            //    .OrderBy(proj => proj.ProjectNumber).ToList<ProjectListModel>();
            //var searchStatus = Session[ProjectSearchConstant.SearchStatusSessionVar]?.ToString();
            //ViewBag.StatusList = BuildStatusListForProjectListPage(searchStatus);
            //ViewBag.ResultCount = result.ResultCount;
            //ViewBag.CurrentPage = searchProjectModel.PageIndex;
            //ViewBag.CurrentPageSize = searchProjectModel.PageSize;
            //return View("ProjectList");

            return Json(new ProjectListResponseModel
            {
                ProjectList = result.ProjectList
                                    .OrderBy(proj => proj.ProjectNumber)
                                    .ToList<ProjectListModel>(),
                PageIndex = searchProjectModel.PageIndex,
                PageSize = searchProjectModel.PageSize,
                ResultCount = result.ResultCount
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult SearchProject([Bind(Include = "SearchTerm, SearchStatus, PageIndex, PageSize")]
                                            SearchProjectModel searchProjectModel)
        {
            if (searchProjectModel.PageIndex == 0
                || searchProjectModel.PageSize == 0)
                return Redirect("/Home/NotFound");

            Session["SearchTerm"] = searchProjectModel.SearchTerm;
            Session["SearchStatus"] = searchProjectModel.SearchStatus;

            SearchProjectRequestModel request = new SearchProjectRequestModel()
            {
                PageIndex = searchProjectModel.PageIndex,
                PageSize = searchProjectModel.PageSize,
                SearchStatus = searchProjectModel.SearchStatus,
                SearchTerm = searchProjectModel.SearchTerm,
            };

            var result = _projectService.GetProjectList(request);
            ViewBag.ProjectList = result.ProjectList
                .OrderBy(proj => proj.ProjectNumber).ToList<ProjectListModel>();
            var searchStatus = Session[ProjectSearchConstant.SearchStatusSessionVar]?.ToString();
            ViewBag.StatusList = BuildStatusListForProjectListPage(searchStatus);
            ViewBag.ResultCount = result.ResultCount;
            ViewBag.CurrentPage = searchProjectModel.PageIndex;
            ViewBag.CurrentPageSize = searchProjectModel.PageSize;
            return View("ProjectList");
        }
        private List<SelectListItem> BuildGroupList(IList<long> groupIdList, long id)
        {
            List<SelectListItem> groupList = new List<SelectListItem>();
            foreach (var item in groupIdList)
            {
                groupList.Add(new SelectListItem
                {
                    Text = $"{Resource.AddEditProject.AddEditProjectRe.Group_WordLabel} {item.ToString()}",
                    Value = item.ToString(),
                    Selected = id == item
                });
            }
            return groupList;
        }
        private List<SelectListItem> BuildStatusList(string status)
        {
            List<SelectListItem> statusList = new List<SelectListItem>();
            statusList.Add(new SelectListItem
            {
                Text = @Resource.AddEditProject.AddEditProjectRe.ProjectStatusNew_Label,
                Value = ProjectStatusConstant.New,
                Selected = status == ProjectStatusConstant.New
            });
            statusList.Add(new SelectListItem
            {
                Text = @Resource.AddEditProject.AddEditProjectRe.ProjectStatusPlanned_Label,
                Value = ProjectStatusConstant.Planned,
                Selected = status == ProjectStatusConstant.Planned
            });
            statusList.Add(new SelectListItem
            {
                Text = @Resource.AddEditProject.AddEditProjectRe.ProjectStatusInProgress_Label,
                Value = ProjectStatusConstant.InProgress,
                Selected = status == ProjectStatusConstant.InProgress,
            });
            statusList.Add(new SelectListItem
            {
                Text = @Resource.AddEditProject.AddEditProjectRe.ProjectStatusFinished_Label,
                Value = ProjectStatusConstant.Finished,
                Selected = status == ProjectStatusConstant.Finished
            });
            return statusList;
        }
        private List<SelectListItem> BuildStatusListForProjectListPage(string status)
        {
            List<SelectListItem> statusList = new List<SelectListItem>();
            statusList = BuildStatusList(status);
            statusList.Add(new SelectListItem
            {
                Text = @Resource.AddEditProject.AddEditProjectRe.ProjectStatusHolder_Label,
                Value = string.Empty,
                Selected = String.IsNullOrEmpty(status) ? true : false
            });
            return statusList;
        }
        [HandleError]
        public ActionResult EditProject(long id)
        {
            var targetProject = _projectService.GetProjectById(id);
            var groupIdList = _groupService.GetGroupIdList();
            ViewBag.EditMode = EditMode.Edit;
            ViewBag.VisaList = _employeeService.GetAllMembers();
            ViewBag.GroupList = BuildGroupList(groupIdList, id);
            ViewBag.StatusList = BuildStatusList(targetProject.Status);
            return View("AddEditProject", targetProject);
        }
        [HttpGet]
        public ActionResult EditProjectAPI(long id)
        {
            var targetProject = _projectService.GetProjectById(id);
            //var groupIdList = _groupService.GetGroupIdList();
            //ViewBag.EditMode = EditMode.Edit;
            //ViewBag.VisaList = _employeeService.GetAllMembers();
            //ViewBag.GroupList = BuildGroupList(groupIdList, id);
            //ViewBag.StatusList = BuildStatusList(targetProject.Status);
            //return View("AddEditProject", targetProject);
            var response = new EditProjectResponseModel
            {
                projectModel = targetProject,
                hasError = false
            };
            return Json(response, JsonRequestBehavior.AllowGet);
                //(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult EditProjectAPI([Bind(Include = "Id, GroupId,ProjectNumber,Name,Customer,Status,StartDate,EndDate,MemberString,Version")] AddEditProjectModel projectModel)
        {
            bool _hasError = false;
            List<String> _errList = new List<string>();
            try
            {
                if (ModelState.IsValid)
                {
                    List<string> memberList = new List<string>(VisaHelper.SplitVisa(projectModel.MemberString));
                    projectModel.MembersList = memberList;
                    _projectService.Update(projectModel);
                }
            }
            catch (ProjectHaveBeenEditedByAnotherUserException)
            {
                var targetProject = _projectService.GetProjectById((long)projectModel.Id);


                //ViewBag.HasVersionWarning = true;
                //ModelState.Clear();

                //var groupIdList = _groupService.GetGroupIdList();
                //ViewBag.EditMode = EditMode.Edit;
                //ViewBag.VisaList = _employeeService.GetAllMembers();
                //ViewBag.GroupList = BuildGroupList(groupIdList, projectModel.Id.Value);
                //ViewBag.StatusList = BuildStatusList(targetProject.Status);
                //return View("AddEditProject", targetProject);
                var response = new EditProjectResponseModel
                {
                    projectModel = targetProject,
                    hasError = true,
                    errorList = new List<string> { FormErrorResponseConstant.ConcurrentUpdate }
                };
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (GroupIDDoesntExistException)
            {
                _hasError = true;
                _errList.Add(FormErrorResponseConstant.UnexpectedError);
                //ModelState.AddModelError(nameof(projectModel.GroupId), "GroupId doesn't exist");
            }
            catch (CantChangeProjectNumberException)
            {
                _hasError = true;
                _errList.Add(FormErrorResponseConstant.UnexpectedError);
                //ModelState.AddModelError(nameof(projectModel.ProjectNumber), "Can't change project number");
            }
            catch (InvalidVisaException)
            {
                _hasError = true;
                _errList.Add(FormErrorResponseConstant.UnexpectedError);
                //ModelState.AddModelError(nameof(projectModel.MemberString), "Invalid member list");
            }
            catch (InvalidStatusException)
            {
                _hasError = true;
                _errList.Add(FormErrorResponseConstant.UnexpectedError);
                //ModelState.AddModelError(nameof(projectModel.Status), "Invalid status");
            }
            catch (EndDateSoonerThanStartDateException)
            {
                _hasError = true;
                _errList.Add(FormErrorResponseConstant.UnexpectedError);
                //ModelState.AddModelError(nameof(projectModel.EndDate), "End date can't be sooner than start date.");
            }

            if (_hasError == false)
            {
                var response = new EditProjectResponseModel
                {
                    hasError = false,
                };
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //var groupIdList = _groupService.GetGroupIdList();
                //ViewBag.EditMode = EditMode.Edit;
                //ViewBag.VisaList = _employeeService.GetAllMembers();
                //ViewBag.GroupList = BuildGroupList(groupIdList, projectModel.Id.Value);
                //ViewBag.StatusList = BuildStatusList(projectModel.Status);
                //return View("AddEditProject", projectModel);
                var targetProject = _projectService.GetProjectById((long)projectModel.Id);

                var response = new EditProjectResponseModel
                {
                    projectModel = targetProject,
                    hasError = true,
                    errorList = _errList
                };
                return Json(JsonConvert.SerializeObject(response), JsonRequestBehavior.AllowGet);
            }
        }

        [HandleError]
        [HttpPost]
        public ActionResult EditProject(
            [Bind(Include = "Id, GroupId,ProjectNumber,Name,Customer,Status,StartDate,EndDate,MemberString,Version")] AddEditProjectModel projectModel)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    List<string> memberList = new List<string>(VisaHelper.SplitVisa(projectModel.MemberString));
                    projectModel.MembersList = memberList;
                    _projectService.Update(projectModel);
                }
            } 
            catch (ProjectHaveBeenEditedByAnotherUserException)
            {
                ViewBag.HasVersionWarning = true;
                ModelState.Clear();
                var targetProject = _projectService.GetProjectById((long)projectModel.Id);
                var groupIdList = _groupService.GetGroupIdList();
                ViewBag.EditMode = EditMode.Edit;
                ViewBag.VisaList = _employeeService.GetAllMembers();
                ViewBag.GroupList = BuildGroupList(groupIdList, projectModel.Id.Value);
                ViewBag.StatusList = BuildStatusList(targetProject.Status);
                return View("AddEditProject", targetProject);
            }
            catch (GroupIDDoesntExistException)
            {
                ModelState.AddModelError(nameof(projectModel.GroupId), "GroupId doesn't exist");
            }
            catch (CantChangeProjectNumberException)
            {
                ModelState.AddModelError(nameof(projectModel.ProjectNumber), "Can't change project number");
            }
            catch (InvalidVisaException)
            {
                ModelState.AddModelError(nameof(projectModel.MemberString), "Invalid member list");
            }
            catch (InvalidStatusException)
            {
                ModelState.AddModelError(nameof(projectModel.Status), "Invalid status");
            }
            catch (EndDateSoonerThanStartDateException)
            {
                ModelState.AddModelError(nameof(projectModel.EndDate), "End date can't be sooner than start date.");
            }

            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            else
            {
                var groupIdList = _groupService.GetGroupIdList();
                ViewBag.EditMode = EditMode.Edit;
                ViewBag.VisaList = _employeeService.GetAllMembers();
                ViewBag.GroupList = BuildGroupList(groupIdList, projectModel.Id.Value);
                ViewBag.StatusList = BuildStatusList(projectModel.Status);
                return View("AddEditProject", projectModel);
            }
        }

        [HttpGet]
        [HandleError]
        public ActionResult NewProject()
        {
            var groupIdList = _groupService.GetGroupIdList();
            ViewBag.EditMode = EditMode.New;
            ViewBag.VisaList = _employeeService.GetAllMembers();
            ViewBag.GroupList = BuildGroupList(groupIdList, 0);
            ViewBag.StatusList = BuildStatusList(string.Empty);
            return View("AddEditProject", new AddEditProjectModel());
        }
        [HttpGet]
        public ActionResult VisaListAPI()
        {
           var response = _employeeService.GetAllMembers();
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GroupListAPI()
        {
            var response = _groupService.GetGroupIdList();
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [HandleError]
        public ActionResult NewProject([Bind(Include = "GroupId,ProjectNumber,Name,Customer,Status,StartDate,EndDate,MemberString,Version")] AddEditProjectModel projectModel)
        {
            try
            {
                if (ModelState.ContainsKey("Id"))
                    ModelState["Id"].Errors.Clear();
                if (ModelState.IsValid)
                {
                    List<string> memberList = new List<string>(VisaHelper.SplitVisa(projectModel.MemberString));
                    projectModel.MembersList = memberList;
                    _projectService.Create(projectModel);
                    return RedirectToAction("Index");
                }
            }
            catch (ProjectNumberDuplicateException)
            {
                ModelState.AddModelError(nameof(projectModel.ProjectNumber), Resource.AddEditProject.AddEditProjectRe.DuplicateProjectNumber_ModelError);
            }
            catch (InvalidVisaException)
            {
                ModelState.AddModelError(nameof(projectModel.MemberString), "Invalid member list");
            }
            catch (InvalidStatusException)
            {
                ModelState.AddModelError(nameof(projectModel.Status), "Invalid status");
            }
            catch (EndDateSoonerThanStartDateException)
            {
                ModelState.AddModelError(nameof(projectModel.EndDate), Resource.AddEditProject.AddEditProjectRe.EndDate_ModelError);
            }
            var groupIdList = _groupService.GetGroupIdList();
            ViewBag.EditMode = EditMode.New;
            ViewBag.VisaList = _employeeService.GetAllMembers();
            ViewBag.GroupList = BuildGroupList(groupIdList, 0);
            ViewBag.StatusList = BuildStatusList(projectModel.Status);
            return View("AddEditProject", projectModel);
        }

        [HttpPost]
        public ActionResult DeleteProject(List<DeleteProjectRequestModel> projectList)
        {
            bool isMultiple = true;
            try
            {
                _projectService.DeleteProject(projectList);
                isMultiple = projectList.Count == 1;
            }
            catch (ProjectNotExistedException)
            {
                return Json(new DeleteProjectResponseModel()
                {
                    HasError = true,
                    ErrMessage = isMultiple
                    ? Resource.ProjectList.ProjectListRe.ProjectDoesntExist_DeleteError
                    : Resource.ProjectList.ProjectListRe.ProjectDoesntExistMultiple_DeleteError
                });
            }
            catch (CantDeleteProjectBecauseProjectHasBeenChangedException)
            {
                return Json(new DeleteProjectResponseModel()
                {
                    HasError = true,
                    ErrMessage = isMultiple
                    ? Resource.ProjectList.ProjectListRe.ProjectHasLowerVersion_DeleteError
                    : Resource.ProjectList.ProjectListRe.ProjectHasLowerVersionMultiple_DeleteError
                });
            }
            catch (ProjectStatusNotNewException)
            {
                return Json(new DeleteProjectResponseModel()
                {
                    HasError = true,
                    ErrMessage = "Project status invalid"
                });
            }
            return Json(new DeleteProjectResponseModel()
            {
                HasError = false,
                ErrMessage = "No error "
            });
        }
    }
}