using CulinarioAPI.Data;
using CulinarioAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CulinarioAPI.Repositories
{
    public class UserCredentialsRepository : IUserCredentialsRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserCredentialsRepository> _logger;

        public UserCredentialsRepository(ApplicationDbContext context, ILogger<UserCredentialsRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<UserCredentials> GetUserByEmailAsync(string email)
        {
            _logger.LogInformation("GetUserByEmailAsync called with email: {Email}", email);

            try
            {
                var user = await _context.UserCredentials.SingleOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    _logger.LogWarning("User not found with email: {Email}", email);
                }
                else
                {
                    _logger.LogInformation("User found with email: {Email}", email);
                }
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting user by email: {Email}", email);
                throw;
            }
        }

        public async Task AddUserAsync(UserCredentials user)
        {
            _logger.LogInformation("AddUserAsync called for email: {Email}", user.Email);

            try
            {
                _context.UserCredentials.Add(user);
                await _context.SaveChangesAsync();
                _logger.LogInformation("User added successfully: {Email}", user.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding user: {Email}", user.Email);
                throw;
            }
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            _logger.LogInformation("UserExistsAsync called with email: {Email}", email);

            try
            {
                var exists = await _context.UserCredentials.AnyAsync(u => u.Email == email);
                _logger.LogInformation("User exists: {Exists} for email: {Email}", exists, email);
                return exists;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while checking if user exists: {Email}", email);
                throw;
            }
        }
    }
}
