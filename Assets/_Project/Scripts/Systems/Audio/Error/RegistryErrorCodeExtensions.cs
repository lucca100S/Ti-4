using System;

/// <summary>
/// Extension methods for RegistryErrorCode enum with detailed object context.
/// </summary>
public static class RegistryErrorCodeExtensions
{
    public static string GetMessage(this RegistryErrorCode code, object obj = null, object context = null)
    {
        string objInfo = obj != null ? $"Object: {obj}" : "Object: <null>";
        string contextInfo = context != null ? $" Context: {context}" : string.Empty;

        return code switch
        {
            RegistryErrorCode.NullObject =>
                $"[Registry Error] Null object encountered. {objInfo}{contextInfo}",

            RegistryErrorCode.DuplicateKey =>
                $"[Registry Error] Duplicate key detected. {objInfo}{contextInfo}",

            RegistryErrorCode.KeyNotFound =>
                $"[Registry Error] Key not found. {objInfo}{contextInfo}",

            RegistryErrorCode.ObjectNotFound =>
                $"[Registry Error] Object not found in registry. {objInfo}{contextInfo}",

            RegistryErrorCode.InvalidRegistryState =>
                $"[Registry Error] Invalid registry state detected. {objInfo}{contextInfo}",

            RegistryErrorCode.None =>
                "No error.",

            _ =>
                $"[Registry Error] Unknown error. {objInfo}{contextInfo}"
        };
    }
}
