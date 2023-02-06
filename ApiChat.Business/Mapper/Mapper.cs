using ApiChat.DataAccess.Models;
using ApiChat.Domain.ModelsDTO.User;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiChat.Business.Mapper
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<TblUser, UserDTO>();
            CreateMap<UserDTO, TblUser>();
            CreateMap<UserCreate, TblUser>();
            CreateMap<TblUser, UserCreate>();
        }
    }
}
