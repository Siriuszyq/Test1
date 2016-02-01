using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace ConsoleApplication4Queue
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

            Console.WriteLine("Server is running...");

            while (true)
            {
                /* 从队列中读取一条信息
                 收到信息后可以根据收到的信息做处理，为了演示方便我们这里只是把信息显示出来
                 在云端发送消息后这条消息将对于后续的请求不可见，但是并未被删除。我们需要显示删除它。
                 否则在一段时间后该消息将重新可见。这一设计的好处是确保了所有消息都能够被处理。
                 如果程序在收到消息后处理消息前就异常终止了那么数据依然在一段时间后可以被重新处理。
                 详情请参考MSDN文档*/
                var message = queue.GetMessage();

                if (message != null)
                {
                    Console.WriteLine(string.Format("Message retrieved:{0}", message.AsString));

                    //处理完数据后必须显示删除消息
                    queue.DeleteMessage(message);
                }

                //每次读取数据后线程休息3秒
                Thread.Sleep(3000);
            }
        }
    }
}
