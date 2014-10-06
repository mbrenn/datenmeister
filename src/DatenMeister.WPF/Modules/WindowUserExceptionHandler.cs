using BurnSystems.UserExceptionHandler;
using DatenMeister.WPF.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.WPF.Modules
{
    public class WindowUserExceptionHandler : IUserExceptionHandler
    {
        public bool Handle(Exception exc)
        {
            var dlg = new ExceptionInfoWindow();
            dlg.SetException(exc);
            dlg.Show();

            return true;
        }
    }
}
