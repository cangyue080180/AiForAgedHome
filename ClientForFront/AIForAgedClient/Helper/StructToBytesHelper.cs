using System;
using System.Runtime.InteropServices;

namespace AIForAgedClient.Helper
{
    public class StructToBytesHelper
    {
        //结构体转数组
        public static byte[] StructToBytes<T>(T instance) where T : struct
        {
            int size = Marshal.SizeOf(instance);
            byte[] temp = new byte[size];

            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(instance, ptr, true);
            Marshal.Copy(ptr, temp, 0, size);
            Marshal.FreeHGlobal(ptr);
            return temp;
        }

        //数组转结构体
        public static T BytesToStruct<T>(byte[] data) where T : struct
        {
            T t = new T();
            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(t));
            Marshal.Copy(data, 0, ptr, data.Length);
            t = (T)Marshal.PtrToStructure(ptr, t.GetType());
            Marshal.FreeHGlobal(ptr);
            return t;
        }
    }
}