using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using SimpleFFO.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;

namespace SimpleFFO.Controller
{
    public class Auth : SimpleDB
    {
        public useraccount currentuser;
        public long employeeid;
        public long warehouseid;

        public enum middleware
        {
            guest = 0,
            authenticate = 1
        }
        public bool hasAuth()
        {
            if (currentuser == null)
                return false;

            return true;
        }
        public Auth(Page page, middleware m = middleware.authenticate, int modcode=0,int submodcode=0)
        {
            employeeid = Convert.ToInt64(SessionController.getSession(SessionController.SesssionKeys.employeeid) ?? 0);
            if (employeeid > 0)
            {
                currentuser = this.useraccounts.Where(u => u.employeeid == employeeid).First();
                warehouseid = currentuser.employee.warehouseid ?? 0;
                if (modcode > 0)
                {
                    userpriv up = this.GetUserpriv(modcode);
                    if(!((up.canadd ?? false) || (up.canedit ?? false) || (up.canrequest ?? false)))
                        page.Response.RedirectToRoute(AppModels.Routes.pagenotfound, new { aspxerrorpath = "Access denied to module " + up.module.name });
                }
                if (submodcode > 0)
                {
                    usersubpriv usp = this.GetSubUserpriv(submodcode);
                    if (!(usp.canaccess ?? false))
                        page.Response.RedirectToRoute(AppModels.Routes.pagenotfound, new { aspxerrorpath = "Access denied to submodule " + usp.systemsubmodule.submoddescription });
                }
            }
            else
            {
                warehouseid = 0;
                if (m == middleware.authenticate)
                {
                    page.Response.RedirectToRoute("login");
                }
            }
        }
        public bool authenticate(string username, string userpass)
        {
            var user = this.useraccounts.FirstOrDefault(u => u.username == username && u.userpass == userpass);
            if (user != null)
            {
                user.showwhatsnew = (user.showwhatsnew ?? 0) + 1;
                this.SaveChanges();
                currentuser = user;
                if(user.showwhatsnew <= 3)
                    SessionController.setSession(SessionController.SesssionKeys.showupdates, 1);

                SessionController.setSession(SessionController.SesssionKeys.employeeid, currentuser.employeeid);
                return true;
            }
            return false;
        }

        public string GetToken()
        {
            string raw = currentuser.employee.employeeid.ToString().Trim()+ "|" + DateTime.Now.ToString("ddMMyyyy")
                + "|" + DateTime.Now.ToString("HHmm");

            return WebServiceSecurity.Encrypt(raw, "");
        }

        public void Logout()
        {
            SessionController.setSession(SessionController.SesssionKeys.employeeid, null);
        }
        public userpriv GetUserpriv(int moduleid)
        {
            var res = this.currentuser.userprivs.Where(up => up.modcode == moduleid).FirstOrDefault();
            return res ?? new userpriv();
        }
        public usersubpriv GetSubUserpriv(int submodcode)
        {
            var res = this.currentuser.usersubprivs.Where(up => up.submodcode == submodcode).FirstOrDefault();
            return res ?? new usersubpriv();
        }

        public void SaveLog(string module, string action, int whatype = 0)
        {
            userlog u = new userlog
            {
                typeid = whatype, //sfawebreport
                module = module,
                action = action,
                logdate = DateTime.Now
            };
            if (currentuser == null)
            {
                u.referenceno = 0;
                u.username = "";
            }
            else
            {
                u.referenceno = currentuser.employeeid;
                u.username = currentuser.employee.firstname + " " + currentuser.employee.lastname;
            }
            this.userlogs.Add(u);
            this.SaveChanges();
        }
        public class WebServiceSecurity
        {
            private const string initVector = "JHAYROSEBROSALES";
            private const int keysize = 256;

            public static string Encrypt(string plainText,string passPhrase)
            {
                byte[] iniVectorBytes = Encoding.UTF8.GetBytes(initVector);
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
                PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
                byte[] keyBytes = password.GetBytes(keysize / 8);
                RijndaelManaged symmetrickey = new RijndaelManaged();
                symmetrickey.Mode = CipherMode.CBC;
                ICryptoTransform encryptor = symmetrickey.CreateEncryptor(keyBytes, iniVectorBytes);
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                cryptoStream.FlushFinalBlock();
                byte[] cipherTextBytes = memoryStream.ToArray();
                memoryStream.Close();
                cryptoStream.Close();
                return Convert.ToBase64String(cipherTextBytes);
            }

            public static string Decrypt(string cipherText,string passPhrase)
            {
                byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
                byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
                PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
                byte[] keyBytes = password.GetBytes(keysize / 8);
                RijndaelManaged symmetriKey = new RijndaelManaged();
                symmetriKey.Mode = CipherMode.CBC;
                ICryptoTransform decryptor = symmetriKey.CreateDecryptor(keyBytes, initVectorBytes);
                MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
                CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                byte[] plainTextBytes = new byte[cipherTextBytes.Length - 1 + 1];
                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                memoryStream.Close();
                cryptoStream.Close();
                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
            }
        }

        public class AppSecurity
        {
            // This constant is used to determine the keysize of the encryption algorithm in bits.
            // We divide this by 8 within the code below to get the equivalent number of bytes.
            private const int Keysize = 256;

            // This constant determines the number of iterations for the password bytes generation function.
            private const int DerivationIterations = 1000;

            public static string Encrypt(string plainText, string passPhrase = "")
            {
                // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
                // so that the same Salt and IV values can be used when decrypting.  
                byte[] saltStringBytes = Generate256BitsOfStaticEntropy();
                byte[] ivStringBytes = Generate256BitsOfStaticEntropy();

                var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
                using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
                {
                    var keyBytes = password.GetBytes(Keysize / 8);
                    using (var symmetricKey = new RijndaelManaged())
                    {
                        symmetricKey.BlockSize = 256;
                        symmetricKey.Mode = CipherMode.CBC;
                        symmetricKey.Padding = PaddingMode.PKCS7;
                        using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                                {
                                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                    cryptoStream.FlushFinalBlock();
                                    // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
                                    var cipherTextBytes = saltStringBytes;
                                    cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
                                    cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                                    memoryStream.Close();
                                    cryptoStream.Close();
                                    return Convert.ToBase64String(cipherTextBytes);
                                }
                            }
                        }
                    }
                }
            }

            public static string Decrypt(string cipherText, string passPhrase = "")
            {
                // Get the complete stream of bytes that represent:
                // [32 bytes of Salt] + [32 bytes of IV] + [n bytes of CipherText]
                var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
                // Get the saltbytes by extracting the first 32 bytes from the supplied cipherText bytes.
                var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();
                // Get the IV bytes by extracting the next 32 bytes from the supplied cipherText bytes.
                var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
                // Get the actual cipher text bytes by removing the first 64 bytes from the cipherText string.
                var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((Keysize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((Keysize / 8) * 2)).ToArray();

                using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
                {
                    var keyBytes = password.GetBytes(Keysize / 8);
                    using (var symmetricKey = new RijndaelManaged())
                    {
                        symmetricKey.BlockSize = 256;
                        symmetricKey.Mode = CipherMode.CBC;
                        symmetricKey.Padding = PaddingMode.PKCS7;
                        using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                        {
                            using (var memoryStream = new MemoryStream(cipherTextBytes))
                            {
                                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                                {
                                    var plainTextBytes = new byte[cipherTextBytes.Length];
                                    var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                    memoryStream.Close();
                                    cryptoStream.Close();
                                    return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                                }
                            }
                        }
                    }
                }
            }

            private static byte[] Generate256BitsOfStaticEntropy()
            {
                string authkey = "SimpleSoftechSolutionsCompany123";
                byte[] bytes = Encoding.ASCII.GetBytes(authkey);
                return bytes;
            }


            public const string EncryptionKey = "SimpleSoftechCo.";
            public static string URLEncrypt(string clearText)
            {
                byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                            cs.Close();
                        }
                        clearText = Convert.ToBase64String(ms.ToArray());
                    }
                }
                clearText = clearText.Replace("/", "_slash_").Replace("+", " ");
                return clearText;
            }

            public static string URLDecrypt(string cipherText)
            {
                cipherText = cipherText.Replace(" ", "+").Replace("_slash_", "/");
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
                return cipherText;
            }


            private static readonly string key = "lkirwf897+22#bbtrm8814z5qq=498j5"; // 32 * 8 = 256 bit key
            private static readonly string iv = "741952hheeyy66#cs!9hjv887mxx7@8y";
            public static string DecryptwithAes(string input)
            {
                try
                {
                    byte[] ciphertext = Convert.FromBase64String(input);
                    PaddedBufferedBlockCipher aes = new PaddedBufferedBlockCipher(new CbcBlockCipher(new RijndaelEngine(256)), new ZeroBytePadding());

                    ICipherParameters ivAndKey = new ParametersWithIV(new KeyParameter(Encoding.ASCII.GetBytes(key)), Encoding.ASCII.GetBytes(iv));
                    aes.Init(false, ivAndKey);
                    return Encoding.UTF8.GetString(cipherData(aes, ciphertext));
                }
                catch
                {
                    return "";
                }
            }
            private static byte[] cipherData(PaddedBufferedBlockCipher cipher, byte[] data)
            {

                int minSize = cipher.GetOutputSize(data.Length);
                byte[] outBuf = new byte[minSize];
                int length1 = cipher.ProcessBytes(data, 0, data.Length, outBuf, 0);
                int length2 = cipher.DoFinal(outBuf, length1);
                int actualLength = length1 + length2;
                byte[] cipherArray = new byte[actualLength];
                for (int x = 0; x < actualLength; x++)
                {
                    cipherArray[x] = outBuf[x];
                }
                return cipherArray;
            }
        }
    }
}