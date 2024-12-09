namespace AdventOfCode.Days;

public class Day09 : BaseDay
{
    private List<DataFile> _files;

    public override ValueTask<string> Solve_1()
    {
        _files = GetDataFiles();

        var lastFileIndex = _files.Count - 1;
        var transferComplete = false;
        foreach (var file in _files)
        {
            if (transferComplete)
                break;

            var endFile = _files[lastFileIndex];

            if (file.Id == endFile.Id)
                break;

            while (file.FreeSpace > 0)
            {
                if (endFile.Data.Count == 0)
                {
                    lastFileIndex--;
                    endFile = _files[lastFileIndex];

                    if (file.Id == endFile.Id)
                    {
                        transferComplete = true;
                        break;
                    }
                }

                file.Data.Enqueue(endFile.Data.Dequeue());
                endFile.FreeSpace++;
                file.FreeSpace--;
            }
        }

        var filePosition = 0UL;
        var checksum = 0UL;

        foreach (var item in _files.SelectMany(file => file.Data))
        {
            checksum += (ulong)item * filePosition;
            filePosition++;
        }

        return new(checksum.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        _files = GetDataFiles();
        
        var fileQueue = new Dictionary<int, PriorityQueue<DataFile, int>>();
        for (var i = 9; i >= 0; i--)
        {
            var filesByFreeSpace = _files.Where(x => x.FreeSpace == i);
            fileQueue.Add(i, new(filesByFreeSpace.Select(x => (x, x.Id))));
        }

        for (var i = _files.Count - 1; i >= 0; i--)
        {
            var file = _files[i];
            var queueIndex = -1;

            var minIndex = _files.Count;
            for (var j = file.OriginalDataCount; j <= 9; j++)
            {
                if (fileQueue[j].Count == 0 || fileQueue[j].Peek().Id >= minIndex) continue;
                
                minIndex = fileQueue[j].Peek().Id;
                queueIndex = j;
            }

            if (queueIndex == -1 || minIndex > file.Id)
                continue;

            var queue = fileQueue[queueIndex];
            var priorityFile = queue.Dequeue();

            while (priorityFile.FreeSpace > 0 && file.Data.Any(x => x == file.Id))
            {
                priorityFile.Data.Enqueue(file.Data.Dequeue());
                priorityFile.FreeSpace--;
            }

            fileQueue[priorityFile.FreeSpace].Enqueue(priorityFile, priorityFile.Id);
        }

        var checksum = 0UL;
        var filePosition = 0;

        foreach (var item in _files)
        {
            var originalDataCount = item.Data.Count(x => x == item.Id);

            if (originalDataCount == 0)
                filePosition += item.OriginalDataCount;

            foreach (var data in item.Data)
            {
                checksum += (ulong)data * (ulong)filePosition;
                filePosition++;
            }

            if (item.FreeSpace > 0)
                filePosition += item.FreeSpace;
        }
        
        return new(checksum.ToString());
    }

    private List<DataFile> GetDataFiles()
    {
        return File.ReadAllText(InputFilePath)
            .Chunk(2)
            .Select((x, i) => new DataFile
            {
                Id = i,
                Data = new(Enumerable.Repeat(i, x[0] - 48)),
                FreeSpace = x.Length == 2 ? x[1] - 48 : 0,
                OriginalDataCount = x[0] - 48
            })
            .ToList();
    }
}

internal class DataFile
{
    public int Id { get; set; }
    public Queue<int> Data { get; set; }
    public int OriginalDataCount { get; set; }
    public int FreeSpace { get; set; }
}