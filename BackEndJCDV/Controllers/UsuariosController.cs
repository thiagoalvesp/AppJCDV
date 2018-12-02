using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entity;
using LiteDB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BackEndJCDV.Controllers
{
    [Authorize("Bearer")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UsuariosController : Controller
    {

        private IConfiguration _config;
        private string DATABASE_PATH;
        const string COLLECTION_USUARIOS_NAME = "usuarios";

        public UsuariosController(IConfiguration Configuration)
        {
            _config = Configuration;
            DATABASE_PATH = _config["DataBasePath"];

            if (string.IsNullOrEmpty(DATABASE_PATH))
                DATABASE_PATH = Directory.GetCurrentDirectory() + "\\Data\\Banco.db";
        }

        [HttpGet("{id}/Location")]
        public IActionResult PatchLocation(Guid id)
        {
            try
            {
                using (var db = new LiteRepository(new LiteDatabase(DATABASE_PATH)))
                {
                    var usuario = db.SingleById<Usuario>(id, COLLECTION_USUARIOS_NAME);
                    if (usuario == null)
                        return NotFound();

                    var location = usuario.Location;
                    return Ok(location);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPatch("{id}/Location")]
        public IActionResult PatchLocation(Guid id, [FromBody]Location location)
        {
            try
            {
                using (var db = new LiteRepository(new LiteDatabase(DATABASE_PATH)))
                {
                    var usuario = db.SingleById<Usuario>(id, COLLECTION_USUARIOS_NAME);
                    if (usuario == null)
                        return NotFound();

                    usuario.Location = location;
                    db.Update<Usuario>(usuario, COLLECTION_USUARIOS_NAME);
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                using (var db = new LiteRepository(new LiteDatabase(DATABASE_PATH)))
                {
                    var usuarios = db.Query<Usuario>(COLLECTION_USUARIOS_NAME).ToList();
                    return Ok(usuarios);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            try
            {
                using (var db = new LiteRepository(new LiteDatabase(DATABASE_PATH)))
                {
                    var usuario = db.Query<Usuario>(COLLECTION_USUARIOS_NAME)
                        .Where(x => x.Id == id).SingleOrDefault();

                    if (usuario == null)
                        return NotFound(id);

                    return Ok(usuario);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Post([FromBody]Usuario usuario)
        {
            try
            {
                using (var db = new LiteRepository(new LiteDatabase(DATABASE_PATH)))
                {
                    if (!ModelState.IsValid)
                        return BadRequest(usuario);

                    usuario.Id = db.Insert<Usuario>(usuario, COLLECTION_USUARIOS_NAME).AsGuid;
                    return Ok(usuario);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody]Usuario usuario)
        {
            try
            {
                using (var db = new LiteRepository(new LiteDatabase(DATABASE_PATH)))
                {
                    if (!ModelState.IsValid)
                        return BadRequest(usuario);

                    var usuarioDb = db.Query<Usuario>(COLLECTION_USUARIOS_NAME)
                        .Where(x => x.Id == id).SingleOrDefault();

                    if (usuarioDb == null)
                        return NotFound(id);

                    //desconsidero o id do body
                    usuario.Id = id;

                    db.Update<Usuario>(usuario, COLLECTION_USUARIOS_NAME);
                    return Ok(usuario);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                using (var db = new LiteRepository(new LiteDatabase(DATABASE_PATH)))
                {
                    var success = db.Delete<Usuario>(id, COLLECTION_USUARIOS_NAME);
                    if (success)
                        return Ok();

                    return NotFound();

                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}