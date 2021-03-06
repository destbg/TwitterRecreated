﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.ViewModels;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Identity
{
    public class UserManagerService : IUserManager
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IDateTime _dateTime;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserManagerService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IDateTime dateTime, IMapper mapper, IConfiguration configuration)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _dateTime = dateTime ?? throw new ArgumentNullException(nameof(dateTime));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<(Result Result, AuthVm Auth)> LoginUserAsync(string username, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(username, password, true, false);
            var user = await _userManager.FindByNameAsync(username);

            return (
                result.ToApplicationResult(),
                new AuthVm
                {
                    User = _mapper.Map<AuthUserVm>(user),
                    Token = CreateJwtToken(user)
                }
            );
        }

        public async Task<(Result Result, AuthVm Auth)> CreateUserAsync(string username, string email, string password)
        {
            if (await VerifyUserDoesNotExist(username, email))
                return (Result.Failure(), default);

            var dateNow = _dateTime.Now;
            var user = new AppUser
            {
                UserName = username,
                Email = email,
                DisplayName = username,
                LastLogin = dateNow,
                JoinedOn = dateNow,
                Description = "",
                Image = "assets/avatar/default.jpg",
                Thumbnail = "assets/thumbnail/default.jpg"
            };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
                await _signInManager.SignInAsync(user, true);

            return (
                result.ToApplicationResult(),
                new AuthVm
                {
                    User = _mapper.Map<AuthUserVm>(user),
                    Token = CreateJwtToken(user)
                }
            );
        }

        public async Task<Result> DeleteUserAsync(string userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);

            return user != null ? await DeleteUserAsync(user) : Result.Success();
        }

        public async Task<Result> DeleteUserAsync(AppUser user)
        {
            var result = await _userManager.DeleteAsync(user);

            return result.ToApplicationResult();
        }

        public Task<AppUser> GetUserByUsername(string username) =>
            _userManager.FindByNameAsync(username);

        public Task<AppUser> GetUserById(string id) =>
            _userManager.FindByIdAsync(id);

        public Task<UserVm> GetUserViewModel(string username, string userId) =>
            _userManager.Users
                .Where(f => f.NormalizedUserName == NormalizeName(username))
                .ProjectTo<UserVm>(_mapper.ConfigurationProvider, new { userId })
                .FirstOrDefaultAsync();

        public Task<List<UserFollowVm>> SeachUsers(string search, string userId)
        {
            var normalizedName = '%' + NormalizeName(search) + '%';
            return _userManager.Users
                .Where(f => EF.Functions.Like(f.NormalizedUserName, normalizedName) ||
                    EF.Functions.Like(f.DisplayName, normalizedName))
                .ProjectTo<UserFollowVm>(_mapper.ConfigurationProvider, new { userId })
                .ToListAsync();
        }

        public Task<List<AppUser>> ValidateUsersnames(IEnumerable<string> usernames) =>
            _userManager.Users
                .Where(f => usernames.Contains(f.UserName))
                .ToListAsync();

        public Task<AppUser> GetCurrentUser(string username) =>
            _userManager.FindByNameAsync(username);

        public async Task<Result> UpdateUser(AppUser user)
        {
            var result = await _userManager.UpdateAsync(user);
            return result.ToApplicationResult();
        }

        public string NormalizeName(string name) =>
            _userManager.NormalizeName(name);

        private async Task<bool> VerifyUserDoesNotExist(string username, string email) =>
            await _userManager.FindByNameAsync(username) != null ||
            await _userManager.FindByEmailAsync(email) != null;

        private string CreateJwtToken(AppUser user)
        {
            if (user == default)
                return default;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims: new[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Email, user.Email)
                },
                expires: DateTime.Now.AddDays(int.Parse(_configuration["Jwt:ExpiryInDays"])),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
