using AutoMapper;
using BLL.Abstract;
using CORE.Abstract;
using CORE.Localization;
using DTO.Responses;
using DTO.Test;
using ENTITIES.Entities;
using DAL.EntityFramework.Utility;
using DAL.EntityFramework.UnitOfWork;

namespace BLL.Concrete;

public class TestService : ITestService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUtilService _utilService;
    public TestService(IMapper mapper, IUnitOfWork unitOfWork, IUtilService utilService)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _utilService = utilService;
    }

    public async Task<IResult> AddAsync(TestToAddDto dto)
    {
        var data = _mapper.Map<Test>(dto);

        await _unitOfWork.TestRepository.AddAsync(data);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(Messages.Success.Translate());
    }

    public async Task<IResult> SoftDeleteAsync(Guid id)
    {
        var data = await _unitOfWork.TestRepository.GetAsync(m => m.Id == id);

        _unitOfWork.TestRepository.SoftDelete(data);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(Messages.Success.Translate());
    }

    public async Task<IDataResult<PaginatedList<TestToListDto>>> GetAsPaginatedListAsync()
    {
        var datas = _unitOfWork.TestRepository.GetList();
        var paginationDto = _utilService.GetPagination();

        var response = await PaginatedList<Test>.CreateAsync(datas.OrderBy(m => m.Id), paginationDto.PageIndex, paginationDto.PageSize);

        var responseDto = new PaginatedList<TestToListDto>(_mapper.Map<List<TestToListDto>>(response.Datas), response.TotalRecordCount, response.PageIndex, paginationDto.PageSize);

        return new SuccessDataResult<PaginatedList<TestToListDto>>(responseDto, Messages.Success.Translate());
    }

    public async Task<IDataResult<List<TestToListDto>>> GetAsync()
    {
        var datas = _mapper.Map<List<TestToListDto>>(await _unitOfWork.TestRepository.GetListAsync());

        return new SuccessDataResult<List<TestToListDto>>(datas, Messages.Success.Translate());
    }

    public async Task<IDataResult<TestToListDto>> GetAsync(Guid id)
    {
        var data = _mapper.Map<TestToListDto>(await _unitOfWork.TestRepository.GetAsync(m => m.Id == id));

        return new SuccessDataResult<TestToListDto>(data, Messages.Success.Translate());
    }

    public async Task<IResult> UpdateAsync(Guid id, TestToUpdateDto dto)
    {
        var data = _mapper.Map<Test>(dto);
        data.Id = id;

        _unitOfWork.TestRepository.Update(data);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(Messages.Success.Translate());
    }
}
