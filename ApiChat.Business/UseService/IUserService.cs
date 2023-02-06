using ApiChat.Domain.ModelsDTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiChat.Business.UseService
{
    public interface IUserService
    {
        Task<List<UserDTO>> GetAll();
        Task<List<UserDTO>> GetIsActived();
        Task<UserDTO> Update(UserDTO user);
        Task<UserDTO> Delete(UserDelete delete);
        Task<UserDTO> Create(UserCreate userCreate);
    }
}
