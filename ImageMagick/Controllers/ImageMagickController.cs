using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ImageMagick.Controllers
{
    [ApiController]
    public class ImageMagickController : ControllerBase
    {
        private readonly IHostingEnvironment _env;
        public ImageMagickController(IHostingEnvironment env)
        {
            _env = env;
        }



        [HttpPost]
        [Route("api/ImageMagick/SaveImageConverter")]
        public async Task<IActionResult> SaveImageConverterAsync(IFormFile image)
        {
            byte[] bytes;

            using (var ms = new MemoryStream())
            {
                await image.CopyToAsync(ms);
                bytes = ms.ToArray();
            }

            using (var imagebytes = new MagickImage(data: bytes))
            {
                imagebytes.ColorFuzz = new Percentage(10);
                //image1.Transparent(MagickColors.White);

                //to adjust Image Quality
                imagebytes.Quality = 25;

                imagebytes.Resize(1200, 1000);

                var fileName = $"{Guid.NewGuid().ToString()}.Jpg";
                var directory = Path.Combine(_env.WebRootPath, "Files", "Images");
                if (!Directory.Exists(directory)) { Directory.CreateDirectory(directory); }

                var path = Path.Combine(_env.WebRootPath, "Files", "Images", fileName);

                imagebytes.Write(path);

                return Ok($"Files/{"Images"}/{fileName}");

            }
        }



    }
}
