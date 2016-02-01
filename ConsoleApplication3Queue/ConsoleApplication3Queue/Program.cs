using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace ConsoleApplication3Queue
{
    class Program
    {
        static void Main(string[] args)
        {
            var storageAccount = CloudStorageAccount.DevelopmentStorageAccount;
            var queueStorage = storageAccount.CreateCloudQueueClient();

            //检查名为stupidqueue的队列是否被创建，如果没有，创建它
            var queue = queueStorage.GetQueueReference("stupidqueue");
            queue.CreateIfNotExist();

            Console.WriteLine("Client is running...");

            while (true)
            {
                //插入数据到队列中
                queue.AddMessage(new CloudQueueMessage(string.Format("Client sent information:{0}", DateTime.UtcNow.ToString())));

                //每次插入数据后线程休息3秒
                Thread.Sleep(3000);
            }
        }
    }
}
