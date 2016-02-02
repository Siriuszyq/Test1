using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace ConsoleApplication5
{
    class Program
    {
        static void Main(string[] args)
        {
            var storageAccount = CloudStorageAccount.DevelopmentStorageAccount;
            var tableStorage = storageAccount.CreateCloudTableClient();

            // 检查名为CustomerInfo的表格是否被创建，如果没有，创建它
            tableStorage.CreateTableIfNotExist("CustomerInfo");

            // 创建表格服务上下文
            var context = new CustomerInfoContext(storageAccount.TableEndpoint.AbsoluteUri, storageAccount.Credentials);

            // 插入两条客户信息数据,客户ID分别设置为0和1
            CustomerInfo ci1 = new CustomerInfo() { CustomerAge = 25, CustomerID = "0", CustomerName = "Mike" };
            context.AddObject("CustomerInfo", ci1);

            CustomerInfo ci2 = new CustomerInfo() { CustomerAge = 32, CustomerID = "1", CustomerName = "Peter" };
            context.AddObject("CustomerInfo", ci2);

            context.SaveChanges();


            // 查找CustomerID为1的客户数据并显示
            Console.WriteLine("Retrieve information of a customer whose ID is 1");

            var query = context.CreateQuery<CustomerInfo>("CustomerInfo").Where(c => c.CustomerID == "1").ToList();
            var returnedcustomerinfo = query.FirstOrDefault();

            Console.WriteLine(string.Format("Customer info retrieved: ID:{0},Name:{1},Age:{2}",
                returnedcustomerinfo.CustomerID, returnedcustomerinfo.CustomerName, returnedcustomerinfo.CustomerAge));


            // 更新CustomerID为1的客户数据中的年龄
            returnedcustomerinfo.CustomerAge = 33;
            context.UpdateObject(returnedcustomerinfo);
            Console.WriteLine("**Customer Info updated**");

            // 重新查询,测试更新效果

            Console.WriteLine("Retrieve information of a customer whose ID is 1");

            var query2 = context.CreateQuery<CustomerInfo>("CustomerInfo").Where(c => c.CustomerID == "1").ToList();
            var returnedcustomerinfo2 = query.FirstOrDefault();
            Console.WriteLine(string.Format("Customer info retrieved: ID:{0},Name:{1},Age:{2}",
                    returnedcustomerinfo2.CustomerID, returnedcustomerinfo2.CustomerName, returnedcustomerinfo2.CustomerAge));


            // 删除插入的两条客户数据
            context.DeleteObject(ci1);
            context.DeleteObject(ci2);
            context.SaveChanges();

            Console.WriteLine("The records has been deleted");
            Console.ReadLine();
        }

    }
    public class CustomerInfo : TableServiceEntity
    {
        public string CustomerID
        {
            get { return this.RowKey; }
            set { this.RowKey = value; }
        }
        public string CustomerName { get; set; }
        public int CustomerAge { get; set; }

        public CustomerInfo()
        {
            this.PartitionKey = "mypartitionkey";
        }
    }

    public class CustomerInfoContext : TableServiceContext
    {
        public CustomerInfoContext(string baseAddress, StorageCredentials credentials) :
            base(baseAddress, credentials)
        {
        }
    }
}