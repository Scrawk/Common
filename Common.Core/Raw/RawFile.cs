using System;
using System.IO;
using System.Collections.Generic;

using Common.Core.Mathematics;

namespace Common.Core.Raw
{
    
    public enum BYTE_ORDER { WINDOWS, MAC };

    /// <summary>
    /// Raw file to help loading and saving of raw file format.
    /// Always converts the data to a float for convenience.
    /// </summary>
    public class RawFile
    {

        private const int STRIP_SIZE = 1024 * 1024;

		/// <summary>
		/// The filename to load from and save to.
		/// </summary>
		public string FileName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool BigEndian { get { return ByteOrder == BYTE_ORDER.MAC; } }

        /// <summary>
        /// Gets the bit depth.
        /// </summary>
        public int BitDepth { get; private set; }

		/// <summary>
		/// Gets the byte order. Only needed for 16 bit files.
		/// </summary>
        public BYTE_ORDER ByteOrder { get; private set; }

		/// <summary>
		/// Gets the size of the file in bytes.
		/// </summary>
        public int ByteCount { get; private set; }

        /// <summary>
        /// Gets the number of elements, ie float == 4 bytes.
        /// </summary>
        public int ElementCount { get; private set; }

        /// <summary>
        /// Max contiguous size of the data.
        /// </summary>
        public int StripSize { get; private set; }

        /// <summary>
        /// The loaded byte data converted to a float.
        /// </summary>
        private byte[] Strip { get; set; }

        /// <summary>
        /// The current file offset were the strip was read from.
        /// </summary>
        private int CurrentOffset { get; set; }

		/// <summary>
		/// Load 32 bit data from file name.
		/// </summary>
        public RawFile(string fileName, int stripSize = 2048)
        {
            if (stripSize % 4 != 0)
                stripSize += stripSize % 4;

            FileName = fileName;
            BitDepth = 32;
            ByteOrder = BYTE_ORDER.WINDOWS;
            StripSize = stripSize;
            
            FindSize();
        }

		/// <summary>
		/// Load data from file with the provided bitdepth and byte order.
		/// </summary>
        public RawFile(string fileName, int bitDepth, BYTE_ORDER byteOrder = BYTE_ORDER.WINDOWS, int stripSize = STRIP_SIZE)
        {
            if (stripSize % 4 != 0)
                stripSize += stripSize % 4;

            FileName = fileName;
            BitDepth = bitDepth;
            ByteOrder = byteOrder;
            StripSize = stripSize;
         
            FindSize();
        }

        /// <summary>
        /// Load data from file with the provided format.
        /// </summary>
        public RawFile(RawFileFormat format, int stripSize = STRIP_SIZE)
        {
            if (stripSize % 4 != 0)
                stripSize += stripSize % 4;

            FileName = format.fileName;
            BitDepth = format.bitDepth;
            ByteOrder = format.byteOrder;
            StripSize = stripSize;
       
            FindSize();
        }

        public float Read(int i)
        {
            switch(BitDepth)
            {
                case 32:
                    return ReadFloat(i);

                case 16:
                    return ReadShort(i) / (float)ushort.MaxValue;

                case 8:
                    return ReadByte(i) / (float)byte.MaxValue;

                default:
                    throw new InvalidDataException("Unhandled bit depth");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public float ReadFloat(int i)
        {
            int index = i * 4;

            if (index >= ByteCount)
                throw new IndexOutOfRangeException("Index out of bounds");

            int offset = index / StripSize * StripSize;

            if (Strip == null || offset != CurrentOffset)
                LoadStrip(offset);

            return BitConverter.ToSingle(Strip, index % StripSize);
        }

        /// <summary>
        /// 
        /// </summary>
        public ushort ReadShort(int i)
        {
            int index = i * 2;

            if (index >= ByteCount)
                throw new IndexOutOfRangeException("Index out of bounds");

            int offset = index / StripSize * StripSize;

            if (Strip == null || offset != CurrentOffset)
                LoadStrip(offset);

            if(BigEndian)
                return (ushort)(Strip[index % StripSize] * 256 + Strip[index % StripSize + 1]);
            else
                return (ushort)(Strip[index % StripSize] + Strip[index % StripSize + 1] * 256);

        }

        /// <summary>
        /// 
        /// </summary>
        public byte ReadByte(int i)
        {
            int index = i;

            if (index >= ByteCount)
                throw new IndexOutOfRangeException("Index out of bounds");

            int offset = index / StripSize * StripSize;

            if (Strip == null || offset != CurrentOffset)
                LoadStrip(offset);

            return Strip[index % StripSize];
        }

        /// <summary>
        /// 
        /// </summary>
        private void FindSize()
        {
            FileInfo fi = new FileInfo(FileName);

            if (fi == null)
                throw new FileNotFoundException("File " + FileName + " not found");

            if (fi.Length > int.MaxValue)
                throw new InvalidOperationException("File size can not be greater than max int");

            ByteCount = (int)fi.Length;

            if (BitDepth == 32)
                ElementCount = ByteCount / 4;
            else if (BitDepth == 16)
                ElementCount = ByteCount / 2;
            else
                ElementCount = ByteCount;
        }

        /// <summary>
        /// Loads the bytes from file.
        /// </summary>
        private void LoadStrip(int offset)
        {
            if(Strip == null)
                Strip = new byte[StripSize];

            int count = Math.Min(ByteCount - offset, StripSize);

            using (Stream stream = new FileStream(FileName, FileMode.Open))
            {
                stream.Seek(offset, SeekOrigin.Begin);
                stream.Read(Strip, 0, count);
            }

            CurrentOffset = offset;
        }

		/// <summary>
		/// Loads 8 bit file and convert to float.
		/// </summary>
		public static float[] Load8Bit(string fileName)
		{
            byte[] bytes = File.ReadAllBytes(fileName);

			int size = bytes.Length;
			float[] data = new float[size];
			
			for (int x = 0; x < size; x++)
			{
				data[x] = (float)bytes[x] / 255.0f;
			}

			return data;
		}

		/// <summary>
		/// Loads 16 bit file and convert to float.
		/// </summary>
		public static float[] Load16Bit(string fileName, BYTE_ORDER byteOrder)
		{
            byte[] bytes = File.ReadAllBytes(fileName);
			
			int size = bytes.Length / 2;
			float[] data = new float[size];
			bool bigendian = byteOrder == BYTE_ORDER.MAC;
			
			for (int x = 0, i = 0; x < size; x++)
			{
				data[x] = (bigendian) ? (bytes[i++] * 256.0f + bytes[i++]) : (bytes[i++] + bytes[i++] * 256.0f);
				data[x] /= (float)ushort.MaxValue;
			}
			
			return data;
		}

		/// <summary>
		/// Loads 32 bit file and convert to float.
		/// </summary>
		public static float[] Load32Bit(string fileName)
		{
            byte[] bytes = File.ReadAllBytes(fileName);

			int size = bytes.Length / 4;
			float[] data = new float[size];
			
			for (int x = 0, i = 0; x < size; x++, i += 4)
			{
				data[x] = BitConverter.ToSingle(bytes, i);
			}
			
			return data;
		}

		/// <summary>
		/// Saves float data to 8 bit..
		/// </summary>
		public static void Save8Bit(string filename, float[] data)
		{
			int size = data.Length;

			byte[] bytes = new byte[size];

			for(int i = 0; i < size; i++)
				bytes[i] = (byte)(FMath.Clamp(data[i], 0.0f, 1.0f) * 255.0f);

			File.WriteAllBytes(filename, bytes);
		}

		/// <summary>
		/// Saves float data to 16 bit..
		/// </summary>
		public static void Save16Bit(string filename, float[] data, BYTE_ORDER byteOrder)
		{
			int size = data.Length * 2;

			byte[] bytes = new byte[size];
			
			ushort[] tmp = new ushort[data.Length];
			for(int i = 0; i < data.Length; i++)
				tmp[i] = (ushort)FMath.Clamp(data[i] * ushort.MaxValue, 0, ushort.MaxValue);

			Buffer.BlockCopy(tmp, 0, bytes, 0, size);

            if(byteOrder == BYTE_ORDER.MAC)
            {
                for (int i = 0; i < size / 2; i++)
                {
                    byte b0 = bytes[i * 2 + 0];
                    byte b1 = bytes[i * 2 + 1];

                    bytes[i * 2 + 0] = b1;
                    bytes[i * 2 + 1] = b0;
                }
            }

			File.WriteAllBytes(filename, bytes);
		}

		/// <summary>
		/// Saves float data to 32 bit..
		/// </summary>
		public static void Save32Bit(string filename, float[] data)
		{
			int size = data.Length * 4;

			byte[] bytes = new byte[size];
			
			Buffer.BlockCopy(data, 0, bytes, 0, size);
			File.WriteAllBytes(filename, bytes);
		}

    }


}























