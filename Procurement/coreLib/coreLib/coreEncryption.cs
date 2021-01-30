using _Encryption._Encryption.Core;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace coreLib.Encryption
{
    public class Sym
    {
        private string _SALT = "I'm Mighty";

        public Sym(string salt = "")
        {
            if (string.IsNullOrEmpty(salt))
                return;
            _SALT = salt;
        }

        public string Encrypt(string s)
        {
            Symmetric symmetric = new Symmetric(Symmetric.Provider.Rijndael, true);
            Data data = new Data(this._SALT);
            Data d = new Data(s);
            Data key = data;

            return symmetric.Encrypt(d, key).ToBase64();
        }

        public string Decrypt(string s)
        {
            Symmetric symmetric = new Symmetric(Symmetric.Provider.Rijndael, true);
            Data data = new Data("I'm Mighty");
            Data encryptedData = new Data() { Base64 = s };
            Data key = data;
            return symmetric.Decrypt(encryptedData, key).ToString();
        }
    }

    public class Base64
    {
        public string Encode(string toEncode)
        {
            byte[] toEncodeAsBytes
                  = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
            string returnValue
                  = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }

        public string Decode(string encodedData)
        {
            byte[] encodedDataAsBytes
                = System.Convert.FromBase64String(encodedData);
            string returnValue =
               System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);
            return returnValue;
        }
    }

    public class EncryptDecryptQueryString
    {
        private byte[] key = { };
        private byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xab, 0xcd, 0xef };
        public string Decrypt(string stringToDecrypt, string sEncryptionKey)
        {
            byte[] inputByteArray = new byte[stringToDecrypt.Length + 1];
            try
            {
                key = System.Text.Encoding.UTF8.GetBytes(sEncryptionKey);
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(stringToDecrypt);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms,
                  des.CreateDecryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                System.Text.Encoding encoding = System.Text.Encoding.UTF8;
                return encoding.GetString(ms.ToArray());
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string Encrypt(string stringToEncrypt, string SEncryptionKey)
        {
            try
            {
                key = System.Text.Encoding.UTF8.GetBytes(SEncryptionKey);
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = Encoding.UTF8.GetBytes(stringToEncrypt);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms,
                  des.CreateEncryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}