﻿using System.Security.Cryptography;
using System.Text;

namespace MyRecipeBook.Application.Services.Crytography;
public class PasswordEncripter
{
	private readonly string _additionalKey;

    public PasswordEncripter(string additionalKey) => _additionalKey = additionalKey;
    public string Encrypt(string password)
	{
		var chaveAdicional = "ABC";

		var newPassword = $"{password}{chaveAdicional}";

		var bytes = Encoding.UTF8.GetBytes(newPassword);
		var hashBytes = SHA512.HashData(bytes);

		return StringBytes(hashBytes);
	}

	private static string StringBytes(byte[] bytes)
	{
		var sb = new StringBuilder();
		
		foreach(byte b in bytes)
		{
			var hex = b.ToString("x2");
			sb.Append(hex);
		}

		return sb.ToString();
	}
}

