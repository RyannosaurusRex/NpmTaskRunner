using System.Collections.Generic;

namespace TaskRunners.Ember
{
    /// <summary>
    /// Parses the tasks out of the file that is specified in <paramref name="configPath"/>. 
    /// In Broccoli and Ember-CLI's case, these are always the same (as opposed to Grunt/Gulp tasks that can be named whatever you want), so they are essentially hard-coded.
    /// </summary>
    class TaskParser
    {
        /// <summary>
        /// Loads up a set of broccoli tasks into a Dictionary where the key is the name 
        /// in the task runner window and the value is the corresponding 
        /// command line command that is run.
        /// </summary>
        /// <param name="configPath"></param>
        /// <returns></returns>
        public static Dictionary<string, string> LoadTasks(string configPath)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            
            dic.Add("serve", "ember serve");
            dic.Add("build", "ember build");
            dic.Add("build production", "ember build --environment=production");
            dic.Add("test", "ember test");

            return dic;
        }
    }
}
