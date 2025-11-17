using Microsoft.AspNetCore.Mvc;
using System.Net;
using Training.Models;

namespace Training.Controllers
{
    public class BaseController: ControllerBase
    {
        public override OkObjectResult Ok(object? val) 
        {
            dynamic value = val;

            ServiceResponse response = new ServiceResponse
            {
                Data = value.Data,
                Message = value.Message,
                Success = value.Success,
                StatusCode = StatusCodes.Status200OK
            };

            return base.Ok(response);
        }

        public override BadRequestObjectResult BadRequest(object? val)
        {
            dynamic value = val;

            ServiceResponse response = new ServiceResponse
            {
                Data = value.Data,
                Message = value.Message,
                Success = value.Success,
                StatusCode = StatusCodes.Status400BadRequest
            };

            return base.BadRequest(response);
        }
    }
}
