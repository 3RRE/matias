using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

namespace CapaPresentacion.Utilitarios
{
    public class GoogleDriveApiHelperV2
    {
        static readonly string[] Scopes = { DriveService.Scope.Drive };
        static string ApplicationName = "cafeasp demo 01";
        public string UploadBase64FileInFolder(DriveService service, string base64string)
        {
            byte[] imageBytes = Convert.FromBase64String(base64string);

            List<string> listaParent = new List<string>();

            //agregar id de folder
            listaParent.Add("1X-DfMHQNjjhLognGib787LmUOk9sPhRn");
            //metadata
            var fileMetadata = new Google.Apis.Drive.v3.Data.File();
            fileMetadata.Name = "MyFile" + DateTime.Now.ToString();
            fileMetadata.MimeType = "image/jpeg";
            fileMetadata.Parents = listaParent;

            FilesResource.CreateMediaUpload request;

            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                Image image = Image.FromStream(ms, true);
                request = service.Files.Create(fileMetadata, ms, "image/jpeg");
                request.Fields = "id";
                request.Upload();
            }
            var file = request.ResponseBody;
            return file.Id;
            //Console.WriteLine("File ID: " + file.Id);
        }
        public string DownloadFile(DriveService service, string fileId)
        {
            string respuesta = "";

            var request = service.Files.Get(fileId);
            var stream = new System.IO.MemoryStream();

            // Add a handler which will be notified on progress changes.
            // It will notify on each chunk download and when the
            // download is completed or failed.
            request.MediaDownloader.ProgressChanged += (Google.Apis.Download.IDownloadProgress progress) =>
            {
                switch (progress.Status)
                {
                    case Google.Apis.Download.DownloadStatus.Downloading:
                        {
                            //Console.WriteLine(progress.BytesDownloaded);
                            break;
                        }
                    case Google.Apis.Download.DownloadStatus.Completed:
                        {
                            //Console.WriteLine("Download complete.");
                            byte[] imageBytes = stream.ToArray();

                            // Convert byte[] to base 64 string
                            string base64String = Convert.ToBase64String(imageBytes);
                            //SaveStream(stream, saveTo);
                            respuesta = base64String;
                            break;
                        }
                    case Google.Apis.Download.DownloadStatus.Failed:
                        {
                            respuesta = "";
                            //respuesta = "Download Failed";
                            //Console.WriteLine("Download failed.");
                            break;
                        }
                }
            };
            request.Download(stream);
            return respuesta;

        }
        public string CreateFolder(DriveService service, string folderName)
        {
            Google.Apis.Drive.v3.Data.File body = new Google.Apis.Drive.v3.Data.File();
            body.Name = folderName;
            body.MimeType = "application/vnd.google-apps.folder";

            // service is an authorized Drive API service instance
            Google.Apis.Drive.v3.Data.File file = service.Files.Create(body).Execute();

            return file.Id;
        }
        public string DeleteFile(DriveService service, string fileId)
        {
            string response = "";
            FilesResource.DeleteRequest request;
            request = service.Files.Delete(fileId);
            response = request.Execute();
            //response = await request.ExecuteAsync().ConfigureAwait(false);
            return response;
        }
        //public UserCredential GetCredentials()
        //{
        //    UserCredential credential;

        //    string direccionCredentials2Json = Server.MapPath("/") + Request.ApplicationPath + "/Content/token_secret.json";
        //    string direccionCredentialsJson = Server.MapPath("/") + Request.ApplicationPath + "/Content/client_secret.json";
        //    using (var str = new FileStream(direccionCredentialsJson, FileMode.Open, FileAccess.Read))
        //    {
        //        string credPath = direccionCredentials2Json;
        //        //string credPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        //        //credPath = Path.Combine(credPath, ".credentials/drive-dotnet-quickstart-credentials.json");
        //        credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
        //         GoogleClientSecrets.FromStream(str).Secrets,
        //         Scopes,
        //         "user",
        //         CancellationToken.None,
        //         new FileDataStore(credPath, true)).Result;
        //    }
        //    //using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
        //    //{
        //    //    string credPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

        //    //    credPath = Path.Combine(credPath, ".credentials/drive-dotnet-quickstart.json");

        //    //    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
        //    //        GoogleClientSecrets.FromStream(stream).Secrets,
        //    //        Scopes,
        //    //        "user",
        //    //        CancellationToken.None,
        //    //        new FileDataStore(credPath, true)).Result;
        //    //    // Console.WriteLine("Credential file saved to: " + credPath);
        //    //}

        //    return credential;
        //}
        public UserCredential GetCredentials()
        {
            //get Credentials from client_secret.json file 
            UserCredential credential;
            //Root Folder of project
            var mapPath = System.Web.Hosting.HostingEnvironment.MapPath("~/");

            string filename = Path.Combine(mapPath, "Content/client_secret.json");

            //string direccionCredentials2Json = Server.MapPath("/") + Request.ApplicationPath + "/Content/token_secret.json";
            //string direccionCredentialsJson = Server.MapPath("/") + Request.ApplicationPath + "/Content/client_secret.json";

            using (FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                string credentialPath = Path.Combine(mapPath, "Content/DriveServiceCredentials.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credentialPath, true)).Result;
            }
            //create Drive API service.
            return credential;
        }
    }
}