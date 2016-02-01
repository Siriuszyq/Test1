using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            var storageAccount = CloudStorageAccount.DevelopmentStorageAccount;
            var blobclient = storageAccount.CreateCloudBlobClient();

            // 检查container是否被创建，如果没有，创建container
            var container = blobclient.GetContainerReference("stupidcontainer");
            container.CreateIfNotExists();

            // 新建一个blob，内容为"Hello World"
            var blob = container.GetBlockBlobReference("myfile");
            /*using (var fileStream = System.IO.File.OpenRead("myfile"))
            {
                blob.UploadFromStream(fileStream);
            }*/
            blob.UploadText("stupid");
      
            // 读取并显示blob内容
            var blobcontent = blob.DownloadText();
            Console.WriteLine(blobcontent);
           
            // 删除blob
            var succeed = blob.DeleteIfExists();
            Console.WriteLine(succeed ? "Delete Succeed" : "Delete Failed");
            Console.ReadLine();
        }
    }
}
