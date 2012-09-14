using System.IO;
using System.Runtime.Serialization.Json;
using System.Threading;
using Weibo.DataModel.Misc;

namespace Weibo.Apis
{
    public static class FlowDocumentHelper
    {
        private static int _seed;
        public static void Save(string filepath,FlowDocumentInfo fdi)
        {
            
            var tfp = filepath + Interlocked.Increment(ref _seed);
            var ser = new DataContractJsonSerializer(typeof(FlowDocumentInfo));
            using(var stream = File.OpenWrite(tfp))
            {
                ser.WriteObject(stream, fdi);                
            }
            try
            {
                if (!File.Exists(filepath))
                    File.Move(tfp, filepath);
                else
                    File.Copy(tfp, filepath, true);
            }catch(IOException )
            {
                
            }
        }
        public static FlowDocumentInfo Load(string filepath)
        {
            if (!File.Exists(filepath))
                return null;
            var ser = new DataContractJsonSerializer(typeof(FlowDocumentInfo));
            using(var stream = File.OpenRead(filepath))
            {
                var fdi = ser.ReadObject(stream) as FlowDocumentInfo;
                return fdi;
            }
        }
    }
}