using System;
using System.Collections.Generic;
using CloudMorphConsole.Handlers;
using CloudMorphConsole.Messages;
using CloudMorphPackagingTools;

namespace CloudMorphConsole
{
    public class CommandObject
    {
        public string Command { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public bool Package { get; set; }
    }

    class Program
    {


        static void Main(string[] args)
        {
            var command = Args.Configuration.Configure<CommandObject>().CreateAndBind(args);

            string TempFolder = @"c:\temp";
            string JobWrapper = @"C:\Dev\BlueMetal\CloudAbstractions\CloudMorph\CloudMorphToolsPackage\JobHostWrapper\bin\Debug\JobHostWrapper.zip";

            Action<string, string> deploy = (from, to) => new DeployJobHandler().Handle(new DeployJobMessage() {PackagePath = from, ToUri = to});
            Func<string, IEnumerable<PackageCompressor.FileMapInfo>, string, string, string> package = (from, mp, to, packageName) =>
                                                       {
                                                           var ph = new PackageHandler();
                                                           ph.Handle(new PackageMessage {PackageName = from, SourceMap = mp, PackageFolderDestination = to, UniquePackageName = packageName});
                                                           return ph.PackagePath;
                                                       };
            Func<string, string, IEnumerable<PackageCompressor.FileMapInfo>> map = (from, packageName) =>
                                                                               {
                                                                                   var fm = new PackageMetadataHandler();
                                                                                   fm.Handle(new FileMapMessage()
                                                                                                 {SourcePath = from,
                                                                                                 PackageName = packageName});

                                                                                   return fm.Map;
                                                                               };

            switch (command.Command.ToLower())
            {
                case "deploy":
                    var uniquPackageName = new PackageNameCreator().GetUniquePackageName(command.Source);
                    var fileMap = map(command.Source, uniquPackageName);
                    string path = package(command.Source, fileMap, TempFolder, uniquPackageName);
                    deploy(path, command.Destination);
                    break;

                case "package":
                    var uniquPackageName1 = new PackageNameCreator().GetUniquePackageName(command.Source);
                    var fileMap1 = map(command.Source, uniquPackageName1);
                    string path1 = package(command.Source, fileMap1, TempFolder, uniquPackageName1);
                    new PackageHandler().MergePackages(path1, JobWrapper, uniquPackageName1);
                    deploy(path1, command.Destination);
                    break;
            }
        }
    }
}
