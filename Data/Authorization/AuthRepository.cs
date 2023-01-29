using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FunBooksAndVideos.Users.Common;
using FunBooksAndVideos.Users.Data.Cryptography;
using FunBooksAndVideos.Users.DTOs;
using FunBooksAndVideos.Users.Models;
using Microsoft.EntityFrameworkCore;

namespace FunBooksAndVideos.Users.Data.Authorization
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ICryptographer _cryptographer;
        private readonly DataContext _dataContext;

        private readonly IMapper _mapper;

        public AuthRepository(DataContext dataContext, ICryptographer cryptographer, IMapper mapper)
        {
            _dataContext = dataContext;
            _cryptographer = cryptographer;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<string>> Login(UserLoginDto loginDto)
        {
            var response = new ServiceResponse<string>();

            var user = 
                await _dataContext.Users.FirstOrDefaultAsync(u => u.UserName.ToLower() == loginDto.UserName.ToLower());

            if(user == null || (user != null 
                    && !_cryptographer.VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt)))
            {
                response.Success = false;
                response.Message = Messages.UserLoginIncorrect;
            }
            else
            {
                response.Success = true;
                response.Data = _cryptographer.CreateToken(user);
            }

            return response; 

        }

        public async Task<ServiceResponse<Guid>> Register(UserRegisterDto registerDto)
        {
            var response = new ServiceResponse<Guid>();
            
            if (!await UserExists(registerDto.UserName))
            {
                var user = new User();
                user = _mapper.Map<User>(registerDto);

                var passwordStore = _cryptographer.CreatePasswordHash(registerDto.Password);


                user.PasswordHash = passwordStore.PasswordHash;
                user.PasswordSalt = passwordStore.PasswordSalt;
                _dataContext.Add(user);
                await _dataContext.SaveChangesAsync();
                response.Data = user.UserId;
                response.Success = true;
            }
            else
            {
                response.Data = Guid.Empty;
                response.Message = Messages.UserExists;
            }
            
             return response;
        }

        private async Task<bool> UserExists(string userName)
        {
            return await _dataContext.Users.AnyAsync(u => u.UserName.ToLower() == userName.ToLower());
        }
    }
}