using System;
using System.IO;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Core.IO
{
    
    public enum BYTE_ORDER { WINDOWS, MAC };

    /// <summary>
    /// Raw file to help loading and saving of raw file format.
    /// Always converts the data to a float for convenience.
    /// </summary>
    public class RawFile
    {

		/// <summary>
		/// Load 32 bit data from file name.
		/// </summary>
        public RawFile(string fileName)
        {
            FileName = fileName;
            BitDepth = 32;
            ByteOrder = BYTE_ORDER.WINDOWS;

            FindSize();
        }

		/// <summary>
		/// Load data from file with the provided bitdepth and byte order.
		/// </summary>
        public RawFile(string fileName, int bitDepth, BYTE_ORDER byteOrder = BYTE_ORDER.WINDOWS)
        {
            FileName = fileName;
            BitDepth = bitDepth;
            ByteOrder = byteOrder;

            FindSize();
        }

        /// <summary>
        /// Load data from file with the provided bitdepth and byte order.
        /// </summary>
        public RawFile(string fileName, int bitDepth, bool bigEndian)
        {
            FileName = fileName;
            BitDepth = bitDepth;
            ByteOrder = bigEndian ? BYTE_ORDER.MAC : BYTE_ORDER.WINDOWS;

            FindSize();
        }

        /// <summary>
        /// Load data from file with the provided format.
        /// </summary>
        public RawFile(RawFileFormat format)
        {
            FileName = format.fileName;
            BitDepth = format.bitDepth;
            ByteOrder = format.byteOrder;

            FindSize();
        }

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
        /// The loaded data.
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[RawFile: BitDepth={0}, ByteOrder={1}, ByteCount={2}, ElementCount={3}]", 
                BitDepth, ByteOrder, ByteCount, ElementCount);
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

            if (index < 0 || index >= ByteCount)
                throw new IndexOutOfRangeException("Index out of bounds");

            if (Data == null) LoadData();

            return BitConverter.ToSingle(Data, index);
        }

        /// <summary>
        /// 
        /// </summary>
        public ushort ReadShort(int i)
        {
            int index = i * 2;

            if (index < 0 || index >= ByteCount)
                throw new IndexOutOfRangeException("Index out of bounds");

            if (Data == null) LoadData();

            if (BigEndian)
                return (ushort)(Data[index] * 256 + Data[index + 1]);
            else
                return (ushort)(Data[index] + Data[index + 1] * 256);

        }

        /// <summary>
        /// 
        /// </summary>
        public byte ReadByte(int i)
        {
            int index = i;

            if (index < 0 || index >= ByteCount)
                throw new IndexOutOfRangeException("Index out of bounds");

            if (Data == null) LoadData();

            return Data[index];
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
        public void LoadData()
        {
            Data = File.ReadAllBytes(FileName);
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
        public static float[] Load16Bit(string fileName, bool bigEndian)
        {
            return Load16Bit(fileName, bigEndian ? BYTE_ORDER.MAC : BYTE_ORDER.WINDOWS);
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
        public static void Save16Bit(string filename, float[] data, bool bigEndian)
        {
            Save16Bit(filename, data, bigEndian ? BYTE_ORDER.MAC : BYTE_ORDER.WINDOWS);
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























