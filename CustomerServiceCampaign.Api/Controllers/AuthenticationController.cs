﻿using AutoMapper;
using Contracts;
using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CustomerServiceCampaign.Api;
    
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IRepositoryManager _repository;
        private readonly IAuthenticationManager _authManager; 
        public AuthenticationController (IMapper mapper, UserManager<User> userManager, IRepositoryManager repository, IAuthenticationManager authManager) 
        {
            _mapper = mapper;
            _userManager = userManager;
            _repository = repository;
            _authManager = authManager;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
        {

            if (userForRegistration.Role.ToLower() == "agent") 
            {
                var agent = await _repository.Agent.GetAgentBySsnAsync(userForRegistration.SSN);
                if (agent == null) return BadRequest("Agent SSN is not registered");
            }
            
            var user = _mapper.Map<User>(userForRegistration);
            var result = await _userManager.CreateAsync(user, userForRegistration.Password); 
            if(!result.Succeeded)
            {
                foreach (var error in result.Errors) 
                {
                    ModelState.TryAddModelError(error.Code, error.Description); 
                }

                return BadRequest(ModelState); 
                }

                await _userManager.AddToRoleAsync(user, userForRegistration.Role); 
                
                return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto user)
        {
            if (!await _authManager.ValidateUser(user)) return Unauthorized();
        
            return Ok(new { Token = await _authManager.CreateToken() }); 
        } 
        
    }

