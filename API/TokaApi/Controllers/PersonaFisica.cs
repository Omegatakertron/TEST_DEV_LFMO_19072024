using Microsoft.AspNetCore.Mvc;
using TokaApi.Models;
using System.Data;
using Util;
using System.Data.SqlClient;

namespace TokaApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PersonaFisica : ControllerBase
{
    
    private readonly SQLHandler db;

    public PersonaFisica(){
        var connectionString = AppSettingsHandler.GetConnectionString("Default1");
        db = new SQLHandler(connectionString);
    }
   

    [HttpPost("RegistrarPersonaFisica")]
    public IActionResult RegistrarPersonaFisica([FromBody] PersonaFisicaModel requestModel)
    {
        var grm = new GenericResponseModel();

        try
        {
              db.ExecStoredProcedure("dbo.sp_AgregarPersonaFisica", new List<SqlParameter>
                {
                    new SqlParameter("@Nombre", requestModel.Nombre),
                    new SqlParameter("@ApellidoPaterno", requestModel.ApellidoPaterno),
                    new SqlParameter("@ApellidoMaterno", requestModel.ApellidoMaterno),
                    new SqlParameter("@RFC", requestModel.rfc),
                    new SqlParameter("@FechaNacimiento", requestModel.FechaNacimiento),
                    new SqlParameter("@UsuarioAgrega", requestModel.UsuarioAgrega)
                });
            grm.Success = true;
            grm.Message = "Persona Fisica registrada existosamente";
        }
        catch (Exception ex)
        {
            grm.Success = false;
            grm.Message = "Ocurrió un error al registrar la persona física.";
            return StatusCode(StatusCodes.Status500InternalServerError, grm);
        }

        return Ok(grm);
    }

    [HttpGet("ObtenerPersonasFisicas")]
    public IActionResult ObtenerPersonasFisicas()
    {
        var grm = new GenericResponseModel();

        try
        {
            grm.Data = db.GetDataTable("dbo.sp_ObtenerPersonasFisicas");
            grm.Success = true;
            grm.Message = string.Empty;
        }
        catch (Exception ex)
        {
            grm.Data = new DataTable();
            grm.Success = false;
            grm.Message = ex.Message;

            return BadRequest(grm);
        }


        return Ok(grm);
    }

    [HttpPut("ActualizarPersonaFisica/{id}")]
    public IActionResult ActualizarPersonaFisica(int id, [FromBody] PersonaFisicaModel requestModel)
    {
        var grm = new GenericResponseModel();

        try
        {
            db.ExecStoredProcedure("dbo.sp_ActualizarPersonaFisica", new List<SqlParameter>
            {
                new SqlParameter("@IdPersonaFisica", id),
                new SqlParameter("@Nombre", requestModel.Nombre),
                new SqlParameter("@ApellidoPaterno", requestModel.ApellidoPaterno),
                new SqlParameter("@ApellidoMaterno", requestModel.ApellidoMaterno),
                new SqlParameter("@RFC", requestModel.rfc),
                new SqlParameter("@FechaNacimiento", requestModel.FechaNacimiento),
                new SqlParameter("@UsuarioAgrega", requestModel.UsuarioAgrega)
            });
            grm.Success = true;
            grm.Message = "Persona física actualizada exitosamente.";
        }
        catch (Exception ex)
        {
            grm.Data = new DataTable();
            grm.Success = false;
            grm.Message = ex.Message;

            return BadRequest(grm);
        }


        return Ok(grm);
    }

    [HttpDelete("EliminarPersonaFisica{id}")]
    public IActionResult EliminarPersonaFisica(int id)
    {
        var grm = new GenericResponseModel();

        try
        {
            db.ExecStoredProcedure("dbo.sp_EliminarPersonaFisica", new List<SqlParameter>
                {
                    new SqlParameter("@IdPersonaFisica", id)
                });
            grm.Success = true;
            grm.Message = "Persona física eliminada exitosamente.";
        }
        catch (Exception ex)
        {
            grm.Data = new DataTable();
            grm.Success = false;
            grm.Message = ex.Message;

            return BadRequest(grm);
        }


        return Ok(grm);
    }
}
