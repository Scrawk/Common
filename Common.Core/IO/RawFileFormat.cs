using System;

namespace Common.Core.IO
{
    /// <summary>
    /// Raw file Settings.
    /// </summary>
    public struct RawFileFormat
    {

        public string fileName;

        public int bitDepth;

        public BYTE_ORDER byteOrder;


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[RawFileFormat: FileName={0}, ByteOrder={1}, ByteCount={2}]",
                fileName, bitDepth, byteOrder);
        }
    }

}