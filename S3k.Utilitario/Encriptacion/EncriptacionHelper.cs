
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace S3k.Utilitario.Encriptacion {
    public class EncriptacionHelper {
        public static readonly string dynamicKey = "Proyecto@10Gladcon";
        public static string DecryptMachineId(string encryptedHash) {
            try {
                string base64 = encryptedHash.Replace('-', '+').Replace('_', '/');
                switch(base64.Length % 4) {
                    case 2:
                        base64 += "==";
                        break;
                    case 3:
                        base64 += "=";
                        break;
                }
                byte[] fullCipherBytes = Convert.FromBase64String(base64);

                // 2. Extraer el IV (Los primeros 16 bytes)
                byte[] iv = new byte[16];
                byte[] cipherText = new byte[fullCipherBytes.Length - 16];

                Buffer.BlockCopy(fullCipherBytes, 0, iv, 0, 16);
                Buffer.BlockCopy(fullCipherBytes, 16, cipherText, 0, fullCipherBytes.Length - 16);

                // 3. Generar la LLAVE (La misma lógica que el cliente)
                const string clave1 = "Proyecto@10Gladconqwerty"; // Misma clave estática
                string clave2 = dynamicKey;
                byte[] aesKey = GetKeyBytes(clave1 + clave2, 32);

                // 4. Desencriptar usando la Llave generada y el IV extraído
                using(Aes aesAlg = Aes.Create()) {
                    aesAlg.Key = aesKey;
                    aesAlg.IV = iv; // Usamos el IV que venía en el mensaje
                    aesAlg.Mode = CipherMode.CBC;
                    aesAlg.Padding = PaddingMode.PKCS7;

                    using(ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV))
                    using(MemoryStream msDecrypt = new MemoryStream(cipherText))
                    using(CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    using(StreamReader srDecrypt = new StreamReader(csDecrypt)) {
                        return srDecrypt.ReadToEnd();
                    }
                }
            } catch(Exception ex) {
                // Manejo de error si la clave es incorrecta o el formato está mal
                return "ERROR_DECRYPTION: " + ex.Message;
            }
        }

        private static byte[] GetKeyBytes(string input, int length) {
            using(SHA256 sha = SHA256.Create()) {
                byte[] hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
                byte[] keyBytes = new byte[length];
                Array.Copy(hashBytes, keyBytes, length);
                return keyBytes;
            }
        }


    }

}
