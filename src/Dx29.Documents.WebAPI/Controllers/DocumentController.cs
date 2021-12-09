using Dx29.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dx29.Documents.WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DocumentController : Controller
    {
        const int SIZE_LIMIT = 24 * 1024 * 1024; // 24 MB

        public DocumentController(DocumentService documentService)
        {
            DocumentService = documentService;
        }

        public DocumentService DocumentService { get; }

        [RequestSizeLimit(SIZE_LIMIT)]
        [ResponseCache(Duration = 30)]
        [HttpGet("{document_type}/{document_name}/{language}")]
        public async Task<IActionResult> Download(string document_type, string document_name, string language, [FromQuery] string version=null) //names sin _
        {
            if (document_name.Length > 128) return BadRequest("Name too large.");

            try
            {
                var stream = await DocumentService.DownloadFileAsync(document_type, document_name, language, version);
                if (stream != null)
                {
                    return File(stream, "application/octet-stream");
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("index")]
        public async Task<IActionResult> DownloadIndex()
        {
           
            try
            {
                var stream = await DocumentService.DownloadFileIndexAsync();
                if (stream != null)
                {
                    return File(stream, "application/octet-stream"); // application/json
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
