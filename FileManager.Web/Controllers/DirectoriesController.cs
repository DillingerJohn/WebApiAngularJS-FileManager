using FileManager.Core.Models;
using FileManager.Core.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;



namespace FileManager.Web.Controllers
{
    [Authorize]
    [RoutePrefix("api/Directories")]
    public class DirectoriesController : ApiController
    {
        private readonly IDirectoryService _driveService;

        public DirectoriesController(IDirectoryService driveService)
        {
            _driveService = driveService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetDirectories()
        {
            DirectoryClass directory = await _driveService._get();

            return Ok(directory);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ByPath/{path?}")]
        public async Task<IHttpActionResult> GetDirectoryByPath(string path = "")
        {
            DirectoryClass directory = await _driveService._get(path);
            if (directory == null)
            {
                return NotFound();
            }
            return Ok(directory);
        }
        [System.Web.Mvc.ValidateInput(false)]
        [AcceptVerbs]
        [AllowAnonymous]
        [HttpPost]
        [Route("openParrent")]
        public async Task<IHttpActionResult> openParrent(PathClass dir)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DirectoryClass Dir = await _driveService._get(dir.parrentPath);
            List<DirectoryClass> directory = new List<DirectoryClass> { };
            directory.Add(Dir);
            if (directory == null)
            {
                return null;
            }
            return Ok(directory);
        }
        [System.Web.Mvc.ValidateInput(false)]
        [AcceptVerbs]
        [AllowAnonymous]
        [HttpPost]
        [Route("openRoot")]
        public async Task<IHttpActionResult> openRoot(PathClass dir)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var getDit = await _driveService._get(dir.rootPath);
            List<DirectoryClass> directory = new List<DirectoryClass>();
            directory.Add(getDit);
            if (directory == null)
            {
                return null;
            }
            return Ok(directory);
            //  return CreatedAtRoute("ApiRoute", new { id = dir }, dir);
        }
        [System.Web.Mvc.ValidateInput(false)]
        [AcceptVerbs]
        [AllowAnonymous]
        [HttpPost]
        [Route("checkDir")]
        public async Task<IHttpActionResult> checkDir(PathClass dir)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var getDit = await _driveService._get(dir.path);
            List<DirectoryClass> directory = new List<DirectoryClass>();
            directory.Add(getDit);
            if (directory == null)
            {
                return null;
            }
            return Ok(directory);

        }
        [AllowAnonymous]
        [System.Web.Mvc.ValidateInput(false)]
        [AcceptVerbs]
        [HttpGet]
        [Route("GetDirectory/{path?}")]
        public async Task<IHttpActionResult> GetDirectory(string path = "")
        {
            // DirectoryClass directory = await _driveService._get(path);
            DirectoryClass directory = await _driveService._get(path);
            var resultdata = new List<DirectoryClass> {};
            resultdata.Add(directory);
               if (resultdata == null)
               {
                   return null;
               }
            return Ok(resultdata);
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            { }
            base.Dispose(disposing);
        }
    }
}
