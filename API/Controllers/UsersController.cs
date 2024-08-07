﻿using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Authorize] // we dont want [AllowAnonymous] on the top otherwise [Authorize] on the below will be ignored
public class UsersController : BaseApiController
{

  private readonly IUserRepository _userRepository;
  // private readonly DataContext _context;
  private readonly IMapper _mapper;
  // public UsersController(DataContext context)
  public UsersController(IUserRepository userRepository, IMapper mapper)
  {
    _userRepository = userRepository;
    _mapper = mapper;
  }

  // [AllowAnonymous]
  [HttpGet] // /api/users/
  public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
  {
    var users = await _userRepository.GetMembersAsync();

    // var usersToReturn = _mapper.Map<IEnumerable<MemberDto>>(users);

    return Ok(users);
    // return await _context.Users.ToListAsync();
  }

  [HttpGet("{username}")]// /api/users/{username}
  public async Task<ActionResult<MemberDto>> GetUser(string username)
  {
    return await _userRepository.GetMemberAsync(username);

    // return await _context.Users.FindAsync(id);
  }

  [HttpPut]
  public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
  {
    var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    var user = await _userRepository.GetUserByUsernameAsync(username);

    if(user == null) return NotFound();

    _mapper.Map(memberUpdateDto, user);

    if(await _userRepository.SaveAllAsync()) return NoContent(); // return 204

    return BadRequest("Failed to update user");
  }
}
