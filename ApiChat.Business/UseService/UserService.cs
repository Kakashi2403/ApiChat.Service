using ApiChat.DataAccess.Models;
using ApiChat.Domain.ModelsDTO.User;
using ApiChat.Domain.UnitOfWork;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiChat.Business.UseService
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<List<UserDTO>> GetAll() => _mapper.Map<List<UserDTO>>((await _unitOfWork.Repository<TblUser>().GetAllAsync()).ToList());

        public async Task<List<UserDTO>> GetIsActived() => _mapper.Map<List<UserDTO>>((await _unitOfWork.Repository<TblUser>().GetAllAsync(x => x.IsActive)).ToList());
        public async Task<UserDTO> Update(UserDTO user)
        {
            var response = await _unitOfWork.Repository<TblUser>().GetByIdAsync(user.UserId);

            if (response == null) throw new Exception("El usuario no existe");

            _mapper.Map(user, response);

            await _unitOfWork.Repository<TblUser>().UpdateAsync(response);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<UserDTO>(response);
        }

        public async Task<UserDTO> Delete(UserDelete delete)
        {
            var response = await _unitOfWork.Repository<TblUser>().GetByIdAsync(delete.UserId);

            if (response == null) throw new Exception("El usuario no existe");

            response.IsActive = false;

            await _unitOfWork.Repository<TblUser>().UpdateAsync(response);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<UserDTO>(response);
        }

        public async Task<UserDTO> Create(UserCreate userCreate)
        {
            var response = (await _unitOfWork.Repository<TblUser>().GetAllAsync(x => x.Email == userCreate.Email)).FirstOrDefault();

            if (response != null) throw new Exception("El correo electronico ya tiene asignado un usuario.");

            var Create = _mapper.Map<TblUser>(userCreate);

            await _unitOfWork.Repository<TblUser>().AddAsync(Create);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<UserDTO>(Create);
        }

    }
}
