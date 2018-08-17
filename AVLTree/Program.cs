using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using TimeTest;

namespace AVLTree
{
    class Program
    {
        const int MaxCount = 10000;

        static void RealMain()
        {
            int[] baseArray = InitialArray();
            var tree = new AVLTree();
            var stream = new MemoryStream();
            var formatter = new BinaryFormatter();
            Action<int>[] delAVL = new Action<int>[] { tree.Insert, tree.Remove };
            Console.WriteLine("AVL Tree test. Fill, Search, Remove and Serialize");
            using (new OperationTimer("AVLTreeTest. Fill, Search, Remove and Serialize time test."))
            {
                for (int oper = 0; oper < 2; oper++)
                {
                    for (int i = 0; i < MaxCount; i++)
                    {
                        delAVL[oper](baseArray[i]);
                    }
                    if (oper == 0)
                    {
                        formatter.Serialize(stream, tree);
                    }
                    Console.WriteLine("Tree height: " + tree.GetHeight().ToString());
                    Console.Write(oper == 0 ? $"item = {baseArray[500]}, Search item result = {tree.Search(baseArray[500])}" : "removing has completed");
                    Console.WriteLine();
                }
            }
            stream.Position = 0;
            tree = (AVLTree)formatter.Deserialize(stream);
            stream.Dispose();
            Console.WriteLine("Deserializing the tree...\nDeserialized tree height: " + tree.GetHeight());
            Console.ReadKey();
        }

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            RealMain();
        }

        public static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            String resourceName = Assembly.GetExecutingAssembly().GetManifestResourceNames().FirstOrDefault(rn => 
                    rn.EndsWith(new AssemblyName(args.Name).Name + ".dll"));
            if (resourceName == null) return null;
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("AVLTree.OperationTimer.dll"))
            {
                byte[] asssemlyData = new byte[stream.Length];
                stream.Read(asssemlyData, 0, asssemlyData.Length);
                return Assembly.Load(asssemlyData);
            }
        }

        private static int[] InitialArray()
        {
            int[] array = new int[MaxCount];
            Random rand = new Random();
            for (int i = 0; i < MaxCount; i++)
            {
                array[i] = rand.Next();
            }
            return array;
        }
    }
}
