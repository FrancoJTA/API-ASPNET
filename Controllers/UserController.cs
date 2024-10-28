using ApiWeb.Data;
using ApiWeb.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiWeb.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : Controller
{
    private MyDBContext _mdb;

    public UserController(MyDBContext mdb)
    {
        _mdb = mdb;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> Listar()
    {
        return await _mdb.Users.ToListAsync();
    }
    
    [HttpPost]
    public async Task<ActionResult<User>> CreateUser([FromBody] User user)
    {
        if (user == null)
        {
            return BadRequest("User data is null.");
        }

        _mdb.Users.Add(user);
        await _mdb.SaveChangesAsync();

        // Retorna el usuario creado y un código de estado 201 Created
        return CreatedAtAction(nameof(Listar), new { id = user.Id }, user);
    }
}