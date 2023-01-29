using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FunBooksAndVideos.Users.Data.Authorization;
using FunBooksAndVideos.Users.DTOs;
using Microsoft.AspNetCore.Mvc; 

namespace FunBooksAndVideos.Users.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        
        public UsersController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
            
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(UserLoginDto loginDto)
        {
            var response = await _authRepository.Login(loginDto);

            if(!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(UserRegisterDto registerDto)
        {
            var response = await _authRepository.Register(registerDto);

            if(!response.Success)
                return BadRequest(response);

            return Ok(response);
        }
    }
}