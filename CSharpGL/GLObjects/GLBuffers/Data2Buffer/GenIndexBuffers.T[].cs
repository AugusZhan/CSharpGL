﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CSharpGL
{
    public static partial class Data2Buffer
    {
        /// <summary>
        /// 生成若干用于存储索引的IBO。索引指定了<see cref="VertexBuffer"/>里各个顶点的渲染顺序。
        /// Generates some Index Buffer Objects storing vertexes' indexes, which indicate the rendering order of each vertex.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="type"></param>
        /// <param name="usage"></param>
        /// <param name="blockSize">How many elements per index buffer?(sometimes except the last one)</param>
        /// <returns></returns>
        public static IndexBuffer[] GenIndexBuffers<T>(this T[] array, IndexBufferElementType type, BufferUsage usage, int blockSize) where T : struct
        {
            if (array == null) { throw new ArgumentNullException("array"); }
            if (blockSize <= 0) { throw new ArgumentException("blockSize must be greater than 0."); }

            GCHandle pinned = GCHandle.Alloc(array, GCHandleType.Pinned);
            IntPtr header = pinned.AddrOfPinnedObject();
            // same result with: IntPtr header = Marshal.UnsafeAddrOfPinnedArrayElement(array, 0);
            var list = new List<IndexBuffer>();
            int current = 0;
            int totalLength = array.Length;
            do
            {
                if (current + blockSize <= totalLength)
                {
                    UnmanagedArrayBase unmanagedArray = new TempUnmanagedArray<T>((IntPtr)(header.ToInt32() + current), blockSize);// It's not necessary to call Dispose() for this unmanaged array.
                    IndexBuffer buffer = GenIndexBuffer(unmanagedArray, type, usage);
                    list.Add(buffer);
                    current += blockSize;
                }
                else
                {
                    int length = totalLength - current;
                    UnmanagedArrayBase unmanagedArray = new TempUnmanagedArray<T>((IntPtr)(header.ToInt32() + current), length);// It's not necessary to call Dispose() for this unmanaged array.
                    IndexBuffer buffer = GenIndexBuffer(unmanagedArray, type, usage);
                    list.Add(buffer);
                    current += length;
                }
            } while (current < totalLength);
            pinned.Free();

            return list.ToArray();
        }

    }
}