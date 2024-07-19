using Microsoft.AspNetCore.Mvc;
using TokaApi.Models;

namespace TokaApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PersonaFisica : ControllerBase
{
    SQLHandler db = new Util.SQLHandler();

    public PersonaFisica(){
        db.SetConnectionStringFromAppSettingsFile("Default");
    }
   

    [HttpPost]
    [Route("InsertPersonaFisica")]
    public IActionResult GetMacroTable([FromBody] CaducosModel requestModel)
    {
        var grm = new GenericResponseModel();

        try
        {
            grm.Data = db.GetDataTable("dbo.up_getMacroTable",requestModel.PK);
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
