
using AutoMapper;
using Project.BLL.Abstract;
using Project.Core.Constants;
using Project.DAL.UnitOfWorks.Abstract;
using Project.DTO.DTOs.Responses;
using Project.DTO.DTOs.RoleDTOs;
using Project.Entity.Entities;

namespace Project.BLL.Concrete
{
	public class RoleService : IRoleService
	{
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public RoleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IDataResult<Result>> AddAsync(RoleToAddOrUpdateDTO dto)
        {
            Role entity = _mapper.Map<Role>(dto);
            await _unitOfWork.RoleRepository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return new SuccessDataResult<Result>(null, Messages.Success);
        }

        public async Task DeleteAsync(int id)
        {
            Role entity = await _unitOfWork.RoleRepository.GetAsync(m => m.RoleId == id);
            entity.IsDeleted = true;
            _unitOfWork.RoleRepository.Update(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IDataResult<List<RoleToListDTO>>> GetAsync()
        {
            List<RoleToListDTO> datas = _mapper.Map<List<RoleToListDTO>>(await _unitOfWork.RoleRepository.GetListAsync());
            return new SuccessDataResult<List<RoleToListDTO>>(datas);
        }

        public async Task<IDataResult<RoleToListDTO>> GetAsync(int id)
        {
            RoleToListDTO data =  _mapper.Map<RoleToListDTO>(await _unitOfWork.RoleRepository.GetAsNoTrackingAsync(m => m.RoleId == id));
            return new SuccessDataResult<RoleToListDTO>(data);
        }

        public async Task<IDataResult<Result>> UpdateAsync(int id, RoleToAddOrUpdateDTO dto)
        {
            Role entity = _mapper.Map<Role>(dto);
            entity.RoleId = id;
            _unitOfWork.RoleRepository.Update(entity);
            await _unitOfWork.CommitAsync();
            return new SuccessDataResult<Result>(null, Messages.Success);
        }
    }
}