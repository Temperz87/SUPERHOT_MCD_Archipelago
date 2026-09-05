using System;
using System.Text;
using HarmonyLib;
using Statistics;
using SystemStorage;

// Patches responsible for redirecting the Archipelago to a new save file

[HarmonyPatch]
public static class Ensure_ArchipelagoDataOnly
{
    [HarmonyPatch(typeof(SHRLSaveManager), nameof(SHRLSaveManager.SaveAsync))]
    [HarmonyPrefix]
    public static bool Prefix_SaveAsync(SHRLSaveManager __instance, ref bool ___saveFileExist, Action<StorageResult> OnFinish)
    {
        if (!ArchipelagoManager.Connected)
            return true;
        
        if (!Singleton<PlayerManager>.Instance.Storage.IsReadyToUse)
		{
			OnFinish.SafeCall(StorageResult.NotReadyToUse);
			return false;
		}

        // Global statistics controls the difficulty level
        // Hence we save the results here
		GlobalStatistics.Instance.Save();
		string text = Traverse.Create(__instance).Method("SerializeSaveData").GetValue<string>();
		Singleton<PlayerManager>.Instance.Storage.SetContainer(ArchipelagoDataManager.SaveFile, Encoding.UTF8.GetBytes(text));
		Singleton<PlayerManager>.Instance.Storage.CommitAsync(OnFinish, CommitParams.None);
		___saveFileExist = true;
        return false;
    }

    [HarmonyPatch(typeof(SHRLSaveManager), nameof(SHRLSaveManager.LoadAsync))]
    [HarmonyPrefix]
    public static bool Prefix_LoadAsync(SHRLSaveManager __instance, SHRLSaveManager.OnLoadAsyncDelegate onFinish)
    {
        if (!ArchipelagoManager.Connected)
            return true;

        // Sometimes there is no better way to do something...
        Singleton<PlayerManager>.Instance.Storage.GetContainerAsync(ArchipelagoDataManager.SaveFile, 
            delegate(StorageResult result, byte[] bytes)
		{
            Traverse traverse = Traverse.Create(__instance);
			StorageResult storageResult;
			if (result)
				storageResult = traverse.Method("DeserializeSaveData", [typeof(byte[])]).GetValue<bool>(bytes) 
                    ? result : new StorageResult(StorageResultType.DataBroken, null);
			else
				storageResult = result;
            
            traverse.Field("saveFileExist").SetValue((bool)result);
			LevelSetup.Initialize();
			DifficultyManager.Reinitialize();
			SHRLGame.MarkForReinitializeIfAvailable();
			if (onFinish != null)
				onFinish(storageResult);
		}, GetContainerParams.None);

        return false;
    }
}
