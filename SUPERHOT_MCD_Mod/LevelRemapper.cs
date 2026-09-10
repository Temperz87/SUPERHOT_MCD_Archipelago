
using System.Collections.Generic;
using SUPERHOT_MCD_Mod;


// TODO:
// Nodes 1a, 1b, 1c, and probably another always appear as unlocked, even if they're unreachable currently in randomized mode
public static class LevelRemapper
{
    private static RunID[] levelOrder =
    [
        (RunID)1,    // SENSORY / NODE 1A
        (RunID)19,   // SENSORY / NODE 1B
        (RunID)2,    // SENSORY / NODE 1C
        (RunID)3,    // SENSORY / NODE 2
        (RunID)21,   // SENSORY / NODE 2A
        (RunID)22,   // SENSORY / NODE 2B
        (RunID)89,   // SENSORY / CACHE / piercshot.hack
        (RunID)93,   // SENSORY / CACHE / explode.hack
        (RunID)99,   // SENSORY / CACHE / grenade.hack
        (RunID)92,   // SENSORY / CACHE / supthrow.hack
        (RunID)58,   // QUARANTINE / BROKEN
        (RunID)4,    // SHORT / NODE 3
        (RunID)20,   // SHORT / NODE 3A
        (RunID)23,   // SHORT / NODE 3B
        (RunID)24,   // SHORT / NODE 3C
        (RunID)5,    // SHORT / NODE 4
        (RunID)25,   // SHORT / NODE 4A
        (RunID)27,   // SHORT / NODE 4C
        (RunID)59,   // QUARANTINE / UNSTABLE
        (RunID)60,   // QUARANTINE / TOXIC
        (RunID)90,   // SHORT / CACHE / defall.hack
        (RunID)94,   // SHORT / CACHE / ricochet.hack
        (RunID)91,   // SHORT / CACHE / suppunch.hack
        (RunID)88,   // SHORT / CACHE / wpnmstr.hack
        (RunID)6,    // LONG / NODE 5
        (RunID)28,   // LONG / NODE 5B
        (RunID)29,   // LONG / NODE 5C
        (RunID)71,   // LONG / NODE 5D
        (RunID)7,    // LONG / NODE 6
        (RunID)30,   // LONG / NODE 6B
        (RunID)37,   // LONG / NODE 6C
        (RunID)8,    // LONG / NODE 7
        (RunID)31,   // LONG / NODE 7B
        (RunID)36,   // LONG / NODE 7C
        (RunID)79,   // LONG / CACHE / berserk.hack
        (RunID)96,   // LONG / CACHE / dthstomp.hack
        (RunID)95,   // LONG / CACHE / shotflow.hack
        (RunID)83,   // LONG / CACHE / killheal.hack
        (RunID)98,   // LONG / CACHE / killreload.hack
        (RunID)97,   // LONG / CACHE / lightreflx.hack
        (RunID)75,   // ENCRYPTED / ADDICT
        (RunID)76,   // ENCRYPTED / NINDŻA
        (RunID)77,   // ENCRYPTED / DOG
        (RunID)9,    // CORE / NODE 8
        (RunID)32,   // CORE / NODE 8A
        (RunID)800,  // CORE / NODE 8B
        (RunID)34,   // CORE / NODE 8C
        (RunID)35,   // CORE / NODE 8D
    ];

    public static void Remap(string levelstream)
    {
        Plugin.Logger.LogDebug($"Remapping using string:\n\t{levelstream}");
        IEnumerator<char> stream = levelstream.GetEnumerator();
        stream.MoveNext();
        IEnumerator<RunID> order = ((IEnumerable<RunID>)levelOrder).GetEnumerator();
        order.MoveNext();
        Dictionary<RunID, RunID> RemappedRuns = new();
        do
        {
            // 1. pull two characters from the stream
            RunID level = order.Current;
            order.MoveNext();
            char first = stream.Current;
            stream.MoveNext();
            char second = stream.Current;

            // 2. convert to a RunID
            int id = int.Parse(first.ToString() + second.ToString());

            // In order to save space, RunID 800 got remapped to RunID 81
            // RunID 81 doesn't correspond ot anything
            if (id == 81)
                id = 800;

            RunID runID = (RunID)id;     

            // 3. Create mapping
            Plugin.Logger.LogDebug($"Remapping {level} ({(int)level} to {runID} ({(int)runID})");
            RemappedRuns[level] = runID;
        } while (stream.MoveNext());

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
                    if (col.Type != CellType.Run && col.Type != CellType.Terminal)
                        continue;
                    
                    if (col.Run != null) {
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

                    if (!RemappedRuns.ContainsKey(col.Run.RunID))
                        continue;

                    RunID remapped = RemappedRuns[col.Run.RunID];
                    if (!newCells.ContainsKey(remapped))
                    {
                        Plugin.Logger.LogError($"\tCouldn't find cell {(int)remapped}");
                        throw new KeyNotFoundException();
                    }
                    
                    // Probably don't need to copy construct
                    PyramidCellData newCell = new(newCells[remapped]);
                    newCell.keyToUnlockAfterCompletion = row.Column[i].keyToUnlockAfterCompletion;
                    row.Column[i] = newCell;
                }
            }
        }
    }
}