using billing_mangment_system.Models;
using billing_mangment_system_v2.ICollectionService;
using billing_mangment_system_v2.Models;
using Microsoft.AspNetCore.Mvc;

namespace billing_mangment_system_v2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminUserController : Controller
    {
        private readonly IAdminUser _adminUser;
      



        public AdminUserController(IAdminUser userService)
        {
            _adminUser = userService;
    
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdminUsers>>> GetUsers()
        {
            var users = await _adminUser.GetUserAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AdminUsers>> GetUserById(string id)
        {
            var user = await _adminUser.GetadminUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<AdminUsers>> CreateUser(AdminUsers user)
        {
            try
            {
                var newUser = await _adminUser.CreateAdminUserAsync(user);
                return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id }, newUser);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while creating the user.");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUser(string id, AdminUsers user)
        {
            if (id != user.Id)
            {
                return BadRequest("User ID mismatch.");
            }
            try
            {


                await _adminUser.UpdateUserById(id, user);
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while updating the user.");
            }
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(string id)
        {
            try
            {
                await _adminUser.DeleteUserAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while deleting the user.");
            }
        }
    }
}
