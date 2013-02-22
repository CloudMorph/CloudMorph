using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using JobAbstractions;
using log4net;

namespace JobHostWrapper
{
    public static class JobUtilities
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(JobUtilities));

        public static IEnumerable<string> GetLocalFiles(this IEnumerable<object> input, string workFolder)
        {
            return from i in input
                   from name in Directory.GetFiles(workFolder, "*.dll")
                   select name;
        }

        // TODO: clean this
        public static void Double()
        {
            IEnumerable<Type> possibleTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => x.IsInterface == false)
                .Where(t => t.GetInterfaces().Any(IsBootstrapperType));
        }

        static bool IsBootstrapperType(Type t)
        {
            if (t.IsGenericType)
                return t.GetGenericTypeDefinition() == typeof(IJobPackage<>);
            return false;
        }

        public static IEnumerable<Func<object>> GetActivators(this IEnumerable<string> fileList, Type type)
        {
            return from file in fileList
                   let assembly = Assembly.ReflectionOnlyLoadFrom(file)
                   from potential in assembly.FindDerivedTypesFromAssembly(type)
                   select new Func<object>(
                              () =>
                                  {

                                      //Type makeme = d1.MakeGenericType(typeArgs);
                                    return Activator.CreateInstanceFrom(file, potential.FullName).Unwrap();   
                                  }
                   );
        }

        public static IEnumerable<Func<object>> GetActivatorsInAppDomain(this IEnumerable<string> fileList, Type type)
        {
            var newDomain = AppDomain.CreateDomain("test");

            return from file in fileList
                   let assembly = newDomain.Load(file)
                   from potential in assembly.FindDerivedTypesFromAssembly(type)
                   select new Func<object>(
                              () =>
                              {

                                  //Type makeme = d1.MakeGenericType(typeArgs);
                                  return Activator.CreateInstanceFrom(file, potential.FullName).Unwrap();
                              }
                   );
        }

        public static IEnumerable<Type> FindDerivedTypesFromAssembly(this Assembly assembly, Type baseType)
        {
            if (assembly == null)
                throw new ArgumentNullException("assembly", "Assembly must be defined");
            if (baseType == null)
                throw new ArgumentNullException("baseType", "Parent Type must be defined");

            // get all the types
            Type[] types = null;

            try
            {
                types = assembly.GetExportedTypes();
            }
            catch (Exception e)
            {
                Log.Error("Failed to get types from assembly: " + assembly, e);

                yield break;
            }

            // works out the derived types
            foreach (var type in types)
            {
                // if classOnly, it must be a class
                // useful when you want to create instance
                if (!type.IsClass)
                    continue;

                if (baseType.IsInterface)
                {
                    var it = type.GetInterface(baseType.FullName);

                    if (it != null)
                        // add it to result list
                        yield return type;
                }
                else if (type.IsSubclassOf(baseType))
                {
                    // add it to result list
                    yield return type;
                }
            }
        }

        public static IEnumerable<object> PrepareEnvironment()
        {
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += CurrentDomainReflectionOnlyAssemblyResolve;

            yield return null;

            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -= CurrentDomainReflectionOnlyAssemblyResolve;
            AppDomain.CurrentDomain.UnhandledException -= OnUnhandledException;
        }


        static Assembly CurrentDomainReflectionOnlyAssemblyResolve(object sender, ResolveEventArgs args)
        {
            AssemblyName name = new AssemblyName(args.Name);
/*
            string assemblyPath = Path.Combine(
                (string)AppDomain.CurrentDomain.GetData(currentAssemblyKey),
                name.Name + ".dll");
*/
            string workFolder = Path.GetDirectoryName(args.RequestingAssembly.Location);

            var assemblyPath = Path.Combine(workFolder, name.Name + ".dll");

            if (File.Exists(assemblyPath))
            {
                // The dependency was found in the same directory as the base
                return Assembly.ReflectionOnlyLoadFrom(assemblyPath);
            }
            else
            {
                // Wasn't found on disk, hopefully we can find it in the GAC...
                return Assembly.ReflectionOnlyLoad(args.Name);
            }
        }

        static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //_log.ErrorFormat("Unhandled {0}exception in app domain {1}: {2}", e.IsTerminating ? "terminal " : "",
            //                AppDomain.CurrentDomain.FriendlyName,
            //                e.ExceptionObject);

            //Dispose();
        }
    }
}