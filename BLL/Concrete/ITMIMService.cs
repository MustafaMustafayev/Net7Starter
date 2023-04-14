
using AutoMapper;
using BLL.Abstract;
using CORE.Abstract;
using CORE.Localization;
using DAL.UnitOfWorks.Abstract;
using DTO.Responses;
using DTO.ITMIM;
using ENTITIES.Entities;
using DAL.Utility;

namespace BLL.Concrete;

public class ITMIMService : IITMIMService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUtilService _utilService;
    public ITMIMService(IMapper mapper, IUnitOfWork unitOfWork, IUtilService utilService)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _utilService = utilService;
    }

    public async Task<IResult> AddAsync(ITMIMToAddDto dto)
    {
        var data = _mapper.Map<ITMIM>(dto);

        await _unitOfWork.ITMIMRepository.AddAsync(data);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(Messages.Success.Translate());
    }

    public async Task<IResult> SoftDeleteAsync(int id)
    {
        var data = await _unitOfWork.ITMIMRepository.GetAsync(m => m.ITMIMId == id);

        _unitOfWork.ITMIMRepository.SoftDelete(data);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(Messages.Success.Translate());
    }

    public async Task<IDataResult<PaginatedList<ITMIMToListDto>>> GetAsPaginatedListAsync()
    {
        var datas = _unitOfWork.ITMIMRepository.GetList();
        var paginationDto = _utilService.GetPagination();

        var response = await PaginatedList<ITMIM>.CreateAsync(datas.OrderBy(m => m.ITMIMId), paginationDto.PageIndex, paginationDto.PageSize);

        var responseDto = new PaginatedList<ITMIMToListDto>(_mapper.Map<List<ITMIMToListDto>>(response.Datas), response.TotalRecordCount, response.PageIndex, paginationDto.PageSize);

        return new SuccessDataResult<PaginatedList<ITMIMToListDto>>(responseDto, Messages.Success.Translate());
    }

    public async Task<IDataResult<List<ITMIMToListDto>>> GetAsync()
    {
        var datas = _mapper.Map<List<ITMIMToListDto>>(await _unitOfWork.ITMIMRepository.GetListAsync());

        return new SuccessDataResult<List<ITMIMToListDto>>(datas, Messages.Success.Translate());
    }

    public async Task<IDataResult<ITMIMToListDto>> GetAsync(int id)
    {
        var data = _mapper.Map<ITMIMToListDto>(await _unitOfWork.ITMIMRepository.GetAsync(m => m.ITMIMId == id));

        return new SuccessDataResult<ITMIMToListDto>(data, Messages.Success.Translate());
    }

    public async Task<IResult> UpdateAsync(int id, ITMIMToUpdateDto dto)
    {
        var data = _mapper.Map<ITMIM>(dto);
        data.ITMIMId = id;

        _unitOfWork.ITMIMRepository.Update(data);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(Messages.Success.Translate());
    }
}
