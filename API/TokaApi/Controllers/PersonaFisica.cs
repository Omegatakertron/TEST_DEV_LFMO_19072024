using Microsoft.AspNetCore.Mvc;
using TokaApi.Models;
using System.Data;

namespace TokaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonaFisica : ControllerBase
{
    SQLHandler db = new Util.SQLHandler();

    public PersonaFisica(){
        db.SetConnectionStringFromAppSettingsFile("Default");
    }
   

    [HttpPost]
    [Route("RegistrarPersonaFisica")]
    public IActionResult GetMacroTable([FromBody] PersonaFisicaModel requestModel)
    {
        var grm = new GenericResponseModel();

        try
        {
            grm.Data = db.GetDataTable("dbo.sp_AgregarPersonaFisica",requestModel.Nombre, requestModel.ApellidoPaterno, requestModel.);
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
}
