using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Packaging;
using ViewModels.Common;
using ViewModels.Dept.Request;
using ViewModels.Users.Request;
using WebAPP.Services;

namespace WebAPP.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
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

        //GET:
        public async Task<IActionResult> List()
        {
            ViewBag.thisPage = "Quản lý danh sách tài khoản";
            var listDept = await _deptApiClient.GetAll();
            ViewBag.listDept = listDept.ResultObj.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            return View();
        }
        public async Task<IActionResult> CreateTaiKhoan()
        {
            ViewBag.thisPage = "Thêm mới tài khoản";
            var listDept = await _deptApiClient.GetAll();
            ViewBag.listDept = listDept.ResultObj.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            return View();
        }
        public IActionResult CreateBophan()
        {
            ViewBag.thisPage = "Quản lý danh sách bộ phận";
            return View();
        }
        public async Task<IActionResult> GetListBophan()
        {
            var result = await _deptApiClient.GetAll();
            return PartialView("DeptUser/_listDept", result.ResultObj);
        }
        public async Task<IActionResult> RoleAssign(Guid id)
        {
            var roleAssignRequest = await GetRoleAssignRequest(id);
            return View(roleAssignRequest);
        }

        //POST:
        [HttpPost]
        public async Task<IActionResult> GetListUsers(int id, string keyword, int pageIndex = 1, int pageSize = 10)
        {
            var request = new GetUserPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var result = await _userApiClient.GetuserPaging(id, request);
            return PartialView("DeptUser/_listUser", result.ResultObj);
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

    }
}
