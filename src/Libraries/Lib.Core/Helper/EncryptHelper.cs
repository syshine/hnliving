using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Lib.Core.Helper
{
    public class EncryptHelper
    {
        #region DES加密解密
        /// <summary>
        /// DES加密(8位key,key=iv)
        /// </summary>
        /// <param name="destString">结果字符串</param>
        /// <param name="encryptString">源字符串</param>
        /// <param name="key">用于对称算法的密钥</param>
        /// <returns>错误信息（成功时为空）</returns>
        public static string DesEncrypt(out string destString, string encryptString, string key)
        {
            return DesEncrypt(out destString, encryptString, key, key);
        }

        /// <summary>
        /// DES解密(8位key,key=iv)
        /// </summary>
        /// <param name="destString">结果字符串</param>
        /// <param name="decryptString">源字符串</param>
        /// <param name="key">用于对称算法的密钥</param>
        /// <returns>错误信息（成功时为空）</returns>
        public static string DesDecrypt(out string destString, string decryptString, string key)
        {
            return DesDecrypt(out destString, decryptString, key, key);
        }

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="destString">结果字符串</param>
        /// <param name="encryptString">源字符串</param>
        /// <param name="key">用于对称算法的密钥</param>
        /// <param name="iv">用于对称算法的初始化向量</param>
        /// <returns>错误信息（成功时为空）</returns>
        public static string DesEncrypt(out string destString, string encryptString, string key, string iv)
        {
            try
            {
                if (string.IsNullOrEmpty(encryptString))
                {
                    destString = "";
                    return "";
                }

                byte[] keyBytes = Encoding.UTF8.GetBytes(key.Substring(0, 8));  // 只能是8位
                byte[] keyIV = Encoding.UTF8.GetBytes(iv.Substring(0, 8));      // 只能是8位
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, provider.CreateEncryptor(keyBytes, keyIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                destString = Convert.ToBase64String(mStream.ToArray());
                return "";
            }
            catch (System.Exception ex)
            {
                destString = "";
                return ex.Message;
            }
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="destString">结果字符串</param>
        /// <param name="decryptString">源字符串</param>
        /// <param name="key">用于对称算法的密钥</param>
        /// <param name="iv">用于对称算法的初始化向量</param>
        /// <returns>错误信息（成功时为空）</returns>
        public static string DesDecrypt(out string destString, string decryptString, string key, string iv)
        {
            try
            {
                if (string.IsNullOrEmpty(decryptString))
                {
                    destString = "";
                    return "";
                }

                byte[] keyBytes = Encoding.UTF8.GetBytes(key.Substring(0, 8));  // 只能是8位
                byte[] keyIV = Encoding.UTF8.GetBytes(iv.Substring(0, 8));      // 只能是8位
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, provider.CreateDecryptor(keyBytes, keyIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                destString = Encoding.UTF8.GetString(mStream.ToArray());
                return "";
            }
            catch (Exception ex)
            {
                destString = "";
                return ex.Message;
            }
        }
        #endregion

        #region MD5加密
        /// <summary>
        /// MD5 16位加密 加密后为大写
        /// </summary>
        /// <param name="destString">结果字符串</param>
        /// <param name="ConvertString">源字符串</param>
        /// <returns>错误信息（成功时为空）</returns>
        public static string Md5Encrypt16(out string destString, string ConvertString)
        {
            try
            {
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                destString = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(ConvertString)), 4, 8);
                destString = destString.Replace("-", "");
                return "";
            }
            catch (System.Exception ex)
            {
                destString = "";
                return ex.Message;
            }
        }

        /// <summary>
        /// MD5 32位加密
        /// </summary>
        /// <param name="destString">结果字符串</param>
        /// <param name="ConvertString">源字符串</param>
        /// <returns>错误信息（成功时为空）</returns>
        public static string Md5Encrypt32(out string destString, string ConvertString)
        {
            try
            {
                destString = "";

                #region 方法一
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                destString = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(ConvertString)));
                destString = destString.Replace("-", "");
                #endregion

                #region // 方法二
                //MD5 md5 = MD5.Create();//实例化一个md5对像
                //// 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
                //byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(ConvertString));
                //// 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
                //for (int i = 0; i < s.Length; i++)
                //{
                //    // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
                //    destString = destString + s[i].ToString("X").PadLeft(2, '0');
                //}
                #endregion

                return "";
            }
            catch (Exception ex)
            {
                destString = "";
                return ex.Message;
            }
        }
        #endregion

        #region //压缩
        ///// <summary>
        ///// 压缩
        ///// </summary>
        ///// <param name="destString">结果字符串</param>
        ///// <param name="compressString">源字符串</param>
        ///// <returns>错误信息（成功时为空）</returns>
        //public static string Compress(out string destString, string compressString)
        //{
        //    string errorMsg = "";

        //    #region // ICSharpCode.SharpZipLib
        //    /* ********************使用ICSharpCode.SharpZipLib，压缩结果与delphi的zlib不符******************** 

        //    byte[] bytData = System.Text.Encoding.UTF8.GetBytes(uncompressedString);

        //    MemoryStream ms = new MemoryStream();
        //    try
        //    {
        //        Deflater mDeflater = new Deflater(ICSharpCode.SharpZipLib.Zip.Compression.Deflater.DEFAULT_COMPRESSION);
        //        ICSharpCode.SharpZipLib.Zip.Compression.Streams.DeflaterOutputStream s =
        //            new ICSharpCode.SharpZipLib.Zip.Compression.Streams.DeflaterOutputStream(ms, mDeflater, 4096);
        //        s.Write(bytData, 0, bytData.Length);
        //        s.Close();
        //        byte[] compressedData = ms.ToArray();

        //        string compressedStr = System.Text.Encoding.UTF8.GetString(compressedData);
        //        return Convert.ToBase64String(compressedData);
        //    }
        //    finally
        //    {
        //        ms.Close();
        //    }*/
        //    #endregion

        //    #region zlib.net
        //    /*******************************使用zlib.net，可行*******************************/
        //    MemoryStream ms = new MemoryStream();
        //    try
        //    {
        //        byte[] bytData = System.Text.Encoding.UTF8.GetBytes(compressString);
        //        zlib.ZOutputStream outputStream = new zlib.ZOutputStream(ms, zlib.zlibConst.Z_DEFAULT_COMPRESSION);

        //        outputStream.Write(bytData, 0, bytData.Length);
        //        outputStream.Close();
        //        byte[] compressedData = ms.ToArray();

        //        //string compressedStr = System.Text.Encoding.UTF8.GetString(compressedData);

        //        destString = Convert.ToBase64String(compressedData);

        //        return "";
        //    }
        //    catch (System.Exception ex)
        //    {
        //        destString = "";
        //        errorMsg = ex.Message;
        //    }
        //    finally
        //    {
        //        ms.Close();
        //    }
        //    #endregion

        //    #region  // ZLib .NET Wrapper
        //    /***********************使用ZLib .NET Wrapper*********************************
        //    //初始化,很重要，程序开始时执行
        //    ManagedZLib.ManagedZLib.Initialize();


        //    byte[] bytData = System.Text.Encoding.UTF8.GetBytes(uncompressedString);
        //    MemoryStream ms = new MemoryStream();
        //    ManagedZLib.CompressionStream zlibStream = new ManagedZLib.CompressionStream(ms, ManagedZLib.CompressionOptions.Compress);
        //    try
        //    {
        //        zlibStream.Write(bytData, 0, bytData.Length);
        //        zlibStream.Close();
        //        byte[] compressedData = ms.ToArray();

        //        string compressedStr = System.Text.Encoding.UTF8.GetString(compressedData);
        //        //return compressedStr;
        //        return Convert.ToBase64String(compressedData);
        //    }
        //    finally
        //    {
        //        ms.Close();
        //        ManagedZLib.ManagedZLib.Terminate();//释放资源,程序结束时执行
        //    }*/
        //    #endregion

        //    return errorMsg;
        //}

        ///// <summary>
        ///// 解压
        ///// </summary>
        ///// <param name="destString">结果字符串</param>
        ///// <param name="compressedString">源字符串</param>
        ///// <returns>错误信息（成功时为空）</returns>
        //public static string DeCompress(out string destString, string compressedString)
        //{
        //    string errorMsg = "";

        //    #region ICSharpCode.SharpZipLib
        //    /* ********************使用ICSharpCode.SharpZipLib********************  
        //    //byte[] bytInput = Encoding.UTF8.GetBytes(compressedString);
        //    byte[] bytInput = Convert.FromBase64String(compressedString);
        //    byte[] writeData = new byte[4096];

        //    ICSharpCode.SharpZipLib.Zip.Compression.Streams.InflaterInputStream mStream
        //        = new ICSharpCode.SharpZipLib.Zip.Compression.Streams.InflaterInputStream(new MemoryStream(bytInput));

        //    MemoryStream mMemory = new MemoryStream();
        //    try
        //    {
        //        Int32 mSize;
        //        byte[] mWriteData = new byte[4096];
        //        while (true)
        //        {
        //            mSize = mStream.Read(mWriteData, 0, mWriteData.Length);
        //            if (mSize > 0)
        //            {
        //                mMemory.Write(mWriteData, 0, mSize);
        //            }
        //            else
        //            {
        //                break;
        //            }
        //        }
        //        mStream.Close();
        //        byte[] byteDest = mMemory.ToArray();
        //        return Encoding.UTF8.GetString(byteDest, 0, byteDest.Length);
        //    }
        //    finally
        //    {
        //        mMemory.Close();
        //    }
        //    */
        //    #endregion

        //    #region zlib.net
        //    /*******************************使用zlib.net，可行*******************************/
        //    MemoryStream ms = new MemoryStream();

        //    try
        //    {
        //        byte[] bytInput = Convert.FromBase64String(compressedString);
        //        zlib.ZOutputStream outputStream = new zlib.ZOutputStream(ms);

        //        outputStream.Write(bytInput, 0, bytInput.Length);
        //        outputStream.Close();

        //        byte[] decompressedData = ms.ToArray();
        //        destString = Encoding.UTF8.GetString(decompressedData, 0, decompressedData.Length);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        destString = "";
        //        errorMsg = ex.Message;
        //    }
        //    finally
        //    {
        //        ms.Close();
        //    }
        //    #endregion

        //    #region ZLib .NET Wrapper
        //    /***********************使用ZLib .NET Wrapper*********************************

        //    //初始化,很重要，程序开始时执行
        //    ManagedZLib.ManagedZLib.Initialize();


        //    byte[] bytInput = Convert.FromBase64String(compressedString);
        //    MemoryStream ms = new MemoryStream();

        //    ManagedZLib.CompressionStream zlibStream = new ManagedZLib.CompressionStream(new MemoryStream(bytInput), ManagedZLib.CompressionOptions.Decompress);
        //    try
        //    {
        //        Int32 mSize;
        //        byte[] mWriteData = new byte[4096];
        //        while (true)
        //        {
        //            mSize = zlibStream.Read(mWriteData, 0, mWriteData.Length);
        //            if (mSize > 0)
        //            {
        //                ms.Write(mWriteData, 0, mSize);
        //            }
        //            else
        //            {
        //                break;
        //            }
        //        }
        //        zlibStream.Close();
        //        byte[] decompressedData = ms.ToArray();
        //        return Encoding.UTF8.GetString(decompressedData, 0, decompressedData.Length);
        //    }
        //    finally
        //    {
        //        ms.Close();
        //        ManagedZLib.ManagedZLib.Terminate();//释放资源,程序结束时执行
        //    }*/
        //    #endregion

        //    return errorMsg;
        //}
        #endregion

        #region 异或
        /// <summary>
        /// 异或
        /// </summary>
        /// <param name="destString">结果字符串</param>
        /// <param name="ConvertString">源字符串</param>
        /// <param name="key">异或键</param>
        /// <returns>错误信息（成功时为空）</returns>
        public static string XorEncrypt(out string destString, string ConvertString, int key)
        {
            try
            {
                ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                byte[] arrSource = asciiEncoding.GetBytes(ConvertString);
                byte[] arr = new byte[ConvertString.Length];
                for (int i = 0; i < ConvertString.Length; i++)
                {
                    arr[i] = (byte)((int)arrSource[i] ^ key);
                }
                destString = asciiEncoding.GetString(arr);
                return "";
            }
            catch (Exception ex)
            {
                destString = "";
                return ex.Message;
            }
        }
        #endregion
    }
}
