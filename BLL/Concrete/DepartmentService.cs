using AutoMapper;
using BLL.Abstract;
using CORE.Localization;
using DAL.EntityFramework.UnitOfWork;
using DAL.EntityFramework.Utility;
using DTO.Department;
using DTO.Responses;
using ENTITIES.Entities;

namespace BLL.Concrete;

public class DepartmentService(IMapper mapper,
                               IUnitOfWork unitOfWork) : IDepartmentService
{
    private readonly IMapper _mapper = mapper;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<IResult> AddAsync(DepartmentCreateRequestDto dto)
    {
        var data = _mapper.Map<Department>(dto);

        await _unitOfWork.DepartmentRepository.AddAsync(data);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(EMessages.Success.Translate());
    }

    public async Task<IResult> SoftDeleteAsync(Guid id)
    {
        var data = await _unitOfWork.DepartmentRepository.GetAsync(m => m.Id == id);
        if (data is not null)
        {
            _unitOfWork.DepartmentRepository.SoftDelete(data);
            await _unitOfWork.CommitAsync();
        }

        return new SuccessResult(EMessages.Success.Translate());
    }

    public async Task<IDataResult<PaginatedList<DepartmentResponseDto>>> GetAsPaginatedListAsync(int pageIndex, int pageSize)
    {
        var datas = _unitOfWork.DepartmentRepository.GetList();

        var response = await PaginatedList<Department>.CreateAsync(datas.OrderBy(m => m.Id), pageIndex, pageSize);

        var responseDto = new PaginatedList<DepartmentResponseDto>(_mapper.Map<List<DepartmentResponseDto>>(response.Datas), response.TotalRecordCount, response.PageIndex, pageSize);

        return new SuccessDataResult<PaginatedList<DepartmentResponseDto>>(responseDto, EMessages.Success.Translate());
    }

    public async Task<IDataResult<IEnumerable<DepartmentResponseDto>>> GetAsync()
    {
        var datas = _mapper.Map<IEnumerable<DepartmentResponseDto>>(await _unitOfWork.DepartmentRepository.GetListAsync());

        return new SuccessDataResult<IEnumerable<DepartmentResponseDto>>(datas, EMessages.Success.Translate());
    }

    public async Task<IDataResult<DepartmentByIdResponseDto>> GetAsync(Guid id)
    {
        var data = _mapper.Map<DepartmentByIdResponseDto>(await _unitOfWork.DepartmentRepository.GetAsync(m => m.Id == id));

        return new SuccessDataResult<DepartmentByIdResponseDto>(data, EMessages.Success.Translate());
    }

    public async Task<IResult> UpdateAsync(Guid id, DepartmentUpdateRequestDto dto)
    {
        var data = _mapper.Map<Department>(dto);
        data.Id = id;

        _unitOfWork.DepartmentRepository.Update(data);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(EMessages.Success.Translate());
    }
}