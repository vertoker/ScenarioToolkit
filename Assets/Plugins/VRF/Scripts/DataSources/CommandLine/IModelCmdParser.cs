using System;
using VRF.DataSources.Interfaces;

namespace VRF.DataSources.CommandLine
{
    /// <summary>
    /// Основной интерфейс для парсера, именно от него создаются парсеры каждой из модели
    /// </summary>
    public interface IModelCmdParser : IModelHandler
    {
        public bool CanParseModel(CommandLineParser parser);
        public object ParseModel(CommandLineParser parser);
    }
}