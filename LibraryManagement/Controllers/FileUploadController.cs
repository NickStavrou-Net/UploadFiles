using LibraryData;
using LibraryData.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Controllers
{
	public class FileUploadController : Controller
	{
		//access the physical path of wwwroot folder while saving image 
		private readonly IWebHostEnvironment _env;
		private readonly ILogger<FileUploadController> _logger;
		private readonly LibraryContext _context;

		public FileUploadController(ILogger<FileUploadController> logger,
			IWebHostEnvironment env,
			LibraryContext context)
		{
			_logger = logger;
			_env = env ?? throw new ArgumentNullException(nameof(env));
			_context = context;
		}

		[HttpGet]
		public IActionResult UploadFile()
		{
			return View();
		}


		[HttpPost("Upload")]
		public async Task<IActionResult> UploadFile(Book book, IFormFileCollection images)
		{
			if (images is null || images.Count() == 0)
				return Content("file not selected");

			//Add Guid
			var addGuid = Convert.ToString(Guid.NewGuid());

			var filepaths = new List<string>(); 
			foreach (var formfiles in images)
			{
				if (formfiles.Length > 0)
				{
					//save it with Guid + random name
					string path = $"{_env.WebRootPath}/images/{string.Concat(addGuid, Path.GetRandomFileName())}.png";

					filepaths.Add(path);

					/*	The recommended way of saving the file is to save outside of the application folders. 
						Because of security issues, if we save the files in the outside directory we can scan those folders
						in background checks without affecting the application. 

						string path = $"{_config["AppSettings:FileRootPath"]}/images/{Path.Combine(addGuid, fileUpload.FormFile.FileName)}";
					*/

					using (var stream = new FileStream(path, FileMode.Create))
					{
						stream.Position = 0;
						await formfiles.CopyToAsync(stream);
						stream.Flush();
					}
					
				}
		
			}

			var newbook = new Book()
			{
				Title = book.Title,
				Description = book.Description,
				ImageFilePaths = filepaths
			};

			_context.Books.Add(newbook);
			await _context.SaveChangesAsync();


			return RedirectToAction(nameof(Index), "Home");
		}
	}
}
