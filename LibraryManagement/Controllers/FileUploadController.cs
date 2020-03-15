using LibraryData.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace LibraryManagement.Controllers
{
	public class FileUploadController : Controller
	{
		//access the physical path of wwwroot folder while saving image 
		private readonly IWebHostEnvironment _env;
		private readonly ILogger<FileUploadController> _logger;
		//private readonly IConfiguration _config;

		public FileUploadController(ILogger<FileUploadController> logger, IWebHostEnvironment env /*, IConfiguration config*/)
		{
			_logger = logger;
			_env = env;
			//_config = config;
		}

		[HttpPost]
		public async Task<IActionResult> UploadFile([FromForm]FileUpload files)
		{
			//Add Guid
			var addGuid = Convert.ToString(Guid.NewGuid());

			if (files.Images != null)
			{
				foreach (var formfile in files.Images)
				{
					//save it with Guid + random name
					string path = $"{_env.WebRootPath}/images/{string.Concat(addGuid, Path.GetRandomFileName())}.png";

					/*	The recommended way of saving the file is to save outside of the application folders. 
						Because of security issues, if we save the files in the outside directory we can scan those folders
						in background checks without affecting the application. 

						string path = $"{_config["AppSettings:FileRootPath"]}/images/{Path.Combine(addGuid, fileUpload.FormFile.FileName)}";
					*/

					using var stream = new FileStream(path, FileMode.Create);
					await formfile.CopyToAsync(stream);
				}
			}

			return RedirectToAction(nameof(Index), "Home");
		}
	}
}
