
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using SUPERHOT_MCD_Mod;

public class RunInfo
{
    public string Filename { get; set; }
    public RunID RunID { get; set; }
}

public class DebugRunData
{
    public List<RunInfo> Items { get; set; }
    public List<RunInfo> Runs { get; set; }
    public List<RunInfo> DontRandomize { get; set; }
}

public static class DebugLevelRandomizer
{
    public static void Remap()
    {
        // First, collect all cells we need (runs, terminals) 
        // and shallow copy them into a dict
        Dictionary<RunID, PyramidCellData> newCells = new();
        List<PyramidDataContainer> pyrmaids = GameData.Instance.PyramidList.Pyramids;
        foreach (var pyramid in pyrmaids)
        {
            foreach (var row in pyramid.Map.Columns)
            {
                foreach (PyramidCellData col in row.Column)
                {
                    if (col.Type == CellType.Path && col.blockedKey != null)
                    {
                        Plugin.Logger.LogDebug("Found path! Blocked by: " + col.blockedKey);
                    }

                    if (col.Type != CellType.Run && col.Type != CellType.Terminal)
                        continue;
                    
                    string info = (col.Run == null)? "none" : col.Run.RunID.ToString();
                    if (col.Run != null)
                    {
                        newCells.Add(col.Run.RunID, new(col));
                    }
                }
            }
        }

        // Then, go through again and remap everything
        foreach (var pyramid in pyrmaids)
        {
            foreach (var row in pyramid.Map.Columns)
            {
                for (int i = 0; i < row.Column.Count; i++)
                {
                    PyramidCellData col = row.Column[i];
                    if (col.Type != CellType.Run && col.Type != CellType.Terminal)
                        continue;

                    if (!ArchipelagoDataManager.RemappedRuns.ContainsKey(col.Run.RunID))
                        continue;

                    RunID remapped = ArchipelagoDataManager.RemappedRuns[col.Run.RunID];
                    if (!newCells.ContainsKey(remapped))
                    {
                        Plugin.Logger.LogError($"\tCouldn't find cell {(int)remapped}");
                        throw new KeyNotFoundException();
                    }
                    
                    // Probably don't need to copy construct
                    row.Column[i] = new(newCells[remapped]);
                }
            }
        }
    }

    public static void DebugRandomize()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        string resourceName = "SUPERHOT_MCD_Mod.level-pools.json";                
        List<RunID> itemRunIds;
        List<RunID> levelRunIds;
        List<RunID> dontRandomizeRunIds;

        using (Stream stream = assembly.GetManifestResourceStream(resourceName))
        {
            if (stream == null)
                throw new FileNotFoundException($"Resource {resourceName} not found.");

            using (StreamReader reader = new StreamReader(stream))
            {
                DebugRunData data = JsonConvert.DeserializeObject<DebugRunData>(reader.ReadToEnd());
                itemRunIds = data.Items.Select(x => x.RunID).ToList();
                levelRunIds = data.Runs.Select(x => x.RunID).ToList();
                dontRandomizeRunIds = data.DontRandomize.Select(x => x.RunID).ToList();
            }
        }

        List<RunID> yetToBeChosenItems = itemRunIds.ToList();
        List<RunID> yetToBeChosenRuns = levelRunIds.ToList();
        Random random = new();
        foreach (RunID id in itemRunIds)
        {
            int randomIndex = random.Next(yetToBeChosenItems.Count);
            RunID chosen = yetToBeChosenItems[randomIndex];
            yetToBeChosenItems.Remove(chosen);
            ArchipelagoDataManager.RemappedRuns.Add(id, chosen);
        }

        foreach (RunID id in levelRunIds)
        {
            int randomIndex = random.Next(yetToBeChosenRuns.Count);
            RunID chosen = yetToBeChosenRuns[randomIndex];
            yetToBeChosenRuns.Remove(chosen);
            ArchipelagoDataManager.RemappedRuns.Add(id, chosen);
        }

        Remap();
    }
}
