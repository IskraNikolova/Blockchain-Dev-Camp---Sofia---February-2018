namespace BitcoinAddressGenerator
{
    using System;
    using System.Globalization;
    using System.Security.Cryptography;
    using System.Numerics;

    public class Program
    {
        public static void Main()
        {
            string hexHash = "0450863AD64A87AE8A2FE83C1AF1A8403CB53F53E486D8511DAD8A04887E5B23522CD470243453A299FA9E77237716103ABC11A1DF38855ED6F2EE187E9C582BA6";
            byte[] pubKey = HexToByte(hexHash);
            Console.WriteLine($"Public Key: {ByteToHex(pubKey)}");

            byte[] pubKeySha = Sha256(pubKey);
            Console.WriteLine($"Sha Public Key: {ByteToHex(pubKeySha)}");

            byte[] pubKeyShaRIPE = RipeMD160(pubKeySha);
            Console.WriteLine($"Ripe Sha Public Key: {ByteToHex(pubKeyShaRIPE)}");

            byte[] preHashWNetwork = AppendBitcoinNetwork(pubKeyShaRIPE, 0);
            byte[] publicHash = Sha256(preHashWNetwork);
            Console.WriteLine($"Public Hash: {ByteToHex(publicHash)}");

            byte[] publicHashHash = Sha256(publicHash);
            Console.WriteLine($"Public HashHash: {ByteToHex(publicHashHash)}");

            Console.WriteLine($"Checksum: {ByteToHex(publicHashHash).Substring(0, 4)}");

            byte[] address = ConcatAddress(preHashWNetwork, publicHashHash);
            Console.WriteLine($"Address: {ByteToHex(address)}");

            Console.WriteLine($"Human Address: {Base58Encode(address)}");
        }

        public static string Base58Encode(byte[] array)
        {
            const string ALPHABET = "123456789ABCDEFGHIJKLMNPQRSTUVWXYZabcdefghijklmnpqrstuvwxyz";
            string retString = string.Empty;
            BigInteger encodeSize = ALPHABET.Length;
            BigInteger arrayToInt = 0;
            for (int i = 0; i < array.Length; i++)
            {
                arrayToInt = arrayToInt * 256 + array[i];
            }

            while (arrayToInt > 0)
            {
                int rem = (int)(arrayToInt % encodeSize);
                arrayToInt /= encodeSize;
                retString = ALPHABET[rem] + retString;
            }

            for (int i = 0; i < array.Length && array[i] == 0; i++)
            {
                retString = ALPHABET[0] + retString;
            }

            return retString;
        }

        private static byte[] HexToByte(string hexHash)
        {
            if (hexHash.Length % 2 != 0)
            {
                throw new Exception("Invalid Hex");
            }

            byte[] result = new byte[hexHash.Length / 2];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = byte.Parse(hexHash.Substring(i * 2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }

            return result;
        }

        private static byte[] Sha256(byte[] pubKey)
        {
            SHA256Managed hashString = new SHA256Managed();
            return hashString.ComputeHash(pubKey);
        }

        private static byte[] RipeMD160(byte[] pubKeySha)
        {
            RIPEMD160Managed hashString = new RIPEMD160Managed();
            return hashString.ComputeHash(pubKeySha);
        }

        private static byte[] AppendBitcoinNetwork(byte[] pubKeyShaRIPE, byte network)
        {
            byte[] extended = new byte[pubKeyShaRIPE.Length + 1];
            extended[0] = (byte)network;
            Array.Copy(pubKeyShaRIPE, 0, extended, 1, pubKeyShaRIPE.Length);
            return extended;

        }

        private static byte[] ConcatAddress(byte[] preHashWNetwork, byte[] checkSum)
        {
            byte[] ret = new byte[preHashWNetwork.Length + 4];
            Array.Copy(preHashWNetwork, ret, preHashWNetwork.Length);
            Array.Copy(checkSum, 0, ret, preHashWNetwork.Length, 4);
            return ret;
        }

        private static string ByteToHex(byte[] pubKeySha)
        {
            byte[] data = pubKeySha;
            string hex = BitConverter.ToString(data);
            return hex;
        }
    }
}
