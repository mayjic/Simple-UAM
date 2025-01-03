using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Models;
using System.Collections.Generic;
using System.Linq;

namespace UserManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Requires authentication for all actions in this controller
    public class UserController : ControllerBase
    {
        private static List<User> Users = new List<User>
        {
                new User { Id = 1, Username = "admin", Email = "admin@example.com", Role = "Admin" },
                new User { Id = 2, Username = "user1", Email = "user1@example.com", Role = "User" }
        };
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            return Ok(Users);
        }

        // GET: api/user/{id}
        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound("User not found");

            return Ok(user);
        }

        // POST: api/user
        [HttpPost]
        public IActionResult AddUser([FromBody] User newUser)
        {
            if (newUser == null)
                return BadRequest("Invalid user data");

            newUser.Id = Users.Count > 0 ? Users.Max(u => u.Id) + 1 : 1; // Auto-increment ID
            Users.Add(newUser);
            return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id }, newUser);
        }

        // PUT: api/user/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] User updatedUser)
        {
            var user = Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound("User not found");

            user.Username = updatedUser.Username;
            user.Email = updatedUser.Email;
            user.Role = updatedUser.Role;
            return Ok( new { Message = "User Updated"});
        }

        // DELETE: api/user/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound("User not found");

            Users.Remove(user);
            return NoContent();
        }
    }
}

