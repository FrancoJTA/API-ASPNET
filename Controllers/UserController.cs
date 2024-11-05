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
    
    
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var usuario = await _mdb.Users.FirstOrDefaultAsync(a=>a.Id == id);
        
        if(usuario == null)
            return NotFound();
        
        return usuario;
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

        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var Usuario = await _mdb.Users.FindAsync(id);
        if(Usuario == null)
                return NotFound();
        _mdb.Users.Remove(Usuario);
        await _mdb.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
    {
        if (id != user.Id)
            return BadRequest();
        _mdb.Entry(user).State = EntityState.Modified;
        try
        {
            _mdb.Users.Update(user);
            await _mdb.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UserExiste(user.Id))
            {
                return NoContent();
            }
            else
            {
                throw;
            }
        }
        return NoContent();
    }

    private bool UserExiste(int id)
    {
        return _mdb.Users.Any(a => a.Id == id);
    }
}