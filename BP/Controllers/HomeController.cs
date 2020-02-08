using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using JesseHolwell.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Text.Json;

namespace JesseHolwell.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHostingEnvironment _hostingEnvironment;

        public HomeController(ILogger<HomeController> logger, IHostingEnvironment hostingEnvironment)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Admin()
        {
            var filePath = _hostingEnvironment.WebRootPath + "/content/content.json";

            var model = new AdminViewModel();
            //string jsonString = string.Empty;
            //try
            //{
            //    using (StreamReader sr = new StreamReader(filePath))
            //    {
            //        // Read the stream to a string, and write the string to the console.
            //        String line = sr.ReadToEnd();
            //        jsonString = line;
            //    }

            //    var jsonModel = JsonSerializer.Deserialize<Content>(jsonString);


            //    model.text = jsonModel.text;
            //    model.texttest = jsonModel.texttest;
            //}
            //catch (FileNotFoundException ex)
            //{
            //    model.text = "no content found";
            //}
            //catch (Exception ex)
            //{
            //    model.text = ex.ToString();
            //}


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UploadSmallFile(IFormFile file)
        {
            // full path to file in temp location
            //var filePath = Path.GetTempFileName();
            var filePath = _hostingEnvironment.WebRootPath + "/img/test.png";

            if (file.Length > 0)
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }

            // process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return RedirectToAction("Admin");
            //return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UploadContent(string text)
        {
            Content model = new Content();
            model.text = text;
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            //var jsonString = File.ReadAllText("my-model.json");
            //var jsonModel = JsonSerializer.Deserialize<MyModel>(jsonString, options);
            var modelJson = JsonSerializer.Serialize(model, options);

            var filePath = _hostingEnvironment.WebRootPath + "/content/content.json";

            //using (StreamWriter outputFile = new StreamWriter(filePath))
            //{
            //    outputFile.WriteLine(modelJson);
            //}

            // Write the text to a new file named "WriteFile.txt".
            System.IO.File.WriteAllText(filePath, modelJson);

            // Create a string array with the additional lines of text
            //string[] lines = { "New line 1", "New line 2" };

            // Append new lines of text to the file
            //System.IO.File.AppendAllLines(Path.Combine(docPath, "WriteFile.txt"), lines);

            return RedirectToAction("Admin");
            //return Ok();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    class Content
    {
        public string text { get; set; }

        public string[] texttest { get; set; }
    }
}
