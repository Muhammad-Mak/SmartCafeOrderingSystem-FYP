using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartCafeOrderingSystem_Api_V2.Context;
using SmartCafeOrderingSystem_Api_V2.Models;
using Microsoft.EntityFrameworkCore;



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
        public async Task<IActionResult> Authenticate([FromBody] User userObj)
        {
            if (userObj == null)
            {
                return BadRequest();
            }

            var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Username == userObj.Username && x.PasswordHash == userObj.PasswordHash);
            if (user == null)
            {
                return NotFound(new { status = false, Message = "User not Found" });
            }
            if (user.Role == 0)
            {
                return Ok(new
                {
                    status = true,
                    Message = "Logged in!",
                    data = user,
                    admin = false,
                    username = userObj.Username
                });
            }
            return Ok(new
            {
                status = true,
                Message = "Logged in!",
                data = user,
                admin = true,
                username = userObj.Username
            });

        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUpUser([FromBody] User userObj)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Username == userObj.Username);
            if (userObj == null)
            {
                return BadRequest();
            }
            else if (user != null)
            {
                return BadRequest(new { Message = "User Already Exists" });
            }


            await dbContext.Users.AddAsync(userObj);

            await dbContext.SaveChangesAsync();
            return Ok(new
            {
                Message = "New User Registered"
            });
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

            // Regular user can only delete their own account
            if (requestingUser.Role == 0 && requestingUser.UserID != userToDelete.UserID)
            {
                return Forbid("You can only delete your own account.");
            }

            // Admin can delete any user account
            dbContext.Users.Remove(userToDelete);
            await dbContext.SaveChangesAsync();

            return Ok(new { Message = "User deleted successfully" });
        }
    }
}
