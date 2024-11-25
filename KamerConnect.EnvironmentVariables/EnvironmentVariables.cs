using DotNetEnv;
using System.Reflection;

namespace KamerConnect.EnvironmentVariables;

public class EnvVariables
{
    public static void Load()
    {
        var assembly = Assembly.GetExecutingAssembly();

        string resourceName = "KamerConnect.EnvironmentVariables.local.env";

        if (assembly != null)
        {
            using (Stream? stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream != null)
                {
                    using (var reader = new StreamReader(stream))
                    {
                        Env.LoadContents(reader.ReadToEnd());
                    }
                }
                else
                {
                    Console.WriteLine("The embedded resource is not found.");
                }
            }
        }
    }
}