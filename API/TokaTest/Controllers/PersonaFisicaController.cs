using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TokaTest.Models;

namespace TokaTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonaFisicaController : ControllerBase
    {
        public readonly masterContext _dbContext;

        public PersonaFisicaController(masterContext _context)
        {
            _dbContext = _context;
        }


        [HttpGet]
        [Route ("ObtenerPeronasFisicas")]
        public IActionResult ObtenerPeronasFisicas()
        {
            List<PersonasFisica> lista = new List<PersonasFisica>();
            try
            {
                lista = _dbContext.PersonasFisicas.ToList();

                return StatusCode(StatusCodes.Status200OK, new { message = "OK", Response = lista });

            }catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = ex.Message, Response = lista });
            }
        }

        [HttpGet]
        [Route("ObtenerPeronasFisicas/{idPersonaFisica:int}")]
        public IActionResult ObtenerPeronasFisicas(int idPersonaFisica)
        {
            PersonasFisica personasFisica = _dbContext.PersonasFisicas.Find(idPersonaFisica);
            if (personasFisica == null)
            {
                return BadRequest("Persona Fisica no se enocontró");
            }
            try
            {
                personasFisica = _dbContext.PersonasFisicas.Where(p => p.IdPersonaFisica == idPersonaFisica).FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, new { message = "OK", Response = personasFisica });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = ex.Message, Response = personasFisica });
            }
        }

        [HttpPost]
        [Route("RegistrarPersonaFisica")]
        public IActionResult RegistrarPersonaFisica([FromBody] PersonasFisica personaF)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@Nombre", personaF.Nombre),
                    new SqlParameter("@ApellidoPaterno", personaF.ApellidoPaterno),
                    new SqlParameter("@ApellidoMaterno", personaF.ApellidoMaterno),
                    new SqlParameter("@RFC", personaF.Rfc),
                    new SqlParameter("@FechaNacimiento", personaF.FechaNacimiento),
                    new SqlParameter("@UsuarioAgrega", personaF.UsuarioAgrega)
                };

                //_dbContext.Database.ExecuteSqlRaw
                // var message = new SqlParameter("@MENSAJEERROR", SqlDbType.VarChar, 500) { Direction = ParameterDirection.Output };
                // parameters.Append(message);
                _dbContext.Database.ExecuteSqlRaw("EXEC dbo.sp_AgregarPersonaFisica @Nombre, @ApellidoPaterno, @ApellidoMaterno, @RFC, @FechaNacimiento, @UsuarioAgrega", parameters);

                return StatusCode(StatusCodes.Status200OK, new { message = "OK", Response = "", success = true });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status302Found, new { message = ex.Message, Response = "" });
            }
        }

        [HttpPut]
        [Route("ActualizarPersonaFisica")]
        public IActionResult ActualizarPersonaFisica([FromBody] PersonasFisica personaF)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@IdPersonaFisica", personaF.IdPersonaFisica),
                    new SqlParameter("@Nombre", personaF.Nombre),
                    new SqlParameter("@ApellidoPaterno", personaF.ApellidoPaterno),
                    new SqlParameter("@ApellidoMaterno", personaF.ApellidoMaterno),
                    new SqlParameter("@RFC", personaF.Rfc),
                    new SqlParameter("@FechaNacimiento", personaF.FechaNacimiento),
                    new SqlParameter("@UsuarioAgrega", personaF.UsuarioAgrega)
                };

                _dbContext.Database.ExecuteSqlRaw("EXEC sp_ActualizarPersonaFisica @IdPersonaFisica, @Nombre, @ApellidoPaterno, @ApellidoMaterno, @RFC, @FechaNacimiento, @UsuarioAgrega", parameters);

                return StatusCode(StatusCodes.Status200OK, new { message = "OK", Response = "" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message, Response = "" });
            }
        }

        [HttpDelete]
        [Route("EliminarPersonaFisica/{idPersonaFisica:int}")]
        public IActionResult EliminarPersonaFisica(int idPersonaFisica)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@IdPersonaFisica", idPersonaFisica)
                };

                
                _dbContext.Database.ExecuteSqlRaw("EXEC sp_EliminarPersonaFisica @IdPersonaFisica", parameters);

                return StatusCode(StatusCodes.Status200OK, new { message = "OK", Response = "Borrao" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message, Response = "" });
            }
        }

    }


}
