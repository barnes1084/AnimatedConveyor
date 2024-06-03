using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Security.Cryptography.X509Certificates;

namespace CommonTools
{
    public class LocalMemoryMap
    {


        MemoryMappedFile mmf;
        long capacity = 1000;
        List<bool> tagPositionUsed;



        public LocalMemoryMap(String mapname = "map", long capacity = 1000)
        {
            Directory.CreateDirectory($@"{Directory.GetCurrentDirectory()}\Tagdata");
            string path = $@"{Directory.GetCurrentDirectory()}\Tagdata\liveData.dat";
            mmf = MemoryMappedFile.CreateFromFile(path, FileMode.OpenOrCreate, mapname, capacity);
            tagPositionUsed = new List<bool>(1000);
        }



        public int WriteMemFile(int tagvalue)
        {
            try
            {
                int position = 0;
                using (var accessor = mmf.CreateViewAccessor())
                {
                    for (position = 0; position < capacity; position += 4)
                    {
                        //if (tagPositionUsed.TrueForAll(x => !x))
                        //{
                        //    tagPositionUsed.Add(false);

                        //}
                        if (tagPositionUsed[position] == false)
                        {
                            accessor.Write(position, tagvalue);
                            tagPositionUsed[position] = true;
                            break;
                        }
                    }
                }
                return position;
            }
            catch (Exception)
            {
                return -1;
            }
        }



        public int ReadMemFile(int tagposition)
        {
            try
            {
                using (var accessor = mmf.CreateViewAccessor())
                {
                    return accessor.ReadInt32(tagposition);
                }
            }
            catch (Exception)
            {
                return -1;
            }
        }


    } 
}














        //private static void MemShareExample()
        //{
        //    MemoryMappedFile mmf = MemoryMappedFile.CreateOrOpen("map1", 1000);

        //    using (MemoryMappedViewAccessor accessor = mmf.CreateViewAccessor())
        //    {
        //        Console.WriteLine($"Write {decimal.MaxValue} to memshare starting at position 0.");
        //        accessor.Write(0, (decimal)decimal.MaxValue);
        //        Console.ReadLine();

        //        int bit = 0;
        //        for (int i = 0; i < 999; i++)
        //        {
        //            if (accessor.ReadByte(i) == 0)
        //            {
        //                break;
        //            }
        //            else
        //            {
        //                Console.WriteLine($"Read from memshare starting at position {i}:  {accessor.ReadByte(i)}");
        //                bit++;
        //            }

        //        }
        //        Console.WriteLine($"{bit}-bit");

        //        Console.ReadLine();
        //        Console.WriteLine($"Read decimal from memshare starting at position 0:  {accessor.ReadDecimal(0)}");
        //        Console.ReadLine();

        //        //byte[] data = Encoding.UTF8.GetBytes("test data");
        //        //Console.WriteLine($"Convert string \'test data\' to byte. This is {data.Length} bytes");
        //        //Console.ReadLine();

        //        //accessor.WriteArray(1, data, 0, data.Length);
        //        //Console.WriteLine($"Write string to data1.dat file starting at position 1.");
        //        //Console.ReadLine();

        //        //byte[] data1 = Encoding.UTF8.GetBytes("blob data");
        //        //accessor.ReadArray(1, data1, 0, data.Length);

        //        //Console.WriteLine($"Read from data.dat file starting at position 1:  {System.Text.Encoding.UTF8.GetString(data1)}");
        //        //Console.ReadLine();
        //    }

        //}

