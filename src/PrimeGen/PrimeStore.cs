using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace MathVenture.PrimeGen
{
	class PrimeStore : IDisposable
	{
		static PrimeStore _inst = null;
		public static PrimeStore Self {
			get {
				if (_inst != null) { return _inst; }
				_inst = new PrimeStore(Options.PrimeStoreFile);
				return _inst;
			}
		}

		private PrimeStore(string filename)
		{
			Init(filename);
		}

		~PrimeStore()
		{
			Dispose();
		}

		SQLiteConnection sqConnection = null;

		void Init(string filename)
		{
			if (!File.Exists(filename)) {
				SQLiteConnection.CreateFile(filename);
			}
			sqConnection = new SQLiteConnection("Data Source="+filename+";Version=3;Synchronous=OFF;Journal Mode=OFF");
			sqConnection.Open();

			string sql = "CREATE TABLE IF NOT EXISTS primes ("
				+ "i INTEGER PRIMARY KEY ASC," //sqllite integer type is 64 bits
				+ "v BLOB"
				+");"
			;

			using (SQLiteCommand command = new SQLiteCommand(sql, sqConnection))
			{
				command.ExecuteNonQuery();
			}

			if (Count < 8) {
				/*0*/ Add(new BigInteger(2));
				/*1*/ Add(new BigInteger(3));
				/*2*/ Add(new BigInteger(5));
				/*3*/ Add(new BigInteger(7));
				/*4*/ Add(new BigInteger(11));
				/*5*/ Add(new BigInteger(13));
				/*6*/ Add(new BigInteger(17));
				/*7*/ Add(new BigInteger(19));
			};
		}

		static BigInteger BytesToBigInt(byte[] encoded)
		{
			object proto;
			BinaryFormatter bf = new BinaryFormatter();
			using (var ms = new MemoryStream(encoded)) {
				proto = bf.Deserialize(ms);
			}
			return (BigInteger)proto;
		}

		static byte[] BigIntToBytes(BigInteger bi)
		{
			BinaryFormatter bf = new BinaryFormatter();
			using (var ms = new MemoryStream()) {
				bf.Serialize(ms,bi);
				ms.Seek(0,SeekOrigin.Begin);
				return ms.ToArray();
			}
		}

		public BigInteger this[long index]
		{
			get {
				string sql = "SELECT v FROM primes WHERE i = "+(index+1)+";";
				using (SQLiteCommand command = new SQLiteCommand(sql, sqConnection))
				using (var reader = command.ExecuteReader())
				{
					while(reader.Read()) {
						byte[] buffer = GetBytes(reader);
						return BytesToBigInt(buffer);
					}
				}
				throw new InvalidDataException();
			}
		}

		//https://stackoverflow.com/questions/625029/how-do-i-store-and-retrieve-a-blob-from-sqlite
		static byte[] GetBytes(SQLiteDataReader reader)
		{
			const int CHUNK_SIZE = 2 * 1024;
			byte[] buffer = new byte[CHUNK_SIZE];
			long bytesRead;
			long fieldOffset = 0;
			using (MemoryStream stream = new MemoryStream())
			{
				while ((bytesRead = reader.GetBytes(0, fieldOffset, buffer, 0, buffer.Length)) > 0)
				{
					stream.Write(buffer, 0, (int)bytesRead);
					fieldOffset += bytesRead;
				}
				return stream.ToArray();
			}
		}

		public long Count { get {
			string sql = "SELECT max(i) FROM primes";
			using (SQLiteCommand command = new SQLiteCommand(sql, sqConnection))
			{
				var reader = command.ExecuteReader(System.Data.CommandBehavior.SingleResult);
				if (reader.Read()) {
					if (!reader.IsDBNull(0)) {
						object o = reader.GetValue(0);
						long l = (long)o;
						return l;
					}
				}
				return 0;
			}
		} }

		public bool IsReadOnly { get { return false; } }

		public void Add(BigInteger item)
		{
			byte[] data = BigIntToBytes(item);

			string sql = "INSERT INTO primes (v) VALUES (@val)";
			using (SQLiteCommand command = new SQLiteCommand(sql, sqConnection))
			{
				var param = command.Parameters.Add("@val",System.Data.DbType.Binary,data.Length);
				param.Value = data;
				command.ExecuteNonQuery();
			}
		}

		public long IndexOf(BigInteger number, out long start)
		{
			start = 0;
			long end = Count;

			while(start <= end) {
				long index = start + (end - start >> 1);
				BigInteger check = this[index];
				int comp = BigInteger.Compare(check,number);
				if (comp == 0) {
					return index;
				} else if (comp < 0) {
					start = index + 1;
				} else {
					end = index - 1;
				}
			}
			return -1;
		}

		public void Dispose()
		{
			if (sqConnection != null) {
				sqConnection.Dispose();
				sqConnection = null;
			}
		}
	}
}
