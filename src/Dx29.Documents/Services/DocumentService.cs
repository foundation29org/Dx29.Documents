using Dx29.Documents.Data;
using Dx29.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dx29.Services
{
    public class DocumentService
    {
        public DocumentService(BlobStorage blobStorage)
        {
            BlobStorage = blobStorage;
        }

        public BlobStorage BlobStorage { get; }

        private const string CONTAINER = "documents";
        private const string INDEX_PATTERN = "index";

        public async Task<Stream> DownloadFileAsync(string documentType, string documentName, string language, string version)
        {
            // Get document name storaged
            string documentNameStored = await GetDocumentNameStoragedAsync(documentType, documentName, language, version);

            // Get & return the file
            if (documentNameStored != null)
            {
                string container = CONTAINER;
                string path = $"{documentType}/{documentNameStored.Replace("\"", "")}";
                return await BlobStorage.DownloadStreamAsync(container, path);
            }
            else
            {
                return null;
            }
        }

        public async Task<Stream> DownloadFileIndexAsync()
        {
            string container = CONTAINER;
            string latestIndexBlob_name = await LastVersionIndexFile();
            return await BlobStorage.DownloadStreamAsync(container, latestIndexBlob_name);
        }

        #region private functions
        private async Task<string> GetDocumentNameStoragedAsync(string documentType, string documentName, string language, string version)
        {
            string nameStoraged;
            if(version != null)
            {
                nameStoraged = language + "-" + version + "-" + documentName;
            }
            else
            {
                string indexBlobName = await LastVersionIndexFile();
                if (indexBlobName != "")
                {
                    string container = CONTAINER;
                    Stream indexBlob = await BlobStorage.DownloadStreamAsync(container, indexBlobName);
                    return GetDocumentStoragedName(indexBlob, documentType, documentName, language);
                }
                else
                {
                    return null;
                }
            }
            return nameStoraged;

        }

        private async Task<string> LastVersionIndexFile ()
        {
            string container = CONTAINER;
            string pattern = INDEX_PATTERN;
            var listBlobNames = await BlobStorage.ListBlobsAsync(container, pattern);
            return listBlobNames.OrderByDescending(r => r).FirstOrDefault();
        }

        private static string GetDocumentStoragedName(Stream blobStream, string documentType, string documentName, string language)
        {
            using (var sr = new StreamReader(blobStream))
            {
                string content = sr.ReadToEnd();
                var docIndex = content.Deserialize<IndexContent>();
                try
                {
                    var versions = docIndex[documentType][documentName][language];
                    var latest = versions.OrderByDescending(r => r.Key).FirstOrDefault();
                    return latest.Value;
                }
                catch
                {
                    return null;
                }
            }
        }
        #endregion
    }
}
