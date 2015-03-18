using DatenMeister.WPF.Modules.IconRepository;
using DatenMeister.WPF.Windows;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Ribbon;

namespace DatenMeister.AddOns.Scripting
{
    public static class ScriptingGui
    {
        public static void Integrate(IDatenMeisterWindow window)
        {
            var menuItem = new RibbonButton();
            menuItem.Label = Localization_DM_Addons.Menu_Scripting;
            menuItem.LargeImageSource = Injection.Application.Get<IIconRepository>().GetIcon("scripting");

            // Initializes the IScriptExecuter
            Injection.Application.Bind<IScriptExecuter>().To<ScriptExecuter>();

            menuItem.Click += (x, y) =>
                {
                    var executer = Injection.Application.Get<IScriptExecuter>();
                    var result = executer.Execute("Message.ToUser(\"Test\")");
                };

            window.AddMenuEntry(Localization_DM_Addons.Menu_Scripting, menuItem);
        }
    }
}
