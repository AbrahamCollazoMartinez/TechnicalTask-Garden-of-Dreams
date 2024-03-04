using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityCipher;

public class Manager_SavingManagment : Singleton<Manager_SavingManagment>
{
	//this save system uses an Encrypt functionality but actually the whole system can be expanded to save this data remotaly by sendind json files
	//when the data is stored locally and encryptation is needed 
	private const string PASSWORD = "121753";

	public void SaveData(JSONObject data, string fileName)
	{
		string filePath = Path.Combine(Application.persistentDataPath, fileName);

		if (File.Exists(filePath))
		{
			File.Delete(filePath);
		}


		File.WriteAllText(filePath, ExecuteEncrypt(data.ToString()));

	}

	public (bool, string) LoadData(string fileName)
	{
		string filePath = Path.Combine(Application.persistentDataPath, fileName);

		if (File.Exists(filePath))
		{
			string jsonData = File.ReadAllText(filePath);
			Debug.Log("File found: " + filePath);

			return (true, ExecuteDecrypt(jsonData));
		}
		else
		{
			Debug.Log("File not found: " + filePath);
			return (false, default);
		}
	}

	private string ExecuteEncrypt(string text)
	{
		return RijndaelEncryption.Encrypt(text, PASSWORD);
	}

	private string ExecuteDecrypt(string text)
	{
		return RijndaelEncryption.Decrypt(text, PASSWORD);
	}
}


