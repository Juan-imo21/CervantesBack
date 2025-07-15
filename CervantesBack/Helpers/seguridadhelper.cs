using System.Security.Cryptography;
using System.Text;

public static class SeguridadHelper
{
    private static string ClaveAES = "ClaveSuperSecreta123!"; // 🔒 Guardar en config segura

    public static string EncriptarAES(string texto)
    {
        byte[] key = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(ClaveAES));
        byte[] iv = new byte[16]; // IV fijo: podrías usar uno aleatorio para más seguridad

        using var aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;

        var encryptor = aes.CreateEncryptor();
        byte[] inputBytes = Encoding.UTF8.GetBytes(texto);
        byte[] result = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);

        return Convert.ToBase64String(result);
    }

    public static string HashSHA256(string texto)
    {
        using var sha = SHA256.Create();
        byte[] bytes = Encoding.UTF8.GetBytes(texto);
        byte[] hash = sha.ComputeHash(bytes);
        return Convert.ToHexString(hash); // o Convert.ToBase64String(hash)
    }

    public static string ProcesarContrasena(string contrasenaTextoPlano)
    {
        string cifrado = EncriptarAES(contrasenaTextoPlano);
        return HashSHA256(cifrado);
    }
}