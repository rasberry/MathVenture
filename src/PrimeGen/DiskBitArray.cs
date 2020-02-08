using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Numerics;

namespace MathVenture.PrimeGen
{
	public class DiskBitArray : IDisposable
	{
		public DiskBitArray(string fileName, BigInteger max)
		{
			FileName = fileName;
			Capacity = (long)(max.Log2() / 8 + 1); //total bytes
			Init();
		}

		public DiskBitArray(string fileName, long size)
		{
			FileName = fileName;
			Capacity = size;
			Init();
		}

		public bool this[long index] {
			get {
				if (index < 0 || index >= Length) {
					throw new IndexOutOfRangeException();
				}

				byte b = mmva.ReadByte(index / 8);
				bool bit = 0 != (b & 1<<(int)(index % 8));
				return bit;
			}
			set {
				if (index < 0 || index >= Length) {
					throw new IndexOutOfRangeException();
				}
				byte b = mmva.ReadByte(index / 8);
				byte n = value
					? (byte)(b | 1<<(int)(index % 8))
					: (byte)(b & ~(1<<(int)(index % 8)))
				;
				mmva.Write(index / 8,n);
			}
		}

		public void ResetAllTo(bool value)
		{
			long len = Capacity;
			byte bval = value ? Byte.MaxValue : Byte.MinValue;
			for(long b=0; b<len; b++) {
				mmva.Write(b,bval);
			}
		}

		public long Length { get { return Capacity * 8; }}

		string FileName;
		long Capacity;
		MemoryMappedViewAccessor mmva = null;
		MemoryMappedFile mmf = null;

		void Init()
		{
			var stream = File.Open(
				FileName,
				FileMode.Create,
				FileAccess.ReadWrite,
				FileShare.Read
			);
			stream.SetLength(Capacity);

			mmf = MemoryMappedFile.CreateFromFile(
				stream,
				null,
				Capacity,
				MemoryMappedFileAccess.ReadWrite,
				HandleInheritability.None,
				false
			);
			mmva = mmf.CreateViewAccessor();
		}

		public void Dispose()
		{
			Dispose(true);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				mmva.Dispose();
				mmf.Dispose();
			}
		}
	}
}
