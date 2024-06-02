using billing_mangment_system.Models;
using billing_mangment_system_v2.Dtos;
using billing_mangment_system_v2.ICollectionService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Xml.Linq;


[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ICollectionBillService _bills;
    private readonly IConfiguration _config;


    // private readonly Bills  _bill;

    public UserController(IUserService userService, ICollectionBillService bills , IConfiguration config)
    {
        _userService = userService;
        _bills = bills;
        _config = config;
       // _bill = bill;
    }

    private string GenerateToken(User user)
    {
        var secretKey = new SymmetricSecurityKey(
            Encoding.ASCII.GetBytes(
                _config.GetValue<string>("Authentication:Key")));

        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        List<Claim> claims = new();
        claims.Add(new(JwtRegisteredClaimNames.Sub, user.CostumerId.ToString()));




        var token = new JwtSecurityToken(
            _config.GetValue<string>("Authentication:Issuer"),
            _config.GetValue<string>("Authentication:Audience"),
            claims,
            DateTime.Now,
            DateTime.Now.AddMonths(1),
            signingCredentials
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    [HttpPost("token")]
    [AllowAnonymous]
    public async Task<ActionResult<string>> Login([FromBody] UserDto userInput)
    {
        var user = await _userService.GetUserByCustomerIdAsync(userInput.CostumerId);
        //if (user == null) return BadRequest("Wrong username or password");
        if (userInput.CostumerId == string.Empty && userInput.Password== string.Empty)
        {
            return BadRequest("please enter user name and password");
        }

        if (user.CostumerId != userInput.CostumerId     )
            return BadRequest("Wrong username or password");
        if (user.Password != userInput.Password)
            return BadRequest("Wrong username or password");

        var token = GenerateToken(user);

        return Ok(token);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        var users = await _userService.GetUserAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUserById(string id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<User>> CreateUser(User user)
    {
        try

        {
          
            var newUser = await _userService.CreateUserAsync(user);

            var bill = new Bills { CostumerId = newUser.CostumerId,
            PostTime = DateTime.Now,
            PreTime = DateTime.Now,
            PostUnit = 0,
            PreUnit = 0};

            await _bills.CreateBillAsync(bill);
            return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id }, newUser);
        }
        catch (Exception ex)
        {
            // Log the exception
            return StatusCode(500, "An error occurred while creating the user.");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateUser(string id, UpdateUserDto user)

    {

        var users = await _userService.GetUserByIdAsync(id);

        if (id != user.CostumerId)
        {
            return BadRequest("User ID mismatch.");
        }
        try
        {
            var updateUser = new User {
            CostumerId = user.CostumerId,
            Email = user.Email,
            Name = user.Name,
            Password = user.Password,
            Phone = user.Phone,
            Address = user.Address,
            typeAccount = users.typeAccount,
            Id = users.Id
            
            };

            await _userService.UpdateUserById(id, updateUser);
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
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            // Log the exception
            return StatusCode(500, "An error occurred while deleting the user.");
        }
    }


}
