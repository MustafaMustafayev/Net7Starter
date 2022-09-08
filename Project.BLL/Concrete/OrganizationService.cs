using AutoMapper;
using Project.BLL.Abstract;
using Project.Core.Constants;
using Project.DAL.UnitOfWorks.Abstract;
using Project.DTO.DTOs.OrganizationDTOs;
using Project.DTO.DTOs.Responses;
using Project.Entity.Entities;

namespace Project.BLL.Concrete
{
	public class OrganizationService : IOrganizationService
	{

        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public OrganizationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IDataResult<Result>> AddAsync(OrganizationToAddOrUpdateDTO dto)
        {
            Organization entity = _mapper.Map<Organization>(dto);
            await _unitOfWork.OrganizationRepository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return new SuccessDataResult<Result>(null, Messages.Success);
        }

        public async Task DeleteAsync(int id)
        {
            Organization entity = await _unitOfWork.OrganizationRepository.GetAsync(m => m.OrganizationId == id);
            entity.IsDeleted = true;
            _unitOfWork.OrganizationRepository.Update(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IDataResult<List<OrganizationToListDTO>>> GetAsync()
        {
            List<OrganizationToListDTO> datas = _mapper.Map<List<OrganizationToListDTO>>(await _unitOfWork.OrganizationRepository.GetListAsync());
            return new SuccessDataResult<List<OrganizationToListDTO>>(datas);
        }

        public async Task<IDataResult<OrganizationToListDTO>> GetAsync(int id)
        {
            OrganizationToListDTO data = _mapper.Map<OrganizationToListDTO>(await _unitOfWork.OrganizationRepository.GetAsNoTrackingAsync(m => m.OrganizationId == id));
            return new SuccessDataResult<OrganizationToListDTO>(data);
        }

        public async Task<IDataResult<Result>> UpdateAsync(int id, OrganizationToAddOrUpdateDTO dto)
        {
            Organization entity = _mapper.Map<Organization>(dto);
            entity.OrganizationId = id;
            _unitOfWork.OrganizationRepository.Update(entity);
            await _unitOfWork.CommitAsync();
            return new SuccessDataResult<Result>(null, Messages.Success);
        }
    }
}

