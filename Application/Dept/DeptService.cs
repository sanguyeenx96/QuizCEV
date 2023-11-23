﻿using Data.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.Dept.Request;
using ViewModels.Dept.Response;

namespace Application.Dept
{
    public class DeptService : IDeptService
    {
        private readonly TracNghiemCEVDbContext _context;
        public DeptService(TracNghiemCEVDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<bool>> Create(DeptCreateRequest request)
        {
            var checkDuplicate = await _context.Depts.Where(x => x.Name == request.Name).CountAsync();
            if (checkDuplicate > 0)
                return new ApiErrorResult<bool>("Tên bộ phận đã bị trùng");
            var newDept = new Data.Entities.Dept()
            {
                Name = request.Name
            };
            _context.Add(newDept);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<bool>> Delete(int id)
        {
            var dept = await _context.Depts.FindAsync(id);
            if (dept == null)
                return new ApiErrorResult<bool>($"Không tìm thấy bộ phận với id {id}");
            _context.Remove(dept);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<List<DeptVm>>> GetAll()
        {
            var result = await _context.Depts.Select(x => new DeptVm()
            {
                Id = x.Id,
                Name = x.Name,
            }).ToListAsync();
            return new ApiSuccessResult<List<DeptVm>>(result);
        }

        public async Task<ApiResult<bool>> Update(int id, DeptUpdateRequest request)
        {
            var dept = await _context.Depts.FindAsync(id);
            if (dept == null)
                return new ApiErrorResult<bool>($"Không tìm thấy bộ phận với id {id}");
            dept.Name = request.Name;
            _context.Update(dept);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
    }
}
