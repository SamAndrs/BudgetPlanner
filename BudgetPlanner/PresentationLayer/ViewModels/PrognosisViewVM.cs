using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlanner.PresentationLayer.ViewModels
{
    public class PrognosisViewVM : ViewModelBase
    {
    }
}

/*
  <!-- 
           Syfte:
        Visa beräkning av nästa månads budget baserat på återkommande poster.

⭐ Innehåll:

Kort med:

”Förväntade inkomster”

”Förväntade utgifter”

”Prognostiserat saldo”

Lista: ”Återkommande poster som påverkar nästa månad”

Möjlighet att justera:

Frånvaro

Extra inkomster

Engångskostnader

MVVM-idé:

PrognosisService → CalculateNextMonthPrognosis()

Spara resultat i Prognosis-tabell
        -->
 
 */
