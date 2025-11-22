using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlanner.PresentationLayer.ViewModels
{
    public class CategoriesViewVM : ViewModelBase
    {
    }
}

/*
  <!-- 
        Syfte:
Administrera kategorier användaren kan välja.

⭐ Funktionalitet:

Visa lista över kategorier

Lägg till kategori

Redigera namn

Ta bort kategori (om inte använd)

MVVM-design:

CategoryService → GetAll, Add, Rename, Delete

CategoryViewModel → Commands för Add/Delete
        
        -->
 
 */
