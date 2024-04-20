using AutoMapper;
using BLL.Abstract;
using CORE.Abstract;
using CORE.Localization;
using DTO.Responses;
using DTO.Department;
using ENTITIES.Entities.Structures;
using DAL.EntityFramework.Utility;
using DAL.EntityFramework.UnitOfWork;

namespace BLL.Concrete;

public class DepartmentService : IDepartmentService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUtilService _utilService;
    public DepartmentService(IMapper mapper, IUnitOfWork unitOfWork, IUtilService utilService)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _utilService = utilService;
    }

    public async Task<IResult> AddAsync(DepartmentCreateRequestDto dto)
    {
        var data = _mapper.Map<Department>(dto);

        await _unitOfWork.DepartmentRepository.AddAsync(data);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(Messages.Success.Translate());
    }

    public async Task<IResult> SoftDeleteAsync(Guid id)
    {
        var data = await _unitOfWork.DepartmentRepository.GetAsync(m => m.Id == id);

        _unitOfWork.DepartmentRepository.SoftDelete(data);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(Messages.Success.Translate());
    }

    public async Task<IDataResult<PaginatedList<DepartmentResponseDto>>> GetAsPaginatedListAsync()
    {
        var datas = _unitOfWork.DepartmentRepository.GetList();
        var paginationDto = _utilService.GetPagination();

        var response = await PaginatedList<Department>.CreateAsync(datas.OrderBy(m => m.Id), paginationDto.PageIndex, paginationDto.PageSize);

        var responseDto = new PaginatedList<DepartmentResponseDto>(_mapper.Map<List<DepartmentResponseDto>>(response.Datas), response.TotalRecordCount, response.PageIndex, paginationDto.PageSize);

        return new SuccessDataResult<PaginatedList<DepartmentResponseDto>>(responseDto, Messages.Success.Translate());
    }

    public async Task<IDataResult<List<DepartmentResponseDto>>> GetAsync()
    {
        var datas = _mapper.Map<List<DepartmentResponseDto>>(await _unitOfWork.DepartmentRepository.GetListAsync());

        return new SuccessDataResult<List<DepartmentResponseDto>>(datas, Messages.Success.Translate());
    }

    public async Task<IDataResult<DepartmentByIdResponseDto>> GetAsync(Guid id)
    {
        var data = _mapper.Map<DepartmentByIdResponseDto>(await _unitOfWork.DepartmentRepository.GetAsync(m => m.Id == id));

        return new SuccessDataResult<DepartmentByIdResponseDto>(data, Messages.Success.Translate());
    }

    public async Task<IResult> UpdateAsync(Guid id, DepartmentUpdateRequestDto dto)
    {
        var data = _mapper.Map<Department>(dto);
        data.Id = id;

        _unitOfWork.DepartmentRepository.Update(data);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(Messages.Success.Translate());
    }
}