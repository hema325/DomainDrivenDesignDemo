﻿using Application.Common.Helpers;
using Application.Common.Interfaces.FilesStorage;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.FileStorage
{
    internal class LocalFileStorageService: IFileStorage
    {
        public async Task<string> SaveAsync(IFormFile file)
        {
            var fileName = string.Concat(Guid.NewGuid().ToString(), Path.GetExtension(file.FileName));
            var fileType = Capitalize(file.ContentType.Substring(0, file.ContentType.IndexOf('/')));

            var fileDirectoryRelativePath = $"WebApi\\Files\\{fileType}s";
            var fileDirectoryAbsolutePath = PathHelper.GetAbsolutePath($"WebApi\\Files\\{fileType}s");

            var fileRelativePath = $"{fileDirectoryRelativePath}\\{fileName}";
            var fileAbsolutePath = PathHelper.GetAbsolutePath(fileRelativePath);

            if (!Directory.Exists(fileDirectoryAbsolutePath))
                Directory.CreateDirectory(fileDirectoryAbsolutePath);

            using var fileStream = File.Create(fileAbsolutePath);
            await file.CopyToAsync(fileStream);

            return fileRelativePath;
        }

        public void Delete(string relativePath)
        {
            var absolutePath = PathHelper.GetAbsolutePath(relativePath);

            File.Delete(absolutePath);
        }

        private string Capitalize(string text)
        {
            var words = text.Split(' ');
            var capitalizedWords = words.Select(word => char.ToUpper(word[0]) + word.Substring(1));

            return string.Join(" ", capitalizedWords);
        }
    }
}
