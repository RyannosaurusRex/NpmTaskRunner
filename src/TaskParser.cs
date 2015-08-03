using System.Collections.Generic;

namespace BroccoliTaskRunner
{
    class TaskParser
    {
        public static Dictionary<string, string> LoadTasks(string configPath)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            dic.Add("serve", "broccoli serve");
            dic.Add("build", "broccoli build dist");

            return dic;
        }
    }
}
