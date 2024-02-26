using Microsoft.AspNetCore.Mvc;

namespace ForumAPI.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class BaseController : Controller
{
	
}