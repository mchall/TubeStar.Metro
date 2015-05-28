using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

namespace TubeStarMetro
{
    public static class EncryptionHelpers
    {
        public static string Encrypt(string clearText)
        {
            // Generate a key and IV from the password and salt
            IBuffer aesKeyMaterial;
            IBuffer iv;
            uint iterationCount = 10000;
            GenerateKeyMaterial(iterationCount, out aesKeyMaterial, out iv);

            IBuffer plainText = CryptographicBuffer.ConvertStringToBinary(clearText, BinaryStringEncoding.Utf8);

            // Setup an AES key, using AES in CBC mode and applying PKCS#7 padding on the input
            SymmetricKeyAlgorithmProvider aesProvider = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesCbcPkcs7);
            CryptographicKey aesKey = aesProvider.CreateSymmetricKey(aesKeyMaterial);

            // Encrypt the data and convert it to a Base64 string
            IBuffer encrypted = CryptographicEngine.Encrypt(aesKey, plainText, iv);
            string ciphertextString = CryptographicBuffer.EncodeToBase64String(encrypted);

            return ciphertextString;
        }

        public static string Decrypt(string cipherText)
        {
            // Generate a key and IV from the password and salt
            IBuffer aesKeyMaterial;
            IBuffer iv;
            uint iterationCount = 10000;
            GenerateKeyMaterial(iterationCount, out aesKeyMaterial, out iv);

            // Setup an AES key, using AES in CBC mode and applying PKCS#7 padding on the input
            SymmetricKeyAlgorithmProvider aesProvider = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesCbcPkcs7);
            CryptographicKey aesKey = aesProvider.CreateSymmetricKey(aesKeyMaterial);

            // Convert the base64 input to an IBuffer for decryption
            IBuffer ciphertext = CryptographicBuffer.DecodeFromBase64String(cipherText);

            // Decrypt the data and convert it back to a string
            IBuffer decrypted = CryptographicEngine.Decrypt(aesKey, ciphertext, iv);
            byte[] decryptedArray = decrypted.ToArray();
            string decryptedString = Encoding.UTF8.GetString(decryptedArray, 0, decryptedArray.Length);

            return decryptedString;
        }

        private static void GenerateKeyMaterial(uint iterationCount, out IBuffer keyMaterial, out IBuffer iv)
        {
            // Setup KDF parameters for the desired salt and iteration count
            IBuffer saltBuffer = CryptographicBuffer.ConvertStringToBinary("?GZR)#!:t4OnJA4", BinaryStringEncoding.Utf8);
            KeyDerivationParameters kdfParameters = KeyDerivationParameters.BuildForPbkdf2(saltBuffer, iterationCount);

            // Get a KDF provider for PBKDF2, and store the source password in a Cryptographic Key
            KeyDerivationAlgorithmProvider kdf = KeyDerivationAlgorithmProvider.OpenAlgorithm(KeyDerivationAlgorithmNames.Pbkdf2Sha256);
            IBuffer passwordBuffer = CryptographicBuffer.ConvertStringToBinary("a-_-*=}^^808+)Y", BinaryStringEncoding.Utf8);
            CryptographicKey passwordSourceKey = kdf.CreateKey(passwordBuffer);

            // Generate key material from the source password, salt, and iteration count.  Only call DeriveKeyMaterial once,
            // since calling it twice will generate the same data for the key and IV.
            int keySize = 256 / 8;
            int ivSize = 128 / 8;
            uint totalDataNeeded = (uint)(keySize + ivSize);
            IBuffer keyAndIv = CryptographicEngine.DeriveKeyMaterial(passwordSourceKey, kdfParameters, totalDataNeeded);

            // Split the derived bytes into a seperate key and IV
            byte[] keyMaterialBytes = keyAndIv.ToArray();
            keyMaterial = WindowsRuntimeBuffer.Create(keyMaterialBytes, 0, keySize, keySize);
            iv = WindowsRuntimeBuffer.Create(keyMaterialBytes, keySize, ivSize, ivSize);
        }
    }
}
