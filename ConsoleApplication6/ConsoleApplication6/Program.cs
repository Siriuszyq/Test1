using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 去除数组中重复的值！
/// 已经排好序数组
/// 通过测试，随机生成10W个和100W个数去重结果比较，方法二和方法三结果变化不大，花的时间主要是插入数据到List；而方法一数据越大，在List里面比较所花费的时间越多!
/// 
/// 最快的方法是不开辟新的存储空间，直接在原数组上面进行比较移动！
/// 
/// TestMachine:
/// CPU:Intel i5-3470 3.2GHz*2, Memory:10G, OS:Win 10
/// 
/// 10万个数数组去重结果：
///RemoveDuplicates1:00:00:18.299
///RemoveDuplicates2:00:00:00.003
///RemoveDuplicates3:00:00:00.002
///Result1:63273, Result2:63273,Result3:63273
///
/// 100万个数数组去重结果：
///RemoveDuplicates1:00:30:29.794
///RemoveDuplicates2:00:00:00.025
///RemoveDuplicates3:00:00:00.020
///Result1:632382, Result2:632382,Result3:632382
///
/// 1000万个数数组去重测试方法2和3结果： 
///RemoveDuplicates2:00:00:00.2710042
///RemoveDuplicates3:00:00:00.2070155
///Result2:6320752,Result3:6320752
///
///  1亿个数数组去重测试方法2和3结果：
/// RemoveDuplicates2:00:00:02.9222428
/// RemoveDuplicates3:00:00:02.0411441
////Result2:63204844,Result3:63204844
/// </summary>
namespace RemoveDuplicatefromSortedArray
{
    class Program
    {
        static void Main(string[] args)
        {
            //int[] nums = { 1, 2, 2, 2, 3, 3, 3, 4, 4, 5 };
            int[] array = RandomNums(200000);
            //int[] result1 = RemoveDuplicates1(array);
            int[] result2 = RemoveDuplicates2(array);
            int result3 = RemoveDuplicates3(array);

            //Console.WriteLine("Result1:{0}, Result2:{1},Result3:{2}", result1.Count(), result2.Count(), result3);
            Console.WriteLine("Result2:{0},Result3:{1}", result2.Count(), result3);
            Console.ReadLine();
        }

        /// <summary>
        /// 容易理解，直接判断List里面是否包含重复的数字，不包含就直接插入到List里面！
        /// 使用场景：对于数组比较小的情况下使用，去重比较快！
        /// </summary>
        /// <param name="nums"></param>
        /// <returns></returns>
        public static int[] RemoveDuplicates1(int[] nums)
        {
            //return nums.GroupBy(p => p).Select(p => p.Key).ToArray();
            DateTime startTime = DateTime.Now;
            List<int> list = new List<int>();
            //Array.Sort(nums);

            foreach (int num in nums)
            {
                if (!list.Contains(num))
                    list.Add(num);
            }
            DateTime endTime = DateTime.Now;
            TimeSpan ElapsedTime = endTime - startTime;
            Console.WriteLine("RemoveDuplicates1:" + ElapsedTime);
            return list.ToArray();
        }

        /// <summary>
        /// 把数组中第一个元素插入List，然后进行相临的：i和ｉ－１两两比较，不相等就直接插入到List里面！
        /// 使用场景：对于大数据量，比如上万的数组，那么这样是非常快的
        /// </summary>
        /// <param name="nums"></param>
        /// <returns></returns>
        public static int[] RemoveDuplicates2(int[] nums)
        {
            DateTime startTime = DateTime.Now;
            List<int> list = new List<int>();
            //Array.Sort(nums);

            list.Add(nums[0]);

            for (int i = 1; i < nums.Length; i++)
            {
                if (nums[i] != nums[i - 1])
                {
                    list.Add(nums[i]);
                }
            }
            DateTime endTime = DateTime.Now;
            TimeSpan ElapsedTime = endTime - startTime;
            Console.WriteLine("RemoveDuplicates2:" + ElapsedTime);
            return list.ToArray();
        }

        /// <summary>
        /// 从第一位开始设置索引index,从第二位开始与第一位进行比较,如果不相等,那么就把值赋给++index，然后继续以index为索引进行比较,不相等就把值赋给++index
        /// 使用场景：在不改变原数组长度，并且不开辟新数组的情况下可以使用！需要返回index+1来获得具体有多少个不重复的数字，然后到原数组里面去取        
        /// </summary>
        /// <param name="nums"></param>
        /// <returns></returns>
        public static int RemoveDuplicates3(int[] nums)
        {
            DateTime startTime = DateTime.Now;
            int index = 0;
            for (int i = 1; i < nums.Length; i++)
            {
                if (nums[index] != nums[i])
                    nums[++index] = nums[i];
            }

            DateTime endTime = DateTime.Now;
            TimeSpan ElapsedTime = endTime - startTime;
            Console.WriteLine("RemoveDuplicates3:" + ElapsedTime);
            return index + 1;
        }

        public static int[] RandomNums(int num)
        {
            Random r = new Random();
            List<int> list = new List<int>();
            for (int i = 0; i < num; i++)
            {
                list.Add(r.Next(1, num));
            }

            list.Sort();
            return list.ToArray();
        }
    }
}