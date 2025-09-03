using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRF.Utilities.Extensions;

namespace VRF.DataSources.CommandLine
{
    /// <summary>
    /// Utility парсер для командной строки, запрещает прямой доступ к командной строке
    /// и делает работу с ними простой, универсальной и предсказуемой 
    /// </summary>
    public class CommandLineParser
    {
        private readonly bool debug;
        private readonly string[] args;
        
        public IReadOnlyList<string> Args => args;
        
        public bool IsEmpty() => args.Length == 0;

        // Данные командной строки в этом парсере делятся на 2 типа
        
        // tag - тэг для парсинга, всегда начинается с - (-s, --scenario)
        // tag может быть существовать сам по себе, без данных
        
        // data - данные, которые стоят после тэга, как бы к нему привязанные
        // data может быть в виде массива, так и в виде одной переменной
        
        public CommandLineParser(bool debug = false)
        {
            args = System.Environment.GetCommandLineArgs();
            this.debug = debug;
        }
        public CommandLineParser(string[] args, bool debug = false)
        {
            this.args = args;
            this.debug = debug;
        }
        
        public bool Contains(string arg) => args.Any(arg2 => arg2.Equals(arg));
        public bool ContainsAny(params string[] argsAny) => argsAny.Any(Contains);
        public bool ContainsAll(params string[] argsAll) => argsAll.All(Contains);
        
        public int IndexOf(string arg) => args.IndexOf(arg2 => arg2.Equals(arg));
        public bool IsTag(string arg) => arg.StartsWith('-');
        
        /// <summary> Получить следующий аргумент в виде данных по тэгу </summary>
        public bool GetDataByTag(out string outData, string tag)
        {
            if (!IsTag(tag))
            {
                if (debug)
                    Debug.LogError($"Arg {tag} must be tag (begins with '-')");
                outData = string.Empty;
                return false;
            }
            
            var length = args.Length - 1;
            for (var i = 0; i < length; i++)
            {
                if (args[i] == tag)
                {
                    if (IsTag(args[i + 1]))
                    {
                        outData = string.Empty;
                        return false;
                    }
                    
                    outData = args[i + 1];
                    return true;
                }
            }
            
            if (debug)
                Debug.LogWarning($"Can't find {tag} in args {string.Join(';', args)}");
            outData = string.Empty;
            return false;
        }
        /// <summary> Получить следующий аргумент в виде данных по любому из тэгов </summary>
        public bool GetDataByAnyTag(out string outData, params string[] tags)
        {
            var length = args.Length - 1;
            
            foreach (var tag in tags)
            {
                if (!IsTag(tag))
                {
                    if (debug)
                        Debug.LogError($"Arg {tag} must be tag (begins with '-')");
                    continue;
                }
            
                for (var i = 0; i < length; i++)
                {
                    if (args[i] == tag)
                    {
                        if (IsTag(args[i + 1])) continue;
                        outData = args[i + 1];
                        return true;
                    }
                }
            }

            if (debug)
                Debug.LogWarning($"Can't find any tag {string.Join(';', tags)} in args {string.Join(';', args)}");
            outData = string.Empty;
            return false;
        }
        /// <summary> Получить все последующие аргументы в виде данных после тэга </summary>
        public List<string> GetDataListByTag(string tag)
        {
            var outData = new List<string>();
            var length = args.Length - 1;
            var counter = 0;

            for (; counter < length; counter++)
            {
                if (args[counter] == tag)
                    break;
            }

            if (counter == length)
                return outData;
            
            for (counter++; counter < args.Length; counter++)
            {
                if (!IsTag(args[counter]))
                    outData.Add(args[counter]);
                else break;
            }

            return outData;
        }
        /// <summary> Получить все последующие аргументы в виде данных после первого найденного тэга </summary>
        public List<string> GetDataListByAnyTag(params string[] tags)
        {
            var outData = new List<string>();
            var length = args.Length - 1;
            var counter = 0;
            
            for (; counter < length; counter++)
            {
                if (tags.Contains(args[counter]))
                    break;
            }

            if (counter == length)
                return outData;
            
            for (counter++; counter < args.Length; counter++)
            {
                if (!IsTag(args[counter]))
                    outData.Add(args[counter]);
                else break;
            }

            return outData;
        }
    }
}