using System;
using System.Collections.Generic;
using System.Linq;

namespace ProcessSchedulerSim
{
    public enum ProcessState { New, Ready, Running, Waiting, Terminated }

    public sealed class ProcessItem
    {
        public string PID { get; set; } = string.Empty;
        public int Arrival { get; set; }
        public int Burst { get; set; }
        public int Remaining { get; set; }
        public ProcessState State { get; set; } = ProcessState.New;

        public int StartTime { get; set; } = -1;
        public int FinishTime { get; set; } = -1;
        public int WaitingTime => (FinishTime >= 0) ? (FinishTime - Arrival - Burst) : 0;
        public int TurnaroundTime => (FinishTime >= 0) ? (FinishTime - Arrival) : 0;

        public ProcessItem CloneForRun() => new ProcessItem
        {
            PID = PID,
            Arrival = Arrival,
            Burst = Burst,
            Remaining = Burst,
            State = ProcessState.New
        };
    }

    public sealed class ScheduledSlice
    {
        public string PID { get; set; } = string.Empty;
        public int Start { get; set; }
        public int End { get; set; }
    }

    public sealed class ScheduleResult
    {
        public List<ScheduledSlice> Timeline { get; } = new();
        public List<ProcessItem> Completed { get; } = new();
        public double AvgWaiting { get; set; }
        public double AvgTurnaround { get; set; }
        public double Throughput { get; set; }
        public int Makespan { get; set; }
    }

    public static class Scheduler
    {
        public static ScheduleResult RunFCFS(IEnumerable<ProcessItem> items)
        {
            var procs = items.Select(p => p.CloneForRun()).OrderBy(p => p.Arrival).ToList();
            var res = new ScheduleResult();
            int time = 0;

            foreach (var p in procs)
            {
                if (time < p.Arrival) time = p.Arrival;
                if (p.StartTime < 0) p.StartTime = time;

                res.Timeline.Add(new ScheduledSlice { PID = p.PID, Start = time, End = time + p.Burst });
                time += p.Burst;
                p.Remaining = 0;
                p.FinishTime = time;
                p.State = ProcessState.Terminated;
                res.Completed.Add(p);
            }

            ComputeMetrics(res);
            return res;
        }

        public static ScheduleResult RunSJF(IEnumerable<ProcessItem> items)
        {
            var all = items.Select(p => p.CloneForRun()).ToList();
            var res = new ScheduleResult();
            int time = 0;
            int done = 0;

            while (done < all.Count)
            {
                var ready = all.Where(p => p.Remaining > 0 && p.Arrival <= time).OrderBy(p => p.Burst).ThenBy(p => p.Arrival).ToList();
                if (ready.Count == 0)
                {
                    var nextArrival = all.Where(p => p.Remaining > 0).Min(p => p.Arrival);
                    time = Math.Max(time, nextArrival);
                    continue;
                }
                var p = ready.First();
                if (p.StartTime < 0) p.StartTime = time;
                res.Timeline.Add(new ScheduledSlice { PID = p.PID, Start = time, End = time + p.Burst });
                time += p.Burst;
                p.Remaining = 0;
                p.FinishTime = time;
                p.State = ProcessState.Terminated;
                res.Completed.Add(p);
                done++;
            }

            ComputeMetrics(res);
            return res;
        }

        public static ScheduleResult RunRR(IEnumerable<ProcessItem> items, int quantum)
        {
            if (quantum <= 0) quantum = 1;

            var all = items.Select(p => p.CloneForRun()).OrderBy(p => p.Arrival).ToList();
            var res = new ScheduleResult();
            var queue = new Queue<ProcessItem>();
            int time = 0;
            int idx = 0;

            while (true)
            {
                while (idx < all.Count && all[idx].Arrival <= time)
                {
                    queue.Enqueue(all[idx]);
                    all[idx].State = ProcessState.Ready;
                    idx++;
                }

                if (queue.Count == 0)
                {
                    if (idx < all.Count)
                    {
                        time = Math.Max(time, all[idx].Arrival);
                        continue;
                    }
                    else break;
                }

                var p = queue.Dequeue();
                p.State = ProcessState.Running;
                if (p.StartTime < 0) p.StartTime = time;

                int slice = Math.Min(quantum, p.Remaining);
                res.Timeline.Add(new ScheduledSlice { PID = p.PID, Start = time, End = time + slice });
                time += slice;
                p.Remaining -= slice;

                while (idx < all.Count && all[idx].Arrival <= time)
                {
                    queue.Enqueue(all[idx]);
                    all[idx].State = ProcessState.Ready;
                    idx++;
                }

                if (p.Remaining > 0)
                {
                    p.State = ProcessState.Ready;
                    queue.Enqueue(p);
                }
                else
                {
                    p.State = ProcessState.Terminated;
                    p.FinishTime = time;
                    res.Completed.Add(p);
                }
            }

            ComputeMetrics(res);
            return res;
        }

        private static void ComputeMetrics(ScheduleResult res)
        {
            res.Makespan = res.Timeline.Count == 0 ? 0 : res.Timeline.Max(s => s.End);
            if (res.Completed.Count > 0)
            {
                res.AvgWaiting = res.Completed.Average(p => p.WaitingTime);
                res.AvgTurnaround = res.Completed.Average(p => p.TurnaroundTime);
                res.Throughput = (double)res.Completed.Count / Math.Max(1, res.Makespan);
            }
        }
    }
}
