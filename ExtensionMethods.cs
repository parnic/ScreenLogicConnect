﻿using ScreenLogicConnect.Messages;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScreenLogicConnect
{
    static class ExtensionMethods
    {
        public static void SendHLMessage(this NetworkStream stream, Messages.HLMessage msg)
        {
            var arr = msg.AsByteArray();
            System.Diagnostics.Debug.WriteLine($"  sent {arr.Length}");
            stream.Write(arr);
        }

        public static void WritePrefixLength(this BinaryWriter bw, string val)
        {
            bw.Write(val.Length);
            bw.Write(Encoding.ASCII.GetBytes(val));
            var boundaryBufferLen = HLMessageTypeHelper.AlignToNext4Boundary(val.Length);
            for (int i = 0; i < boundaryBufferLen; i++)
            {
                bw.Write((byte)0);
            }
        }

        public static void WritePrefixLength(this BinaryWriter bw, ReadOnlySpan<byte> val)
        {
            bw.Write(val.Length);
            bw.Write(val);
        }

        public static void Write(this BinaryWriter bw, HLTime hlTime)
        {
            bw.Write(hlTime.year);
            bw.Write(hlTime.month);
            bw.Write(hlTime.dayOfWeek);
            bw.Write(hlTime.day);
            bw.Write(hlTime.hour);
            bw.Write(hlTime.minute);
            bw.Write(hlTime.second);
            bw.Write(hlTime.millisecond);
        }

        public static async Task<TResult> TimeoutAfter<TResult>(this Task<TResult> task, TimeSpan timeout)
        {

            using (var timeoutCancellationTokenSource = new CancellationTokenSource())
            {

                var completedTask = await Task.WhenAny(task, Task.Delay(timeout, timeoutCancellationTokenSource.Token));
                if (completedTask == task)
                {
                    timeoutCancellationTokenSource.Cancel();
                    return await task;
                }
                else
                {
                    return default;
                }
            }
        }
    }
}
