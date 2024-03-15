using Microsoft.AspNetCore.Mvc;

namespace Forum.Web.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class BaseController : Controller
{
	
}