using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Packaging;
using ViewModels.Cell.Request;
using ViewModels.Common;
using ViewModels.Dept.Request;
using ViewModels.Model.Request;
using ViewModels.Users.Request;
using WebAPP.Services;

namespace WebAPP.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "adminpolicy")]
    public class DeptUserController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IDeptApiClient _deptApiClient;
        private readonly IRoleApiClient _roleApiClient;
        public DeptUserController(IUserApiClient userApiClient, IDeptApiClient deptApiClient, IRoleApiClient roleApiClient)
        {
            _userApiClient = userApiClient;
            _deptApiClient = deptApiClient;
            _roleApiClient = roleApiClient;
        }

        public async Task<IActionResult> List()
        {
            ViewBag.thisPage = "Danh sách tài khoản";
            var listDept = await _deptApiClient.GetAll();
            ViewBag.listDept = listDept.ResultObj.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            return View();
        }
        public async Task<IActionResult> ListBophan()
        {
            ViewBag.thisPage = "Danh sách bộ phận";
            var listDept = await _deptApiClient.GetAll();
            ViewBag.listDept = listDept.ResultObj.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            return View();
        }
        public async Task<IActionResult> GetListBophan()
        {
            var result = await _deptApiClient.GetAll();
            return PartialView("DeptUser/_listDept", result.ResultObj);
        }
        public async Task<IActionResult> Createbophan()
        {
            ViewBag.thisPage = "Thêm bộ phận";
            var listDept = await _deptApiClient.GetAll();
            ViewBag.listDept = listDept.ResultObj.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetListModel(int id)
        {
            var result = await _deptApiClient.GetAllByDept(id);
            ViewBag.selectmodellist = result.ResultObj.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            return PartialView("DeptUser/_listModel", result.ResultObj);
        }

        public async Task<IActionResult> RoleAssign(Guid id)
        {
            var roleAssignRequest = await GetRoleAssignRequest(id);
            return View(roleAssignRequest);
        }
        public async Task<IActionResult> Count(int id)
        {
            var result = await _userApiClient.Count(id);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false, message = result.Message });
            }
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> CreateModel(int id, ModelCreateRequest request)
        {
            var result = await _deptApiClient.CreateModel(id, request);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false, message = result.Message });
            }
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> GetListUsers(int id)
        {
            var result = await _userApiClient.GetAllByDeptId(id);
            return PartialView("DeptUser/_listUser", result.ResultObj);
        }
     
        [HttpPost]
        public async Task<IActionResult> RoleAssign(RoleAssignRequest request)
        {
            var result = await _userApiClient.RoleAssign(request.Id, request);
            if (result.IsSuccessed)
            {
                return RedirectToAction("Index");
            }
            var roleAssignRequest = GetRoleAssignRequest(request.Id);
            return View(roleAssignRequest);
        }
        [HttpPost]
        public async Task<IActionResult> CreateBophan(DeptCreateRequest request)
        {
            var result = await _deptApiClient.Create(request);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false, message = result.Message });
            }
            return Json(new { success = true });
        }
        [HttpPost]
        public async Task<IActionResult> UpdateDeptInfo(int id, DeptUpdateRequest request)
        {
            var result = await _deptApiClient.Update(id, request);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false, message = result.Message });
            }
            return Json(new { success = true });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteDeptAndUsers(int id)
        {
            var result = await _deptApiClient.Delete(id);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false, message = result.Message });
            }
            return Json(new { success = true });
        }
        [HttpPost]
        public async Task<IActionResult> Phanquyen(Guid id, UserPhanquyenRequest request)
        {
            var result = await _userApiClient.Phanquyen(id, request);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false, message = result.Message });
            }
            return Json(new { success = true });
        }
        
        [HttpPost]
        public async Task<IActionResult> UpdateUserInfo(Guid id, UserUpdateRequest request)
        {
            var result = await _userApiClient.Update(id, request);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false, message = result.Message });
            }
            return Json(new { success = true });
        }
        [HttpPost]
        public async Task<IActionResult> ResetUserPassword(Guid id, UserResetPasswordRequest request)
        {
            var result = await _userApiClient.ResetPassword(id, request);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false, message = result.Message });
            }
            return Json(new { success = true });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var result = await _userApiClient.Delete(id);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false, message = result.Message });
            }
            return Json(new { success = true });
        }

        //PRIVATE:
        private async Task<RoleAssignRequest> GetRoleAssignRequest(Guid id)
        {
            var user = await _userApiClient.GetById(id);
            var roles = await _roleApiClient.GetAll();
            var roleAssignRequest = new RoleAssignRequest();
            foreach (var role in roles.ResultObj)
            {
                roleAssignRequest.Roles.Add(new SelectItem()
                {
                    Id = role.Id,
                    Name = role.Name,
                    Selected = user.ResultObj.Roles.Contains(role.Name) //Nếu contain thì true, ngược lại thì false
                });
            }
            return roleAssignRequest;
        }


        [HttpPost]
        public async Task<IActionResult> EditModelName(int id, ModelUpdateRequest request)
        {
            var result = await _deptApiClient.UpdateModel(id, request);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false, message = result.Message });
            }
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteModel(int id)
        {
            var result = await _deptApiClient.DeleteModel(id);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false, message = result.Message });
            }
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> CreateCell(int id, CellCreateRequest request)
        {
            var result = await _deptApiClient.CreateCell(id, request);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false, message = result.Message });
            }
            return Json(new { success = true });
        }

        public async Task<IActionResult> GetListCell(int id)
        {
            var result = await _deptApiClient.GetAllByModel(id);
            return PartialView("DeptUser/_listCell", result.ResultObj);
        }

        [HttpPost]
        public async Task<IActionResult> EditCellName(int id, CellUpdateRequest request)
        {
            var result = await _deptApiClient.UpdateCell(id, request);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false, message = result.Message });
            }
            return Json(new { success = true });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteCell(int id)
        {
            var result = await _deptApiClient.DeleteCell(id);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false, message = result.Message });
            }
            return Json(new { success = true });
        }


        //CREATE NEW USER

        public async Task<IActionResult> GetSelectListDept()
        {
            var listDept = await _deptApiClient.GetAll();
            var result = listDept.ResultObj.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> GetSelectListModel(int deptId)
        {
            var listModel = await _deptApiClient.GetAllByDept(deptId);
            var result = listModel.ResultObj.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> GetSelectListCelll(int modelId)
        {
            var listCell = await _deptApiClient.GetAllByModel(modelId);
            var result = listCell.ResultObj.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            return Json(result);
        }

        public async Task<IActionResult> CreateTaiKhoan()
        {
            ViewBag.thisPage = "Thêm mới tài khoản";
           
            var listRoles = await _roleApiClient.GetAll();
            ViewBag.listRoles = listRoles.ResultObj.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RegisterRequest request)
        {
            var result = await _userApiClient.Create(request);
            if (!result.IsSuccessed)
            {
                return Json(new { success = false, message = result.Message });
            }
            return Json(new { success = true });
        }

        //EDIT USER
        [HttpPost]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var result = await _userApiClient.GetById(id);
            var currentDeptName = result.ResultObj.Dept;
            var currentModelName = result.ResultObj.Model;
            var currentCellName = result.ResultObj.Cell;
            ViewBag.currentDeptName = currentDeptName;
            ViewBag.currentModelName = currentModelName;
            ViewBag.currentCellName = currentCellName;
            return PartialView("DeptUser/_userInfoById", result.ResultObj);
        }


        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> ImportExcel([FromForm] IFormFile file, string role, int cellId)
        {
            using (var fileStream = file.OpenReadStream())
            {
                var result = await _userApiClient.ImportExcel(fileStream, role, cellId);
                if (result.IsSuccessed)
                {
                    return Json(new { success = true, data = result.ResultObj });
                }
                return Json(new { success = false, message = result.Message });
            }
        }
    }
}
