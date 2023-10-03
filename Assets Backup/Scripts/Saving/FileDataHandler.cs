using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Newtonsoft.Json;

public class FileDataHandler
{

    private string dataDirPath = "";
    private string saveDirPath = "";
    private string playerDataFileName = "";
    private string worldDataFileName = "";

    private bool useEncryption = false;
    private readonly string encryptionCodeWord = "COCKANDBALL";
    private readonly string backupExtension = ".bak";

    public FileDataHandler(string dataDirPath, string playerDataFileName, string worldDataFileName, bool useEncryption)
    {
        this.dataDirPath = dataDirPath;
        saveDirPath = Path.Combine(this.dataDirPath, "saves");
        this.playerDataFileName = playerDataFileName;
        this.worldDataFileName = worldDataFileName;
        this.useEncryption = useEncryption;
    }

    public GameDataMaster Load(string profileID, bool allowRestore = true)
    {
        if(profileID == null)
        {
            return null;
        }

        string fullPlayerPath = GetFullPath(profileID, playerDataFileName);
        string fullWorldPath = GetFullPath(profileID, worldDataFileName);
        GameDataMaster loadedData = new GameDataMaster();
        if(File.Exists(fullPlayerPath) && File.Exists(fullWorldPath))
        {

            // Load player data
            try
            {
                string playerDataToLoad = "";
                using (FileStream stream = new FileStream(fullPlayerPath, FileMode.Open))
                {
                    using(StreamReader reader = new StreamReader(stream))
                    {
                        playerDataToLoad = reader.ReadToEnd();
                    }
                }

                if (useEncryption)
                {
                    playerDataToLoad = EncryptDecrypt(playerDataToLoad);
                }

                loadedData.playerGameData = JsonConvert.DeserializeObject<PlayerGameData>(playerDataToLoad);
            }
            catch (Exception e)
            {
                if (allowRestore)
                {
                    Debug.LogWarning("Error occured when trying to load data to file, attempting rollback" + "\n" + e);
                    bool playerRollbackSuccess = AttemptRollback(fullPlayerPath);
                    if (playerRollbackSuccess)
                    {
                        loadedData = Load(profileID, false);
                    }
                }
                else
                {
                    Debug.LogError("Filed to load file or backup at " + fullPlayerPath + e);
                }
            }

            // Load world data
            try
            {
                string worldDataToLoad = "";
                using (FileStream stream = new FileStream(fullWorldPath, FileMode.Open))
                {
                    using(StreamReader reader = new StreamReader(stream))
                    {
                        worldDataToLoad = reader.ReadToEnd();
                    }
                }

                if (useEncryption)
                {
                    worldDataToLoad = EncryptDecrypt(worldDataToLoad);
                }

                loadedData.worldGameData = JsonConvert.DeserializeObject<WorldGameData>(worldDataToLoad);
            }
            catch (Exception e)
            {
                if (allowRestore)
                {
                    Debug.LogWarning("Error occured when trying to load data to file, attempting rollback" + "\n" + e);
                    bool worldRollbackSuccess = AttemptRollback(fullWorldPath);
                    if (worldRollbackSuccess)
                    {
                        loadedData = Load(profileID, false);
                    }
                }
                else
                {
                    Debug.LogError("Filed to load file or backup at " + fullWorldPath + e);
                }
            }
        }
        else
        {
            if(!File.Exists(fullWorldPath) && File.Exists(fullPlayerPath))
            {
                Debug.LogWarning("Missing world data on profile " + profileID + ", attempting restore");
                bool worldRollbackSuccess = AttemptRollback(fullWorldPath);
                if(worldRollbackSuccess)
                {
                    loadedData = Load(profileID);
                }
                else
                {
                    Debug.LogError("World data could not be found");
                    return null;
                }
            }
            else
            {
                if(!File.Exists(fullPlayerPath) && File.Exists(fullWorldPath))
                {
                    Debug.LogWarning("Missing player data on profile " + profileID + ", attempting restore");
                    bool playerRollbackSuccess = AttemptRollback(fullPlayerPath);
                    if(playerRollbackSuccess)
                    {
                        loadedData = Load(profileID);
                    }
                    else
                    {
                        Debug.LogError("Player data could not be found");
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }
        return loadedData;
    }

    public void Save(GameDataMaster data, string profileID) 
    {
        if(profileID == null)
        {
            return;
        }
        string fullPlayerPath = GetFullPath(profileID, playerDataFileName);
        string fullWorldPath = GetFullPath(profileID, worldDataFileName);
        string backupPlayerFilePath = fullPlayerPath + backupExtension;
        string backupWorldFilePath = fullWorldPath + backupExtension;
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPlayerPath));
            Directory.CreateDirectory(Path.GetDirectoryName(fullWorldPath));

           string playerDataToStore = JsonConvert.SerializeObject(data.playerGameData,formatting:Formatting.Indented,
           new JsonSerializerSettings
           {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
           });

            string worldDataToStore = JsonConvert.SerializeObject(data.worldGameData,formatting:Formatting.Indented,
           new JsonSerializerSettings
           {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
           });

           if (useEncryption)
           {
                playerDataToStore = EncryptDecrypt(playerDataToStore);
                worldDataToStore = EncryptDecrypt(worldDataToStore);
           }

           using (FileStream stream = new FileStream(fullPlayerPath, FileMode.Create))
           {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(playerDataToStore);
                }
           }

            using (FileStream stream = new FileStream(fullWorldPath, FileMode.Create))
           {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(worldDataToStore);
                }
           }

           GameDataMaster verifiedGameData = Load(profileID);

           if (verifiedGameData != null)
           {
                File.Copy(fullPlayerPath, backupPlayerFilePath, true);
                File.Copy(fullWorldPath, backupWorldFilePath, true);
           }
           else
           {
                throw new Exception("Save file could not be verified");
           }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file: " + fullPlayerPath + " and " + fullWorldPath + "\n" + e);
        }
    }

    public void Rename(string newProfileID, string oldProfileID)
    {
        string oldPath = GetFullPath(oldProfileID, playerDataFileName);
        string newPath = GetFullPath(newProfileID, playerDataFileName);
        try
        {
            Directory.Move(Path.GetDirectoryName(oldPath),Path.GetDirectoryName(newPath));
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to rename folder: " + oldPath + " to " + newPath + "\n" + e);
        }

    }

    public void Delete(string profileID)
    {
        if (profileID == null)
        {
            return;
        }
        string fullPath = GetFullPath(profileID, playerDataFileName);

        try
        {
            if (File.Exists(fullPath))
            {
                Directory.Delete(Path.GetDirectoryName(fullPath), true);
            }
            else
            {
                Debug.LogWarning("Couldn't find data to delete at " + fullPath);
            }
        }
        catch(Exception e)
        {
            Debug.LogError("Failed to delete profile data for " + profileID + fullPath + "\n" + e);
        }
    }

    private string GetFullPath(string profileID, string dataFileName)
    {

        return Path.Combine(saveDirPath, profileID, dataFileName);
    }

    public Dictionary<string, GameDataMaster> LoadAllProfiles()
    {
        Dictionary<string, GameDataMaster> profileDictionary = new Dictionary<string, GameDataMaster>();

        IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(saveDirPath).EnumerateDirectories();

        foreach(DirectoryInfo dirInfo in dirInfos)
        {
            string profileID = dirInfo.Name;

            GameDataMaster profileData = Load(profileID);

            if(profileData != null)
            {
                profileDictionary.Add(profileID, profileData);
            }
            else
            {
                Debug.LogError("Profile couldn't load, something has gone seriously wrong. ID:" + profileID);
            }
        }

        return profileDictionary;
    }

    public bool CheckDuplicate(string name)
    {
        IEnumerable<DirectoryInfo> dirNames = new DirectoryInfo(saveDirPath).EnumerateDirectories();
        foreach(DirectoryInfo dirName in dirNames)
        {
            if (dirName.Name.ToLower() == name.ToLower())
            {
                return true;
            }
        }
        return false;
    }

    public string GetMostRecentlyUpdatedProfileID()
    {
        string mostRecentProfileID = null;

        Dictionary<string, GameDataMaster> profilesGameData = LoadAllProfiles();
        foreach(KeyValuePair<string, GameDataMaster> pair in profilesGameData)
        {
            string profileID = pair.Key;
            PlayerGameData gameData = pair.Value.playerGameData;

            if(gameData == null)
            {
                continue;
            }

            if (mostRecentProfileID == null)
            {
                mostRecentProfileID = profileID;
            }
            else
            {
                GameDataMaster mostRecentGameData = null;
                profilesGameData.TryGetValue(mostRecentProfileID, out mostRecentGameData);
                DateTime mostRecentDateTime = DateTime.FromBinary(mostRecentGameData.playerGameData.lastUpdated);
                DateTime newDateTime = DateTime.FromBinary(gameData.lastUpdated);

                if (newDateTime > mostRecentDateTime)
                {
                    mostRecentProfileID = profileID;
                }
            }
        }

        return mostRecentProfileID;
    }

    private string EncryptDecrypt(string data)
    {
        string modifedData = "";
        for (int i = 0; i < data.Length ; i++)
        {
            modifedData += (char) (data[i] ^ encryptionCodeWord[i%encryptionCodeWord.Length]);
        }
        return modifedData;
    }

    private bool AttemptRollback(string fullPath)
    {
        bool success = false;

        string backupFilePath = fullPath + backupExtension;

        try
        {
            if (File.Exists(backupFilePath))
            {
                File.Copy(backupFilePath, fullPath, true);
                success = true;
                Debug.LogWarning("Save corrupted, data rolled back to " + backupFilePath);
            }
            else
            {
                throw new Exception("No backup file to roll back to");
            }
        }
        catch(Exception e)
        {
            Debug.LogError("Could not attempt to rollback file to " + backupFilePath + "\n" + e);
        }

        return success;
    }

}
