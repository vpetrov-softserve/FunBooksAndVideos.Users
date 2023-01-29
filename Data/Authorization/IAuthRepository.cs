using FunBooksAndVideos.Users.DTOs;
using FunBooksAndVideos.Users.Models;

namespace FunBooksAndVideos.Users.Data.Authorization
{
    public interface IAuthRepository
    {
        Task<ServiceResponse<Guid>> Register(UserRegisterDto registerDto);
        Task<ServiceResponse<string>> Login(UserLoginDto loginDto);
    }
}