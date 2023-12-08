using Data.EF;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Cell.Request;
using ViewModels.Cell.Response;
using ViewModels.Common;
using ViewModels.Dept.Request;
using ViewModels.Dept.Response;
using ViewModels.LogExam.Response;
using ViewModels.LogExamTrinhtuthaotac.Response;
using ViewModels.Model.Request;
using ViewModels.Model.Response;

namespace Application.Dept
{
    public class DeptService : IDeptService
    {
        private readonly TracNghiemCEVDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public DeptService(TracNghiemCEVDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<ApiResult<bool>> Create(DeptCreateRequest request)
        {
            var checkDuplicate = await _context.Depts.Where(x => x.Name == request.Name).CountAsync();
            if (checkDuplicate > 0)
                return new ApiErrorResult<bool>("Tên bộ phận đã bị trùng");
            var newDept = new Data.Entities.Dept()
            {
                Name = request.Name.ToUpper()
            };
            _context.Add(newDept);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<bool>> CreateCell(int ModelId, CellCreateRequest request)
        {
            var checkDuplicate = await _context.Cells.Where(x => (x.ModelId == ModelId && x.Name == request.Name)).CountAsync();
            if (checkDuplicate > 0)
                return new ApiErrorResult<bool>("Tên Cell đã bị trùng");
            var newCell = new Data.Entities.Cell()
            {
                Name = request.Name.ToUpper(),
                ModelId = ModelId
            };
            _context.Add(newCell);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<bool>> CreateModel(int DeptId, ModelCreateRequest request)
        {
            var checkDuplicate = await _context.Models.Where(x => (x.DeptId == DeptId && x.Name == request.Name)).CountAsync();
            if (checkDuplicate > 0)
                return new ApiErrorResult<bool>("Tên Model đã bị trùng");
            var newModel = new Data.Entities.Model()
            {
                Name = request.Name.ToUpper(),
                DeptId = DeptId
            };
            _context.Add(newModel);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<bool>> Delete(int id)
        {
            var dept = await _context.Depts.FindAsync(id);
            if (dept == null)
                return new ApiErrorResult<bool>($"Không tìm thấy bộ phận với id {id}");
            var listModelOfDept = _context.Models.Where(x => x.DeptId == id).ToList();
            foreach (var model in listModelOfDept)
            {
                var listCellOfModel = _context.Cells.Where(x => x.ModelId == model.Id).ToList();
                foreach (var cell in listCellOfModel)
                {
                    var listUserOfCell = _userManager.Users.Include(x => x.Cell).Where(x => x.CellId == cell.Id).ToList();
                    foreach (var user in listUserOfCell)
                    {
                        _context.Remove(user);
                    }
                    _context.Remove(cell);
                }
                _context.Remove(model);
            }
            _context.Remove(dept);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<bool>> DeleteCell(int id)
        {
            var cell = await _context.Cells.FindAsync(id);
            if (cell == null)
                return new ApiErrorResult<bool>($"Không tìm thấy Cell với id {id}");
            var listUserOfCell = _userManager.Users.Include(x => x.Cell).Where(x => x.CellId == id).ToList();
            foreach (var user in listUserOfCell)
            {
                _context.Remove(user);
            }
            _context.Remove(cell);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<bool>> DeleteModel(int id)
        {
            var model = await _context.Models.FindAsync(id);
            if (model == null)
                return new ApiErrorResult<bool>($"Không tìm thấy Model với id {id}");
            var listCellOfModel = _context.Cells.Where(x => x.ModelId == id).ToList();
            foreach (var cell in listCellOfModel)
            {
                var listUserOfCell = _userManager.Users.Include(x => x.Cell).Where(x => x.CellId == cell.Id).ToList();
                foreach (var user in listUserOfCell)
                {
                    _context.Remove(user);
                }
                _context.Remove(cell);
            }
            _context.Remove(model);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<List<DeptVm>>> GetAll()
        {
            var result = await _context.Depts
                .Include(x => x.Models)
                .ThenInclude(x => x.Cells)
                .ThenInclude(x=>x.AppUsers)
                .Select(x => new DeptVm()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Models = x.Models != null
                            ? x.Models.Select(y => new ModelVm
                            {
                                Id = y.Id,
                                DeptId = y.DeptId,
                                Name = y.Name,
                                Cells = y.Cells != null
                                ? y.Cells.Select(z => new ViewModels.Cell.Response.CellVm
                                {
                                    Id = z.Id,
                                    ModelId = z.ModelId,
                                    Name = z.Name,
                                    Users = z.AppUsers != null 
                                    ? z.AppUsers.Select(u=>new ViewModels.Users.Response.UserVm
                                    {
                                        Name = u.Name,
                                        Id =u.Id
                                    }).ToList()
                                    :new List<ViewModels.Users.Response.UserVm>()
                                }).ToList()
                                : new List<ViewModels.Cell.Response.CellVm>()
                            }).ToList()
                            : new List<ModelVm>()
                }).ToListAsync();
            foreach (var item in result)
            {
                int soluongtaikhoan = await _context.Users.Where(x => x.Cell.Model.Dept.Id == item.Id).CountAsync();
                item.Soluongtaikhoan = soluongtaikhoan;
            }
            return new ApiSuccessResult<List<DeptVm>>(result);
        }

        public async Task<ApiResult<List<ModelVm>>> GetAllByDept(int id)
        {
            var result = await _context.Models
                .Include(x => x.Cells)
                .ThenInclude(x => x.AppUsers)
                .Where(x=>x.DeptId == id)
                .Select(y => new ModelVm()
                {
                    Id = y.Id,
                    DeptId = y.DeptId,
                    Name = y.Name,
                    Cells = y.Cells != null
                                ? y.Cells.Select(z => new ViewModels.Cell.Response.CellVm
                                {
                                    Id = z.Id,
                                    ModelId = z.ModelId,
                                    Name = z.Name,
                                    Users = z.AppUsers != null
                                    ? z.AppUsers.Select(u => new ViewModels.Users.Response.UserVm
                                    {
                                        Name = u.Name,
                                        Id = u.Id
                                    }).ToList()
                                    : new List<ViewModels.Users.Response.UserVm>()
                                }).ToList()
                                : new List<ViewModels.Cell.Response.CellVm>()
                }).ToListAsync();
            foreach (var item in result)
            {
                int soluongtaikhoan = await _context.Users.Where(x => x.Cell.Model.Id == item.Id).CountAsync();
                item.Soluongtaikhoan = soluongtaikhoan;
            }
            return new ApiSuccessResult<List<ModelVm>>(result);
        }

        public async Task<ApiResult<List<CellVm>>> GetAllByMOdel(int id)
        {
            var result = await _context.Cells
               .Include(x => x.AppUsers)
               .Where(x => x.ModelId == id)
               .Select(z => new CellVm()
               {
                   Id = z.Id,
                   ModelId = z.ModelId,
                   Name = z.Name,
                   Users = z.AppUsers != null
                                   ? z.AppUsers.Select(u => new ViewModels.Users.Response.UserVm
                                   {
                                       Name = u.Name,
                                       Id = u.Id
                                   }).ToList()
                                   : new List<ViewModels.Users.Response.UserVm>()
               }).ToListAsync();
            foreach (var item in result)
            {
                int soluongtaikhoan = await _context.Users.Where(x => x.Cell.Id == item.Id).CountAsync();
                item.Soluongtaikhoan = soluongtaikhoan;
            }
            return new ApiSuccessResult<List<CellVm>>(result);
        }

        public async Task<ApiResult<bool>> Update(int id, DeptUpdateRequest request)
        {
            var dept = await _context.Depts.FindAsync(id);
            if (dept == null)
                return new ApiErrorResult<bool>($"Không tìm thấy bộ phận với id {id}");
            var checktrungten = await _context.Depts.ToListAsync();
            if (checktrungten.Any(x => x.Name == request.Name))
            {
                return new ApiErrorResult<bool>("Tên bộ phận đã bị trùng");
            }
            dept.Name = request.Name.ToUpper();
            _context.Update(dept);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<bool>> UpdateCell(int id, CellUpdateRequest request)
        {
            var cell = await _context.Cells.FindAsync(id);
            if (cell == null)
                return new ApiErrorResult<bool>($"Không tìm thấy Cell với id {id}");
            var checktrungten = await _context.Cells.Where(x => x.ModelId == cell.ModelId).ToListAsync();
            if (checktrungten.Any(x => x.Name == request.Name))
            {
                return new ApiErrorResult<bool>("Tên Cell tại Model đã bị trùng");
            }
            cell.Name = request.Name.ToUpper();
            _context.Update(cell);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<bool>> UpdateModel(int id, ModelUpdateRequest request)
        {
            var model = await _context.Models.FindAsync(id);
            if (model == null)
                return new ApiErrorResult<bool>($"Không tìm thấy Model với id {id}");
            var checktrungten = await _context.Models.Where(x => x.DeptId == model.DeptId).ToListAsync();
            if(checktrungten.Any(x=>x.Name == request.Name))
            {
                return new ApiErrorResult<bool>("Tên Model tại bộ phận đã bị trùng");
            }
            model.Name = request.Name.ToUpper();
            _context.Update(model);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
    }
}
