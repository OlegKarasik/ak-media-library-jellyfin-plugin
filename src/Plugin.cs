using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Serialization;

public class Plugin : BasePlugin<PluginConfiguration>
{
    public override string Name => "AK Media Library Plugin";
    public override Guid Id     => Guid.Parse("22A701F7-F48D-47D9-9897-90347F8A6B6D");

    public Plugin(
      IApplicationPaths applicationPaths, 
      IXmlSerializer xmlSerializer) 

      : base(applicationPaths, xmlSerializer)
    {
    }
}
