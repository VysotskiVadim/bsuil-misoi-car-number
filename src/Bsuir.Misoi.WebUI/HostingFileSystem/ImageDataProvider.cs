using System;
using System.IO;
using System.Threading.Tasks;
using Bsuir.Misoi.Core.Storage;
using Microsoft.Dnx.Runtime;
using System.Collections.Generic;

namespace Bsuir.Misoi.WebUI.HostingFileSystem
{
	public class ImageDataProvider : IImageDataProvider
	{
		private const string ImagesFolderName = "images";

		private readonly IApplicationEnvironment _hostingEnvironment;

		public ImageDataProvider(IApplicationEnvironment hostingEnvironment)
		{
			_hostingEnvironment = hostingEnvironment;
        }

		public Task<IEnumerable<string>> GetAllImagesNames()
		{
			var result = Directory.EnumerateFiles(this.GetPathForFile());
            return Task.FromResult(result);
		}

		public Stream GetImage(string name)
		{
			return File.OpenRead(this.GetPathForFile(name));
		}

		public Stream GetStreamForSaving(string name)
		{
			return File.OpenWrite(this.GetPathForFile(name));
		}

		private string GetPathForFile(string name = "")
		{
			return Path.Combine(_hostingEnvironment.ApplicationBasePath, ImagesFolderName, name);
		}
	}
}
