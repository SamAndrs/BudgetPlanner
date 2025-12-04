using BudgetPlanner.DomainLayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlanner.PresentationLayer.ViewModels
{
    public class SettingsViewVM : ViewModelBase
    {

        public UserSettingsService Settings { get; }

        public SettingsViewVM(UserSettingsService settings)
        {
            Settings = settings;
        }
    }

}