using Microsoft.AspNetCore.Mvc;
using TokaApi.Models;
using System.Data;

namespace TokaApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PersonaFisica : ControllerBase
{
    
    Util.SQLHandler db = new Util.SQLHandler();

    public PersonaFisica(){
        db.SetConnectionStringFromAppSettingsFile("Default1");
    }
   

    [HttpPost]
    [Route("RegistrarPersonaFisica")]
    public IActionResult RegistrarPersonaFisica([FromBody] PersonaFisicaModel requestModel)
    {
        var grm = new GenericResponseModel();

        try
        {
            grm.Data = db.GetDataTable("dbo.sp_AgregarPersonaFisica",requestModel.Nombre, requestModel.ApellidoPaterno, requestModel.ApellidoMaterno, requestModel.rfc, requestModel.FechaNacimiento, requestModel.UsuarioAgrega);
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

    [HttpGet]
    [Route("ObtenerPersonasFisicas")]
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

    [HttpPut]
    [Route("ActualizarPersonaFisica")]
    public IActionResult ActualizarPersonaFisica([FromBody] PersonaFisicaModel requestModel)
    {
        var grm = new GenericResponseModel();

        try
        {
            grm.Data = db.GetDataTable("dbo.sp_ActualizarPersonaFisica", requestModel.idPersonaFisica, requestModel.Nombre, requestModel.ApellidoPaterno, requestModel.ApellidoMaterno, requestModel.rfc, requestModel.FechaNacimiento, requestModel.UsuarioAgrega);
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

    [HttpDelete]
    [Route("EliminarPersonaFisica")]
    public IActionResult EliminarPersonaFisica([FromBody] PersonaFisicaModel requestModel)
    {
        var grm = new GenericResponseModel();

        try
        {
            grm.Data = db.GetDataTable("dbo.sp_EliminarPersonaFisica", requestModel.idPersonaFisica);
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
}
