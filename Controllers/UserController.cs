using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartCafeOrderingSystem_Api_V2.Context;
using SmartCafeOrderingSystem_Api_V2.Models;
using Microsoft.EntityFrameworkCore;
using SmartCafeOrderingSystem_Api_V2.DTOs;



namespace SmartCafeOrderingSystem_Api_V2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext dbContext;
        public UserController(AppDbContext appDbContext)
        {
            dbContext = appDbContext;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody] UserDTO userObj)
        {
            if (userObj == null)
            {
                return BadRequest(new { Message = "Invalid login data" });
            }

            var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Username == userObj.Username && x.PasswordHash == userObj.PasswordHash);
            if (user == null)
            {
                return NotFound(new { status = false, Message = "User not Found" });
            }

            return Ok(new
            {
                status = true,
                Message = "Logged in!",
                data = new UserDTO
                {
                    UserID = user.UserID,
                    Username = user.Username,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Role = user.Role,
                    CreatedDate = user.CreatedDate,
                    UpdatedDate = user.UpdatedDate
                },
                admin = user.Role != 0,
                username = user.Username
            });
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUpUser([FromBody] UserDTO userObj)
        {
            if (userObj == null || string.IsNullOrWhiteSpace(userObj.Username) || string.IsNullOrWhiteSpace(userObj.Email))
            {
                return BadRequest(new { Message = "Invalid user data" });
            }

            var existingUser = await dbContext.Users.FirstOrDefaultAsync(x => x.Username == userObj.Username);
            if (existingUser != null)
            {
                return BadRequest(new { Message = "User Already Exists" });
            }

            var newUser = new User
            {
                Username = userObj.Username,
                Email = userObj.Email,
                PhoneNumber = userObj.PhoneNumber,
                PasswordHash = userObj.PasswordHash, // Ensure proper hashing before storing
                Role = 0, // Default role as customer
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };

            await dbContext.Users.AddAsync(newUser);
            await dbContext.SaveChangesAsync();

            return Ok(new { Message = "New User Registered" });
        }

        [HttpDelete("delete/{userId}")]
        public async Task<IActionResult> DeleteUser(int userId, [FromQuery] int requestingUserId)
        {
            var requestingUser = await dbContext.Users.FindAsync(requestingUserId);
            if (requestingUser == null)
            {
                return Unauthorized(new { Message = "Invalid Requesting User" });
            }

            var userToDelete = await dbContext.Users.FindAsync(userId);
            if (userToDelete == null)
            {
                return NotFound(new { Message = "User not found" });
            }

            if (requestingUser.Role == 0 && requestingUser.UserID != userToDelete.UserID)
            {
                return Forbid();
            }

            dbContext.Users.Remove(userToDelete);
            await dbContext.SaveChangesAsync();

            return Ok(new { Message = "User deleted successfully" });
        }
    }
}
